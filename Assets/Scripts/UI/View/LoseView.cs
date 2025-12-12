using Core.Event.Abstract;
using UI.View.Abstract;
using UnityEngine;

namespace UI.View
{
    public class LoseView : UIView
    {
        [Header("Events")]
        [SerializeField] private AScriptableEvent retryLevelEvent;

        public void OnRetryLevel()
        {
            if (retryLevelEvent == null)
            {
                Debug.LogError("RetryLevelEvent is null");
                return;
            }
            
            retryLevelEvent.Raise();
        }
    }
}