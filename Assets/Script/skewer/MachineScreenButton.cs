using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Script.skewer
{
    public class MachineScreenButton : MonoBehaviour
    {
        public enum WhereAmI
        {
            First, Second, Third
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
            if (value is < 0.25F and >= 0)
            {
                currentPosition = WhereAmI.First;
            } else if (value is > 0.25F and < 0.75F)
            {
                currentPosition = WhereAmI.Second;
            } else if (value is >= 0.75F and <= 1)
            {
                currentPosition = WhereAmI.Third;
            }
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