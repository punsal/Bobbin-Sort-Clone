using Core.Event.Abstract;
using UI.View.Abstract;
using UnityEngine;

namespace UI.View
{
    public class WinView : UIView
    {
        [Header("Events")]
        [SerializeField] private AScriptableEvent nextLevelEvent;
        
        public void OnNextLevel()
        {
            if (nextLevelEvent == null)
            {
                Debug.LogError("NextLevelEvent is null");
                return;
            }
            
            nextLevelEvent.Raise();
        }
    }
}