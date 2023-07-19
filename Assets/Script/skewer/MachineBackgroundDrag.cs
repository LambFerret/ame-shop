using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Script.skewer
{
    public class MachineBackgroundDrag : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private Vector3 initialPosition;
        private Tweener moveTweener;
        public float moveTime = 0.5f; // the time it takes to move the inventory
        public Ease easeType = Ease.OutQuart; // the type of easing to use

        private void Start()
        {
            initialPosition = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (moveTweener != null && moveTweener.IsActive() && !moveTweener.IsComplete())
                moveTweener.Kill(); // stop any movement that's currently happening

            // start a new movement to follow the mouse
            moveTweener = transform.DOMove(Camera.main.ScreenToWorldPoint(Input.mousePosition), moveTime)
                .SetEase(easeType);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (moveTweener != null && moveTweener.IsActive() && !moveTweener.IsComplete())
                moveTweener.Kill(); // stop any movement that's currently happening

            // when the mouse is released, move the inventory back to its initial position
            moveTweener = transform.DOMove(initialPosition, moveTime).SetEase(easeType);
        }
    }

}