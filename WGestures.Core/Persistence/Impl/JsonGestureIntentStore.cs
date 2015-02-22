using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WGestures.Core.Persistence.Impl
{

    public class JsonGestureIntentStore : IGestureIntentStore
    {
        public string FileVersion { get; set; }

        private Dictionary<string, ExeApp> ExeAppsRegistry { get; set; }
        public GlobalApp GlobalApp { get; set; }

        private string jsonPath;
        private JsonSerializer ser = new JsonSerializer();

        private JsonGestureIntentStore() { }

        public JsonGestureIntentStore(string jsonPath, string fileVersion)
        {
            FileVersion = fileVersion;
            this.jsonPath = jsonPath;
            SetupSerializer();


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

        public JsonGestureIntentStore(Stream stream, bool closeStream, string fileVersion)
        {
            FileVersion = fileVersion;
            SetupSerializer();
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
            finally
            {
                if (closeStream) stream.Dispose();
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
                Deserialize(file, false);
            }

            //todo: 完全在独立domain中加载json.net?
            /*var deserializeDomain = AppDomain.CreateDomain("jsonDeserialize");
            var wrapperRef = (SerializeWrapper) deserializeDomain.CreateInstanceAndUnwrap(typeof(SerializeWrapper).Assembly.FullName, typeof(SerializeWrapper).FullName);
            
            wrapperRef.DeserilizeFromFile(jsonPath, FileVersion);

            GlobalApp = wrapperRef.GlobalApp;
            ExeAppsRegistry = wrapperRef.ExeAppsRegistry;
            
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            AppDomain.Unload(deserializeDomain);
            
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
            */

        }

        private void SetupSerializer()
        {
            ser.Formatting = Formatting.None;
            ser.TypeNameHandling = TypeNameHandling.Auto;

            if (FileVersion.Equals("1"))
            {
                ser.Converters.Add(new GestureIntentConverter_V1());

            }else if (FileVersion.Equals("2"))
            {
                ser.Converters.Add(new GestureIntentConverter());

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

        internal class SerializeWrapper : MarshalByRefObject
        {
            [JsonProperty("FileVersion")]
            public string FileVersion { get; set; }

            [JsonProperty("Apps")]
            public Dictionary<string, ExeApp> ExeAppsRegistry { get; set; }
            
            [JsonProperty("Global")]
            public GlobalApp GlobalApp { get; set; }

            public void DeserilizeFromFile(string filename, string version)
            {
                using (var file = new FileStream(filename, FileMode.Open))
                {
                    using (var txtReader = new StreamReader(file))
                    using (var jsonReader = new JsonTextReader(txtReader))
                    {
                        var ser = new JsonSerializer();
                        ser.Formatting = Formatting.None;
                        ser.TypeNameHandling = TypeNameHandling.Auto;

                        if (version.Equals("1"))
                        {
                            ser.Converters.Add(new GestureIntentConverter_V1());

                        }
                        else if (version.Equals("2"))
                        {
                            ser.Converters.Add(new GestureIntentConverter());

                        }
                    
                        var result = ser.Deserialize<SerializeWrapper>(jsonReader);

                        FileVersion = result.FileVersion;
                        ExeAppsRegistry = result.ExeAppsRegistry;
                        GlobalApp = result.GlobalApp;
    
                    
                    }
                }
            }
        }

        internal class GestureIntentConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var dict = value as GestureIntentDict;
                serializer.Serialize(writer, dict.Values.ToList());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var dict = new GestureIntentDict();
                var list = serializer.Deserialize<List<GestureIntent>>(reader);
                foreach (var i in list)
                {
                    dict.Add(i);
                }

                return dict;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(GestureIntentDict);
            }
        }

        //.json
        internal class GestureIntentConverter_V1 : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var dict = value as GestureIntentDict;
                serializer.Serialize(writer, dict.Values.ToList());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var dict = new GestureIntentDict();
                var list = serializer.Deserialize<List<KeyValuePair<Gesture, GestureIntent>>>(reader);
                foreach (var i in list)
                {
                    Debug.WriteLine("Add Gesture: " + i.Value.Gesture);
                    dict.Add(i.Value);
                }

                return dict;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(GestureIntentDict);
            }
        }
    }
}
