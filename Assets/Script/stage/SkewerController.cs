using System.Collections.Generic;
using Script.machine;
using UnityEngine;

namespace Script.stage
{
    public class SkewerController : MonoBehaviour
    {
        public Dictionary<FirstIngredient, GameObject> FirstIngredientPrefabs = new Dictionary<FirstIngredient, GameObject>();
        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;

        private Skewer _currentSkewer;
        private GameObject _currentSkewerGameObject;

        public void CreateNewSkewer()
        {
            _currentSkewer = new Skewer();
            _currentSkewerGameObject = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
        }

        public void DestroySkewer()
        {
            _currentSkewer = null;
            Destroy(_currentSkewerGameObject);
        }

        public void AddFirstIngredient(FirstIngredient type)
        {
            if (_currentSkewer.GetFirstIngredients().Count > 2) return;
            if (FirstIngredientPrefabs.TryGetValue(type, out GameObject prefab))
            {
                switch (type)
                {
                    case FirstIngredient.Banana:
                        _currentSkewer.AddFirstIngredient(type);

                        break;
                    case FirstIngredient.Strawberry:
                        break;
                    case FirstIngredient.GreenGrape:
                        break;

                }

            }
        }

    }
}