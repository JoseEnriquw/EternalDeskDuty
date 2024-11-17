using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class ObjectHighlighter : MonoBehaviour
    {
        public float rayDistance = 5f; // Distancia máxima del raycast
        public Color outlineColor = Color.yellow;
        public float outlineWidth = 5f;

        private GameObject currentObject;
        [SerializeField] private Camera mainCamera;

        void Update()
        {
            // Lanza un raycast desde la cámara hacia adelante
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag(Tags.Interactive)) 
                {
                    // Si es un nuevo objeto, actualiza el resaltado
                    if (currentObject != hitObject)
                    {
                        ClearHighlight();
                        HighlightObject(hitObject);
                    }
                }
                else
                {
                    ClearHighlight();
                }
            }
            else
            {
                ClearHighlight();
            }
        }

        void HighlightObject(GameObject obj)
        {
            if (!obj.TryGetComponent<Outline>(out var outline))
            {
                outline = obj.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = outlineWidth;
            }
            else
            {
                outline.enabled = true;
            }

            currentObject = obj;
        }

        void ClearHighlight()
        {
            if (currentObject != null)
            {
                if (currentObject.TryGetComponent<Outline>(out var outline))
                {
                    outline.enabled = false;
                }
                currentObject = null;
            }
        }
    }

}
