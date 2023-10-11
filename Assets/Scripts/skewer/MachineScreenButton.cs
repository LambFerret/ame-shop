using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class MachineScreenButton : MonoBehaviour
    {
        public enum WhereAmI
        {
            First,
            Second,
            Third
        }

        public Scrollbar scrollbar;
        public WhereAmI currentPosition;

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
        }

        private void Update()
        {
            float value = scrollbar.value;
            currentPosition = value switch
            {
                < 0.25F and >= 0 => WhereAmI.First,
                > 0.25F and < 0.75F => WhereAmI.Second,
                >= 0.75F and <= 1 => WhereAmI.Third,
                _ => currentPosition
            };
        }


        public void GoRight()
        {
            switch (currentPosition)
            {
                case WhereAmI.First:
                    DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 0.5F, 1f);
                    break;
                case WhereAmI.Second:
                    DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 1, 1f);
                    break;
            }
        }

        public void GoLeft()
        {
            switch (currentPosition)
            {
                case WhereAmI.Second:
                    DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 0, 1f);
                    break;
                case WhereAmI.Third:
                    DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 0.5F, 1f);
                    break;
            }
        }
    }
}