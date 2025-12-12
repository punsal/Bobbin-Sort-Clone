using Core.Event.Abstract;
using UI.View.Abstract;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIView winView;
        [SerializeField] private UIView loseView;

        [Header("Events")]
        [SerializeField] private AScriptableEvent levelStartEvent;
        [SerializeField] private AScriptableEvent levelWinEvent;
        [SerializeField] private AScriptableEvent levelLoseEvent;

        private void Awake()
        {
            HideViews();
        }

        private void OnEnable()
        {
            if (levelStartEvent != null)
            {
                levelStartEvent.OnRaised += HideViews;
            }
            
            if (levelWinEvent != null)
            {
                levelWinEvent.OnRaised += ShowWinView;
            }

            if (levelLoseEvent != null)
            {
                levelLoseEvent.OnRaised += ShowLoseView;
            }
        }

        private void OnDisable()
        {
            if (levelStartEvent != null)
            {
                levelStartEvent.OnRaised -= HideViews;
            }
            
            if (levelWinEvent != null)
            {
                levelWinEvent.OnRaised -= ShowWinView;
            }

            if (levelLoseEvent != null)
            {
                levelLoseEvent.OnRaised -= ShowLoseView;
            }
        }

        private void HideViews()
        {
            winView.SafeSetVisible(false);
            loseView.SafeSetVisible(false);
        }
        
        private void ShowWinView()
        {
            loseView.SafeSetVisible(false);
            winView.SafeSetVisible(true);
        }
        
        private void ShowLoseView()
        {
            winView.SafeSetVisible(false);
            loseView.SafeSetVisible(true);
        }
    }
}
