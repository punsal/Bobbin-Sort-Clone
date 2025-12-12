using Gameplay.Level.Config;
using UnityEngine;

namespace Gameplay.Level
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private Transform levelRoot;

        private LevelController _currentLevel;

        public LevelController LoadLevel(int levelIndex)
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);
                _currentLevel = null;
            }

            var def = levelDatabase.GetLevel(levelIndex);
            if (def == null || def.levelPrefab == null)
                return null;

            _currentLevel = Instantiate(def.levelPrefab, levelRoot);
            _currentLevel.Initialize(def);

            return _currentLevel;
        }
    }
}