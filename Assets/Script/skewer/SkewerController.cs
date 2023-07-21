using System.Collections.Generic;
using Script.machine;
using Script.player;
using UnityEngine;

namespace Script.skewer
{
    public class SkewerController : MonoBehaviour
    {
        public List<GameObject> FirstIngredientPrefabs;
        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;

        private bool _isSkewerCreated;
        private Skewer _currentSkewer;
        private GameObject _currentSkewerGameObject;

        private bool _stepChange;
        private Step _currentStep;

        public GameManager gameManager;

        public enum Step
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
            if (_currentStep != 0) return;
            _currentSkewer.AddFirstIngredient(type);
            if (_currentSkewer.GetFirstIngredients().Count > 2)
            {
                GoToStep(Step.Second);
            }

            // _currentSkewerGameObject

            // break;
            // case FirstIngredient.Strawberry:
            // break;
            // case FirstIngredient.GreenGrape:
            // break;
            // }
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

        private void GoToStep(Step step)
        {
            gameManager.stageController.GotToMachineStep((int)step);
        }
    }
}