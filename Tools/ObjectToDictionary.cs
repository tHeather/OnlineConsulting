using System;
using System.Collections.Generic;

namespace OnlineConsulting.Tools
{
    public class ObjectToDictionary
    {
        public static Dictionary<string,string> ToDictionary(object source)
        {
            var dictionary = new Dictionary<string, string>();
            if(source == null) return dictionary;

            foreach (var property in source.GetType().GetProperties())
            {
                var value = property.GetValue(source);
                if (value == null) continue;

                string valueString;

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.DateTime:
                        DateTime valueDateTime = (DateTime)value;   
                        valueString = valueDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
                        break;

                    case TypeCode.Int32:
                        valueString = value.ToString();
                        break;

                    default: 
                        valueString = (string)value;
                        break;
                }

                dictionary.Add(property.Name, valueString);
            }

            return dictionary;
        }
        
    }
 }
