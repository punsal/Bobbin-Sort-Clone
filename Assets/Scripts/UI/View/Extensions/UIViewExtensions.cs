using UI.View.Abstract;
using UnityEngine;

namespace UI
{
    public static class UIViewExtensions
    {
        public static void SafeSetVisible(this UIView view, bool isVisible)
        {
            if (view != null)
            {
                view.SetVisible(isVisible);
            }
            else
            {
                Debug.LogWarning("View is null");
            }
        }
    }
}