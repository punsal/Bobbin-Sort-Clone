using Core.Save.Data;

namespace Gameplay.Data
{
    public sealed class GameSaveData : SaveData
    {
        public override string Key => "GameSaveData";

        public int LastLevelIndex = 0;
    }
}