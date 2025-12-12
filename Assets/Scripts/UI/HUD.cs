using Gameplay.Level.Observables;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private LevelIndexObservable levelIndexObservable;
        
        [Header("References")]
        [SerializeField] private TextMeshProUGUI levelText;

        [Header("Configuration")]
        [SerializeField] private int levelIndexDisplayOffset = 1;
        
        private void OnEnable()
        {
            if (levelIndexObservable != null)
            {
                levelIndexObservable.OnChanged += OnLevelIndexChanged;
                OnLevelIndexChanged(levelIndexObservable.Value);
            }
        }

        private void OnDisable()
        {
            if (levelIndexObservable != null)
            {
                levelIndexObservable.OnChanged -= OnLevelIndexChanged;
            }
        }

        private void OnLevelIndexChanged(int newLevelIndex)
        {
            if (levelText == null)
            {
                Debug.LogError("LevelText is null");
                return;
            }
            
            levelText.text = $"{newLevelIndex + levelIndexDisplayOffset}";
        }
    }
}
