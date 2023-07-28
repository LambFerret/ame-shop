using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.title
{
    public class HoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject ingredientDetailPanel;

        // This function will be called when the mouse pointer enters the GameObject's area
        public void OnPointerEnter(PointerEventData eventData)
        {
            ingredientDetailPanel.SetActive(true);
        }

        // This function will be called when the mouse pointer exits the GameObject's area
        public void OnPointerExit(PointerEventData eventData)
        {
            ingredientDetailPanel.SetActive(false);
        }
    }
}