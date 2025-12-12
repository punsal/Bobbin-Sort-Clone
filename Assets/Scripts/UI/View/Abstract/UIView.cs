using UnityEngine;

namespace UI.View.Abstract
{
    public abstract class UIView : MonoBehaviour
    {
        [SerializeField] private GameObject view;

        private void Awake()
        {
            if (view == null)
            {
                Debug.LogError("View is null");
            }
        }

        private void OnValidate()
        {
            if (view != null)
            {
                return;
            }
            Debug.LogWarning("View is null, assigning attached gameObject.");
            view = GetComponent<GameObject>();
        }

        public void SetVisible(bool isVisible)
        {
            view.SetActive(isVisible);
        }
    }
}
