using Newtonsoft.Json;

namespace HuaweiSolar
{
    /// <summary>
    /// The HuaweiInvalidNAValueConverter was introduced because the Huawei FusionSolar API was returning the value "N/A" as a string instead of a close_time which should
    /// be the EPOC time the system last shutdown. This valid caused a JSON deserialization error that was not handled, close_time isn't important so assume N/A is 0
    /// is fine for the handling of the invalid value.
    /// </summary>
    public class HuaweiInvalidNAValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                if (objectType == typeof(Double))
                {
                    return (double)0;
                }
                else if (objectType == typeof(Int64))
                {
                    return (long)0;
                }
                else if (objectType == typeof(Nullable<Double>))
                {
                    return null;
                }
                else if (objectType == typeof(Nullable<Int64>))
                {
                    return null;
                }
                else
                {
                    return 0;
                }
            }
            
            if (reader.ValueType == typeof(string))
            {
                string value = (string)reader.Value;
                // It was returning N/A in upper case but since they aren't matching a schema just be safe and capture anything
                if (value.Equals("N/A", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (objectType == typeof(Double))
                    {
                        return (double)0;
                    }
                    else if (objectType == typeof(Int64))
                    {
                        return (long)0;
                    }
                    else if (objectType == typeof(Nullable<Double>))
                    {
                        return null;
                    }
                    else if (objectType == typeof(Nullable<Int64>))
                    {
                        return null;
                    }
                    else 
                    {
                        return 0;
                    }
                }
                else
                {
                    if (Int64.TryParse(value, out long close_time))
                    {
                        return close_time;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (reader.ValueType == typeof(Int64))
            {
                return reader.Value;
            }
            else if (reader.ValueType == typeof(Double))
            {
                return reader.Value;
            }
            else
            {
                return 0;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}