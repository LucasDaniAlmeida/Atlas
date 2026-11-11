using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Models;

    public class CommandParameters
    {
        private readonly Dictionary<string, object> _values = [];

        public void Set(string key, object value)
        {
            _values[key] = value;
        }

        public T? Get<T>(string key)
        {
            if (_values.TryGetValue(key, out var value))
                return (T?)value;

            return default;
        }

        public bool Contains(string key)
        {
            return _values.ContainsKey(key);
        }
    }