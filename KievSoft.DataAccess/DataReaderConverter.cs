using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KievSoft.DataAccess
{
    public class DataReaderConverter<T> where T : new()
    {
        private class Mapping
        {
            public int Index;
            public PropertyInfo Property;
        }

        private Mapping[] _mappings;
        private DbDataReader _lastReader;

        public T Convert(DbDataReader reader)
        {
            if (_mappings == null || reader != _lastReader)
                _mappings = MapProperties(reader);

            var o = new T();

            foreach (var mapping in _mappings)
            {
                var prop = mapping.Property;
                var rawValue = reader.GetValue(mapping.Index);
                var value = DbConvert.To(prop.PropertyType, rawValue);
                prop.SetValue(o, value, null);
            }

            _lastReader = reader;

            return o;
        }

        private Mapping[] MapProperties(DbDataReader reader)
        {
            var fieldCount = reader.FieldCount;

            var fields = new Dictionary<string, int>(fieldCount);

            for (var i = 0; i < fieldCount; i++)
                fields.Add(reader.GetName(i).ToLowerInvariant(), i);

            var type = typeof(T);

            var mapping = new List<Mapping>(fieldCount);

            foreach (var prop in type.GetProperties())
            {
                var name = prop.Name.ToLowerInvariant();

                int index;

                if (fields.TryGetValue(name, out index))
                    mapping.Add(new Mapping() { Index = index, Property = prop });
            }

            return mapping.ToArray();
        }
    }
}
