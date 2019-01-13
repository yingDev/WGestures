﻿using System;
using System.Collections;
 using System.Collections.Generic;
 using System.Linq;

namespace WGestures.Common.Config
{
    public abstract class AbstractDictConfig : IConfig
    {
        private Dictionary<string, object> _dict =
            new Dictionary<string, object>();
        protected Dictionary<string, object> Dict
        {
            get { return _dict; }
            set { _dict = value; }
        }

        public bool IsSet(string key)
        {
            return _dict.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            object val;
            if (_dict.TryGetValue(key, out val))
            {                    
                return (T) val;
            }
                
            var t = Nullable.GetUnderlyingType(typeof(T));
            if (t != null)
            {
                return default(T);
            }

            throw new KeyNotFoundException(key);
        }

        public T Get<T>(string key, T defaultValue)
        {
            try
            {
                return Get<T>(key);
            }
            catch (KeyNotFoundException)
            {

                return defaultValue;
            }

        }

        public void Set<T>(string key, T value)
        {
            _dict[key] = value;
        }

        public abstract void Save();

        public void Import(params IConfig[] from)
        {
            if (from == null || !@from.Any()) return;

            foreach (var config in from)
            {
                if(config == null) continue;

                foreach (var kv in config)
                {
                    Set(kv.Key, kv.Value);
                }
            }

        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dict.GetEnumerator();
        }
    }

}
