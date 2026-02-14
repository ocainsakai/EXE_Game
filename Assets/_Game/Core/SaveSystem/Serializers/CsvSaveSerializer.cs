using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using UnityEngine;

namespace _Game.Core.SaveSystem.Serializers
{
    public class CsvSaveSerializer : ISaveSerializer
    {
        public string FileExtension => "csv";

        public string Serialize(object data)
        {
            if (data == null) return string.Empty;

            var type = data.GetType();
            var sb = new StringBuilder();

            if (data is IEnumerable list)
            {
                bool headerWritten = false;
                foreach (var item in list)
                {
                    var itemType = item.GetType();
                    var fields = itemType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    
                    if (!headerWritten)
                    {
                        sb.AppendLine(string.Join(",", fields.Select(f => f.Name)));
                        headerWritten = true;
                    }
                    
                    sb.AppendLine(string.Join(",", fields.Select(f => SerializeValue(f.GetValue(item)))));
                }
            }
            else
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                sb.AppendLine(string.Join(",", fields.Select(f => f.Name)));
                sb.AppendLine(string.Join(",", fields.Select(f => SerializeValue(f.GetValue(data)))));
            }

            return sb.ToString();
        }

        public T Deserialize<T>(string content)
        {
            // Note: Simple CSV deserialization logic
            // This is a basic implementation and might need refinement for nested objects
            if (string.IsNullOrEmpty(content)) return default;

            var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) return default;

            var headers = lines[0].Split(',');
            var values = lines[1].Split(',');

            var result = Activator.CreateInstance<T>();
            for (int i = 0; i < headers.Length; i++)
            {
                var field = typeof(T).GetField(headers[i], BindingFlags.Public | BindingFlags.Instance);
                if (field != null && i < values.Length)
                {
                    field.SetValue(result, Convert.ChangeType(values[i], field.FieldType));
                }
            }

            return result;
        }

        private string SerializeValue(object val)
        {
            if (val == null) return "";
            string s = val.ToString();
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
            {
                return $"\"{s.Replace("\"", "\"\"")}\"";
            }
            return s;
        }
    }
}
