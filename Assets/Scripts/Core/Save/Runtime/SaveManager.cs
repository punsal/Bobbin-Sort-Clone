using Core.Save.Data;
using UnityEngine;

namespace Core.Save
{
    public static class SaveManager
    {
        public static void Save(SaveData data)
        {
            if (data == null)
            {
                return;
            }
            
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(data.Key, json);
            PlayerPrefs.Save();
        }
        
        public static T Load<T>() where T : SaveData, new()
        {
            var temp = new T();
            var key = temp.Key;

            if (!PlayerPrefs.HasKey(key))
            {
                return temp;
            }

            var json = PlayerPrefs.GetString(key, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
            {
                return temp;
            }

            try
            {
                var data = JsonUtility.FromJson<T>(json);
                return data ?? temp;
            }
            catch
            {
                return temp;
            }
        }
        
        public static void Clear<T>() where T : SaveData, new()
        {
            var key = new T().Key;
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}