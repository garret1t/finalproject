using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public static class Save
    {
        private static Dictionary<string, object> info = new Dictionary<string, object>();

        public static void SetSavedData<T>(string k, T v)
        {
            if (!info.ContainsKey(k)) info.Add(k, v);
            else info[k] = v;
        }
        public static T GetSavedData<T>(string key)
        {
            if (!info.ContainsKey(key)) return default(T);
            else
            {
                if (info[key] is T)
                    return (T)info[key];
                else
                {
                    Console.Error.WriteLine("Cannot cast " + info[key].GetType().ToString() + " to " + typeof(T).ToString() + ". Return default value of " + typeof(T).ToString() + ".");
                    Console.Error.WriteLine("Error encountered while attempting to retrieve Saved Data. Key: " + key);
                    return default(T);
                }
            }
        }
        public static T GetSavedData<T>(string key, T defaultReturn)
        {
            if (!HasSavedData(key)) return defaultReturn;
            else return GetSavedData<T>(key);
        }
        public static bool HasSavedData(string k)
        {
            return info.ContainsKey(k);
        }
    }
}
