using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoomImage : MonoBehaviour, IScrollHandler {
        private Vector3 initialScale;
        private Vector3 initialPosition;

        [SerializeField]
        private float zoomSpeed = 0.1f;
        [SerializeField]
        private float maxZoom = 10f;

        private RectTransform rectTransform;

        private void Awake() {
            initialScale = transform.localScale;
            initialPosition = transform.position;
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnScroll(PointerEventData eventData) {
            Vector2 mousePosition = eventData.position;
            
            // Convert the mouse position to local coordinates relative to the image
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, eventData.pressEventCamera, out localPoint);

            var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
            var desiredScale = transform.localScale + delta;
            
            desiredScale = ClampDesiredScale(desiredScale);
            Vector3 scaleChange = desiredScale - transform.localScale;

            // Adjust the position to zoom towards the mouse position
            Vector3 positionAdjustment = new Vector3(localPoint.x * scaleChange.x, localPoint.y * scaleChange.y, 0f);
            rectTransform.localPosition -= positionAdjustment;

            transform.localScale = desiredScale;
        }

        private Vector3 ClampDesiredScale(Vector3 desiredScale) {
            desiredScale = Vector3.Max(initialScale, desiredScale);
            desiredScale = Vector3.Min(new Vector3(maxZoom,maxZoom,maxZoom), desiredScale);
            return desiredScale;
        }
    }