using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AlphaGame.HelperModules
{
    public class MenuDragHandler : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public RectTransform dragRectTransform;
        public Canvas canvas;

        private void Awake() {
            if(canvas == null)
                canvas = GetComponent<Canvas>();
            if(dragRectTransform == null)
                dragRectTransform = GetComponent<RectTransform>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            dragRectTransform.SetAsLastSibling();
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}