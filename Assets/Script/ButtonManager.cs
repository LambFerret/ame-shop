using UnityEngine;
using DG.Tweening;

namespace Script
{
    public class ButtonManager : MonoBehaviour
    {
        public GameObject machineScene;

        public void OnClickToCallMachineScene()
        {
            machineScene.transform.DOMove(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutBounce);
        }
    }
}