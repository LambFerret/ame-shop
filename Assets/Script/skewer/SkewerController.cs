using System.Collections.Generic;
using Script.player;
using Script.setting;
using UnityEngine;

namespace Script.skewer
{
    public class SkewerController : MonoBehaviour
    {
        public GameManager gameManager;

        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;
        public GameObject pickUpDesk;
        public List<GameObject> allSkewerIHave = new();
        private SkewerBehavior _currentSkewerBehavior;

        private GameObject _currentSkewerOnHand;

        public void CreateNewSkewer()
        {
            if (_currentSkewerOnHand != null) return;
            var skewer = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
            allSkewerIHave.Add(skewer);
            SetHand(skewer);
        }

        public void AddFirstIngredientToSkewerInHand(IngredientManager.FirstIngredient type)
        {
            if (_currentSkewerOnHand == null) return;
            var prefab = gameManager.ingredientManager.GetFirstIngredientPrefab(type);
            _currentSkewerBehavior.AddFirstIngredient(prefab);
            if (_currentSkewerBehavior.GetSkewer().GetFirstIngredients().Count > 2)
            {
                GoToStep(Step.Second);
            }
        }

        public bool ReceiveSkewerFromBoiler(GameObject skewerGameObject)
        {
            if (_currentSkewerOnHand != null)
            {
                Debug.Log("my hand is already full");
                return false;
            }

            SetHand(skewerGameObject);
            _currentSkewerOnHand.transform.SetParent(skewerPlaceHolder.transform);
            GoToStep(Step.Third);
            return true;
        }

        public void GiveSkewerToBoiler(out GameObject skewerGameObject)
        {
            if (_currentSkewerOnHand == null)
            {
                skewerGameObject = null;
                Debug.Log("No skewer to give.");
                return;
            }

            skewerGameObject = _currentSkewerOnHand;
            SetHand(null);
        }

        public void AddThirdIngredient(IngredientManager.ThirdIngredient type)
        {
            if (_currentSkewerOnHand == null) return;

            _currentSkewerBehavior.AddThirdIngredient(type);
        }

        public void Blend()
        {
            _currentSkewerBehavior.Blend();
            GoToStep(Step.First);
        }

        public void PackUp()
        {
            _currentSkewerBehavior.SwitchToPackUp();
            _currentSkewerOnHand.transform.SetParent(pickUpDesk.transform);
            gameManager.stageController.SwitchCashierMachine(true);
            SetHand(null);
            GoToStep(Step.Close);
        }

        public void Destroy()
        {
            Destroy(_currentSkewerOnHand);
            SetHand(null);
            GoToStep(Step.First);

        }

        private void SetHand(GameObject skewerGameObject)
        {
            // If the skewerGameObject is null, unset the current skewer
            if (skewerGameObject == null)
            {
                if (_currentSkewerBehavior != null) _currentSkewerBehavior.SetSkewerFocused(false);
                _currentSkewerOnHand = null;
                _currentSkewerBehavior = null;
            }
            else
            {
                // If a GameObject is provided, set it as the current skewer
                _currentSkewerOnHand = skewerGameObject;
                _currentSkewerBehavior = _currentSkewerOnHand.GetComponent<SkewerBehavior>();
                _currentSkewerBehavior.SetSkewerFocused(true);
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