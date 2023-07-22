using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.ui
{
    public class UIManager : MonoBehaviour
    {
        public List<GameObject> cashierUI;
        public List<GameObject> machineUI;

        public void SetActiveUI(bool isCashierScene)
        {
            foreach (var ui in cashierUI)
            {
                ui.SetActive(isCashierScene);
            }

            foreach (var ui in machineUI)
            {
                ui.SetActive(!isCashierScene);
            }
        }
    }
}