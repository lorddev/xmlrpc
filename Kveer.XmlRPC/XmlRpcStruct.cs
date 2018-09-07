/* 
XML-RPC.NET library
Copyright (c) 2001-2009, Charles Cook <charlescook@cookcomputing.com>

Permission is hereby granted, free of charge, to any person 
obtaining a copy of this software and associated documentation 
files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be 
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CookComputing.XmlRpc
{
    public class XmlRpcStruct : Dictionary<string, object>, IDictionary, ISerializable, IDeserializationCallback, ICloneable
    {
        private readonly List<string> _keys = new List<string>();
        private readonly List<object> _values = new List<object>();
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        private readonly object _syncRoot = new object();

        public void Add(object key, object value)
        {
            if (!(key is string))
            {
                throw new ArgumentException("XmlRpcStruct key must be a string.");
            }

            _dictionary.Add(key as string, value);
            _keys.Add(key as string);
            _values.Add(value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _keys.Clear();
            _values.Clear();
        }

        public bool Contains(object key)
        {
            return _dictionary.ContainsKey(key as string);
        }

        public bool ContainsKey(object key)
        {
            return _dictionary.ContainsKey(key as string);
        }

        public bool ContainsValue(object value)
        {
            return _dictionary.ContainsValue(value as string);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new XmlRpcStruct.Enumerator(_keys, _values);
        }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public ICollection Keys => _keys;

        public void Remove(object key)
        {
            _dictionary.Remove(key as string);
            int idx = _keys.IndexOf(key as string);
            if (idx >= 0)
            {
                _keys.RemoveAt(idx);
                _values.RemoveAt(idx);
            }
        }

        public ICollection Values => _values;

        public object this[object key]
        {
            get { return _dictionary[key as string]; }
            set
            {
                if (!(key is string))
                {
                    throw new ArgumentException("XmlRpcStruct key must be a string.");
                }

                _dictionary[key as string] = value;
                _keys.Add(key as string);
                _values.Add(value);
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException(); // TODO: implement
        }

        public int Count => _dictionary.Count;

        public bool IsSynchronized => false;

        public object SyncRoot => _syncRoot;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new XmlRpcStruct.Enumerator(_keys, _values);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException(); // TODO: implement
        }

        int ICollection.Count => _dictionary.Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => _syncRoot;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException(); // TODO: implement
        }

        public void OnDeserialization(object sender)
        {
            throw new NotImplementedException(); // TODO: implement
        }

        public object Clone()
        {
            throw new NotImplementedException(); // TODO: implement
        }

        private class Enumerator : IDictionaryEnumerator
        {
            private readonly List<string> _keys;
            private readonly List<object> _values;
            private int _index;

            public Enumerator(List<string> keys, List<object> values)
            {
                _keys = keys;
                _values = values;
                _index = -1;
            }

            public void Reset()
            {
                _index = -1;
            }

            public object Current
            {
                get
                {
                    CheckIndex();
                    return new DictionaryEntry(_keys[_index], _values[_index]);
                }
            }

            public bool MoveNext()
            {
                _index++;
                if (_index >= _keys.Count)
                    return false;
                else
                    return true;
            }

            public DictionaryEntry Entry
            {
                get
                {
                    CheckIndex();
                    return new DictionaryEntry(_keys[_index], _values[_index]);
                }
            }

            public object Key
            {
                get
                {
                    CheckIndex();
                    return _keys[_index];
                }
            }

            public object Value
            {
                get
                {
                    CheckIndex();
                    return _values[_index];
                }
            }

            private void CheckIndex()
            {
                if (_index < 0 || _index >= _keys.Count)
                    throw new InvalidOperationException(
                        "Enumeration has either not started or has already finished.");
            }
        }
    }
}