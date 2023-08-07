namespace Script.events
{
    using DG.Tweening;
    using UnityEngine;

    public class Mover : MonoBehaviour
    {
        public float duration = 2f;
        private Vector3 originalPosition;
        private Tween tween;


        public void Activate(Vector3 targetPosition)
        {
            gameObject.SetActive(true);
            originalPosition = transform.position;
            tween = transform.DOMove(targetPosition, duration).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            tween.Kill();
            transform.position = originalPosition;
        }
    }

}