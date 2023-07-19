using System.Collections.Generic;
using Script.machine;
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

        public void CreateNewSkewer()
        {
            if (_isSkewerCreated) return;
            _isSkewerCreated = true;
            _currentSkewer = new Skewer();
            _currentSkewerGameObject = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
        }

        public void DestroySkewer()
        {
            _isSkewerCreated = false;
            _currentSkewer = null;
            Destroy(_currentSkewerGameObject);
        }

        public void AddFirstIngredient(FirstIngredient type)
        {
            if (_currentSkewer.GetFirstIngredients().Count > 2) return;
            // switch (type)
            // {
            // case FirstIngredient.Banana:
            _currentSkewer.AddFirstIngredient(type);

            // _currentSkewerGameObject

            // break;
            // case FirstIngredient.Strawberry:
            // break;
            // case FirstIngredient.GreenGrape:
            // break;
            // }
        }
    }
}