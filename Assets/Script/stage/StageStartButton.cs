using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Script.stage
{
    public class StageStartButton : MonoBehaviour
    {
        private Tween _glowTween;

        private void Awake()
        {
            _glowTween = GetComponent<Button>().transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.5f)
                .SetLoops(-1, LoopType.Restart).SetAutoKill(false);
        }

        public void SwitchAnimation(bool glowOn)
        {
            if (glowOn)
            {
                _glowTween.Restart();
            }
            else
            {
                _glowTween.Kill();
            }
        }
    }
}