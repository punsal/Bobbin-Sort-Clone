using UnityEngine;

namespace Gameplay.Level.Config
{
    [CreateAssetMenu(menuName = "Level/Database")]
    public class LevelDatabase : ScriptableObject
    {
        public LevelDefinition[] levels;

        public LevelDefinition GetLevel(int index)
        {
            if (levels == null || levels.Length == 0)
            {
                Debug.LogError("LevelDatabase is empty.");
                return null;
            }

            if (index < 0 || index >= levels.Length)
            {
                Debug.LogWarning($"Requested level index {index} is out of range. Modulate the index to fit.");
                index %= levels.Length;
            }

            return levels[index];
        }
    }
}