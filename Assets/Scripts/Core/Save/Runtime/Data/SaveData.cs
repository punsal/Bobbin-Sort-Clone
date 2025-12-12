using System;

namespace Core.Save.Data
{
    [Serializable]
    public abstract class SaveData
    {
        public abstract string Key { get; }
    }
}