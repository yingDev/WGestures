using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WGestures.Core;
using WGestures.Core.Persistence.Impl;

namespace SerializeWrapper
{
    public class SerializeWrapper : MarshalByRefObject, JsonGestureIntentStore.ISerializeWrapper
    {
        public string FileVersion { get; set; }
        public Dictionary<string, ExeApp> Apps { get; set; }
        public GlobalApp Global { get; set; }

        public void DeserilizeFromFile(string filename, string version)
        {
            using (var file = new FileStream(filename, FileMode.Open))
            {
                DeserializeFromStream(file, version, true);
            }
        }

        public void DeserializeFromStream(Stream s, string version, bool closeStream = false)
        {
            try
            {
                using (var txtReader = new StreamReader(s))
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
                    ser.Populate(jsonReader, this);

                }
            }
            finally
            {
                if(closeStream) s.Dispose();
            }

        }

        public void SerializeTo(string fileName)
        {
            using (var fs = new StreamWriter(fileName))
            {
                var ser = new JsonSerializer();
                ser.Formatting = Formatting.None;
                ser.TypeNameHandling = TypeNameHandling.Auto;

                ser.Serialize(fs, this);
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
