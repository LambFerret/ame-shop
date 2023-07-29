using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.player;
using Script.setting;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.skewer
{
    public class SkewerController : MonoBehaviour
    {
        public GameManager gameManager;

        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;
        public GameObject pickUpDesk;

        private SkewerBehavior _currentSkewer;
        private GameObject _currentSkewerObject;

        public void CreateNewSkewer(int size)
        {
            if (_currentSkewerObject != null) return;
            var skewer = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
            skewer.GetComponent<SkewerBehavior>().SetSize(size);
            SetHand(skewer);
        }

        public bool AddFirstIngredientToSkewerInHand(IngredientManager.FirstIngredient type)
        {
            if (_currentSkewerObject == null)
            {
                MakeWarningMessage("No skewer in hand");
                return false;
            }
            var prefab = gameManager.ingredientManager.GetFirstIngredient(type);
            if (IsSkewerLengthMax(prefab.size))
            {
                MakeWarningMessage("Not enough space in skewer");
                return false;
            }

            _currentSkewer.AddFirstIngredient(prefab);

            return true;
        }

        private void MakeWarningMessage(string text)
        {
            var warningMessage = Instantiate(gameManager.warningMessagePrefab, skewerPlaceHolder.transform);
            warningMessage.GetComponent<TextMeshProUGUI>().text = text;
            warningMessage.transform.DOBlendableMoveBy(new Vector3(0, 200, 0), 1).OnComplete(() => Destroy(warningMessage));
        }
        public bool IsSkewerLengthMax(int size)
        {
            int currentSize = 0;
            foreach (var element in _currentSkewer.GetFirstIngredients())
            {
                currentSize += element.size;
            }

            return currentSize + size > _currentSkewer.currentSkewerMaxLength;
        }

        public bool ReceiveSkewerFromBoiler(GameObject skewerGameObject)
        {
            if (_currentSkewerObject != null)
            {
                Debug.Log("my hand is already full");
                return false;
            }

            SetHand(skewerGameObject);
            _currentSkewerObject.transform.SetParent(skewerPlaceHolder.transform);
            GoToStep(Step.Third);
            return true;
        }

        public void GiveSkewerToBoiler(out GameObject skewerGameObject)
        {
            if (_currentSkewerObject == null)
            {
                skewerGameObject = null;
                Debug.Log("No skewer to give.");
                return;
            }

            skewerGameObject = _currentSkewerObject;
            SetHand(null);
        }

        public void AddTemperature(int temperature, int concentration)
        {
            _currentSkewer.AddTemperature(temperature, concentration);
        }

        public void AddThirdIngredient(IngredientManager.ThirdIngredient type)
        {
            if (_currentSkewerObject == null) return;

            _currentSkewer.AddThirdIngredient(type);
        }

        public void Blend()
        {
            _currentSkewer.AddBlendIngredient();
            GoToStep(Step.First);
        }

        public void PackUp()
        {
            _currentSkewer.SwitchToPackUp();
            _currentSkewerObject.transform.SetParent(pickUpDesk.transform);
            gameManager.stageController.SwitchCashierMachine(true);
            SetHand(null);
            GoToStep(Step.Close);
        }

        public void Destroy()
        {
            Destroy(_currentSkewerObject);
            SetHand(null);
            GoToStep(Step.First);

        }

        private void SetHand(GameObject skewerGameObject)
        {
            // If the skewerGameObject is null, unset the current skewer
            if (skewerGameObject == null)
            {
                if (_currentSkewer != null) _currentSkewer.SetSkewerFocused(false);
                _currentSkewerObject = null;
                _currentSkewer = null;
            }
            else
            {
                // If a GameObject is provided, set it as the current skewer
                _currentSkewerObject = skewerGameObject;
                _currentSkewer = _currentSkewerObject.GetComponent<SkewerBehavior>();
                _currentSkewer.SetSkewerFocused(true);
            }
        }

        private void GoToStep(Step step)
        {
            gameManager.stageController.GotToMachineStep((int)step);
        }

        private enum Step
        {
            First,
            Second,
            Third,
            Close
        }
    }
}