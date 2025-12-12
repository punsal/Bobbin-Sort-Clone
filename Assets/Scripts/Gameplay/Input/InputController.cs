using UnityEngine;

namespace Gameplay.Input
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Camera inputCamera;
        [SerializeField] private LayerMask inputLayer;

        private void Awake()
        {
            if (inputCamera != null)
            {
                return;
            }
        
            Debug.Log("Input Camera is null");
            inputCamera = Camera.main;
        }
    
        public bool DidTap<T>(out T result) where T : Component
        {
            result = null;
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                return DidOverlap(out var hitCollider) && hitCollider.TryGetComponent(out result);
            }
        
            return false;
        }

        private bool DidOverlap(out Collider2D result)
        {
            var screenPosition = UnityEngine.Input.mousePosition;
            var worldPosition = (Vector2)inputCamera.ScreenToWorldPoint(screenPosition);
            result = Physics2D.OverlapPoint(worldPosition, inputLayer);
            return result != null;
        }
    }
}