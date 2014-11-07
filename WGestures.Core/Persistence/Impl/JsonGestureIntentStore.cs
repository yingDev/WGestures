using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WGestures.Core.Persistence.Impl
{

    public class JsonGestureIntentStore : IGestureIntentStore
    {
        public string FileVersion { get; private set; }

        private Dictionary<string, ExeApp> ExeAppsRegistry { get; set; }
        public GlobalApp GlobalApp { get; set; }

        private string jsonPath;
        private JsonSerializer ser = new JsonSerializer();

        private JsonGestureIntentStore() { }

        public JsonGestureIntentStore(string jsonPath)
        {
            this.jsonPath = jsonPath;

            ser.Formatting = Formatting.None;

            ser.TypeNameHandling = TypeNameHandling.Auto;


            if (File.Exists(jsonPath))
            {
                Deserialize();
            }
            else
            {
                ExeAppsRegistry = new Dictionary<string, ExeApp>();
                GlobalApp = new GlobalApp();
            }
        }

        public JsonGestureIntentStore(Stream stream, bool closeStream)
        {
            ser.Formatting = Formatting.None;

            ser.TypeNameHandling = TypeNameHandling.Auto;

            Deserialize(stream, closeStream);
        }

        private void Deserialize(Stream stream, bool closeStream)
        {
            if(stream == null || !stream.CanRead) throw new ArgumentException("stream");

            try
            {
                using (var txtReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(txtReader))
                {
                    var result = ser.Deserialize<SerializeWrapper>(jsonReader);

                    FileVersion = result.FileVersion;

                    ExeAppsRegistry = result.ExeAppsRegistry;
                    GlobalApp = result.GlobalApp;
                }
            }
            catch (Exception)
            {
                if(closeStream) stream.Dispose();
                throw;
            }
            
        }

        private void Serialize()
        {
            using (var fs = new StreamWriter(jsonPath))
            {
                ser.Serialize(fs, new SerializeWrapper()
                {FileVersion = FileVersion, ExeAppsRegistry = ExeAppsRegistry,GlobalApp = GlobalApp});
            }
        }

        private void Deserialize()
        {

            using (var file = new FileStream(jsonPath, FileMode.Open))
            {
                Deserialize(file, true);
            }
        }



        public bool TryGetExeApp(string key, out ExeApp found)
        {
            return ExeAppsRegistry.TryGetValue(key, out found);
        }

        public ExeApp GetExeApp(string key)
        {
            return ExeAppsRegistry[key];
        }


        public void Remove(string key)
        {
            ExeAppsRegistry.Remove(key);
        }

        public void Remove(ExeApp app)
        {
            Remove(app.ExecutablePath);
        }

        public void Add(ExeApp app)
        {
            ExeAppsRegistry.Add(app.ExecutablePath, app);
        }

        public void Save()
        {
            Serialize();

        }

        public JsonGestureIntentStore Clone()
        {
            var ret = new JsonGestureIntentStore();
            ret.GlobalApp = GlobalApp;
            ret.ExeAppsRegistry = ExeAppsRegistry;
            ret.FileVersion = FileVersion;
            ret.jsonPath = jsonPath;

            return ret;
        }

        public void Import(JsonGestureIntentStore from, bool replace=false)
        {
            if (from == null) return;

            if (replace)
            {
                GlobalApp.GestureIntents.Clear();
                GlobalApp.IsGesturingEnabled = from.GlobalApp.IsGesturingEnabled;
                ExeAppsRegistry.Clear();
            }

            GlobalApp.ImportGestures(from.GlobalApp);
            
            foreach (var kv in from.ExeAppsRegistry)
            {
                ExeApp appInSelf;
                //如果应用程序已经在列表中，则合并手势
                if (TryGetExeApp(kv.Key, out appInSelf))
                {
                    appInSelf.ImportGestures(kv.Value);
                    appInSelf.IsGesturingEnabled = appInSelf.IsGesturingEnabled && kv.Value.IsGesturingEnabled;
                }
                else//否则将app添加到列表中
                {
                    Add(kv.Value);
                }
            }
        }

        public IEnumerator<ExeApp> GetEnumerator()
        {
            return ExeAppsRegistry.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal class SerializeWrapper
        {
            [JsonProperty("FileVersion")]
            public string FileVersion { get; set; }

            [JsonProperty("Apps")]
            public Dictionary<string, ExeApp> ExeAppsRegistry { get; set; }
            [JsonProperty("Global")]
            public GlobalApp GlobalApp { get; set; }
        }
    }
}
