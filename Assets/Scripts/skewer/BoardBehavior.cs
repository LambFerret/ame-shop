using System;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class BoardBehavior : MonoBehaviour
    {
        public SkewerController skewer;

        private SkewerBehavior _currentSkewer;
        private GameObject _currentSkewerGameObject;

        private void Start()
        {
        }

        public void OnClickBoard()
        {
            if (_currentSkewer is null)
            {
                ReceiveSkewerFromController();
            }
            else
            {
                // GiveSkewerToController();
            }
        }

        private void ReceiveSkewerFromController()
        {
            skewer.GiveSkewerToBoiler(out GameObject skewerGameObject);
            if (skewerGameObject == null) return;

            _currentSkewerGameObject = skewerGameObject;
            _currentSkewer = skewerGameObject.GetComponent<SkewerBehavior>();
            _currentSkewerGameObject.transform.SetParent(transform);
            _currentSkewerGameObject.transform.localScale = new Vector3(3, 3, 3);
        }

        public void GiveSkewerToController()
        {
            if (_currentSkewer == null || _currentSkewerGameObject == null)
            {
                Debug.Log("No skewer to give.");
                return;
            }

            if (!skewer.ReceiveSkewerFromBoiler(_currentSkewerGameObject)) return;
            _currentSkewerGameObject.transform.localScale = new Vector3(1, 1, 1);
            _currentSkewer = null;
            _currentSkewerGameObject = null;
        }

        private void Update()
        {
        }
    }
}