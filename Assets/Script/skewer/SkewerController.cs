using System.Collections.Generic;
using Script.player;
using UnityEngine;

namespace Script.skewer
{
    public class SkewerController : MonoBehaviour
    {
        public List<GameObject> firstIngredientPrefabs;
        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;
        public GameObject pickUpDesk;

        private bool _isSkewerCreated;
        private Skewer _currentSkewer;
        private GameObject _currentSkewerGameObject;

        public GameManager gameManager;

        private enum Step
        {
            First,
            Second,
            Third,
            Close
        }

        public void CreateNewSkewer()
        {
            if (_isSkewerCreated) return;
            _isSkewerCreated = true;
            _currentSkewer = new Skewer();
            _currentSkewerGameObject = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
            _currentSkewerGameObject.GetComponent<SkewerBehavior>().SetSkewerFocused(true);
        }

        public void DestroySkewer()
        {
            _isSkewerCreated = false;
            _currentSkewer = null;
            Destroy(_currentSkewerGameObject);
            GoToStep(Step.First);
        }

        public void AddFirstIngredient(FirstIngredient type)
        {
            if (_currentSkewer == null) return;
            if (_currentSkewer.GetFirstIngredients().Count > 3) return;
            _currentSkewer.AddFirstIngredient(type);
            foreach (var prefab in firstIngredientPrefabs)
            {
                var nameStr = prefab.GetComponent<IngredientBehavior>().GetName();
                if (nameStr.Equals(type.ToString()))
                {
                    _currentSkewerGameObject.GetComponent<SkewerBehavior>().AddFirstIngredient(prefab);
                }
            }
            if (_currentSkewer.GetFirstIngredients().Count > 2)
            {
                GoToStep(Step.Second);
            }
        }

        public void ReceiveSkewerFromBoiler(Skewer skewer, GameObject skewerGameObject)
        {
            if (_isSkewerCreated)
            {
                Debug.Log("A skewer is already in hand.");
                return;
            }

            _currentSkewer = skewer;
            _currentSkewerGameObject = skewerGameObject;
            _isSkewerCreated = true;
            _currentSkewerGameObject.GetComponent<SkewerBehavior>().SetSkewerFocused(true);
            _currentSkewerGameObject.transform.SetParent(skewerPlaceHolder.transform);

            if (_currentSkewer.GetSecondDryTime() > 0) GoToStep(Step.Third);
        }

        public void GiveSkewerToBoiler(out Skewer skewer, out GameObject skewerGameObject)
        {
            if (!_isSkewerCreated)
            {
                skewer = null;
                skewerGameObject = null;
                Debug.Log("No skewer to give.");
                return;
            }

            skewer = _currentSkewer;
            skewerGameObject = _currentSkewerGameObject;
            _isSkewerCreated = false;
        }

        public void AddThirdIngredient(ThirdIngredient type)
        {
            if (_currentSkewer == null) return;
            if (_currentSkewer.GetFirstIngredients().Count == 0) return;
            _currentSkewer.AddThirdIngredient(type);
            GoToStep(Step.Close);
        }

        public void Blend()
        {
            _currentSkewer.AddBlendIngredient();
            GoToStep(Step.First);
        }

        public void PackUp()
        {
            if (_currentSkewer.GetFirstIngredients().Count == 0) return;
            SkewerBehavior behavior = _currentSkewerGameObject.GetComponent<SkewerBehavior>();
            behavior.SetSkewerFocused(false);
            behavior.SwitchToPackUp();
            behavior.transform.SetParent(pickUpDesk.transform);

            gameManager.stageController.SwitchCashierMachine(true);
        }

        private void GoToStep(Step step)
        {
            gameManager.stageController.GotToMachineStep((int)step);
        }
    }
}