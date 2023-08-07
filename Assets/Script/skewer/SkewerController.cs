using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.player;
using Script.setting;
using Script.stage;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.skewer
{
    public class SkewerController : MonoBehaviour
    {
        public GameManager gameManager;
        public StageController stageController;

        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;
        public GameObject pickUpDesk;
        public GameObject weaponPlaceHolder;
        public GameObject weapon;

        [Header("Test Field")]
        [SerializeField] public WhatsOnHand whatsOnHand;
        [SerializeField] private SkewerBehavior _currentSkewer;
        [SerializeField] private GameObject _currentSkewerObject;
        [SerializeField] private bool _isWeaponOnHand;

        public enum WhatsOnHand
        {
            None,
            Skewer,
            Weapon
        }

        private void Awake()
        {
            weapon = weaponPlaceHolder.transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            if (_currentSkewer is not null)
            {
                whatsOnHand = WhatsOnHand.Skewer;
            }
            else if (_currentSkewer is null && _isWeaponOnHand)
            {
                whatsOnHand = WhatsOnHand.Weapon;
            }
            else
            {
                whatsOnHand = WhatsOnHand.None;
            }

            if (whatsOnHand == WhatsOnHand.Weapon)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(weapon.transform.parent as RectTransform,
                    Input.mousePosition, null, out Vector2 localPoint);
                weapon.transform.localPosition = localPoint;
            }
        }

        private void SetWeaponOnHand(bool toHand)
        {
            if (!toHand)
            {
                weapon.transform.DOLocalMove(Vector3.zero, 0.5f);
                _isWeaponOnHand = false;
                return;
            }

            switch (whatsOnHand)
            {
                case WhatsOnHand.None:
                    _isWeaponOnHand = true;
                    break;
                case WhatsOnHand.Skewer:
                    MakeWarningMessage("Cannot hold weapon with skewer");
                    return;
                case WhatsOnHand.Weapon:
                    return;
            }
        }

        public void EquipWeapon()
        {
            SetWeaponOnHand(!_isWeaponOnHand);
        }

        public void CreateNewSkewer(int size)
        {
            SetWeaponOnHand(false);
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
            warningMessage.transform.DOBlendableMoveBy(new Vector3(0, 200, 0), 1)
                .OnComplete(() => Destroy(warningMessage));
        }

        private bool IsSkewerLengthMax(int size)
        {
            int currentSize = 0;
            foreach (var element in _currentSkewer.GetFirstIngredients())
            {
                currentSize += element.size;
            }

            return currentSize + size > _currentSkewer.maxLength;
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
            stageController.SwitchCashierMachine(true);
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
            stageController.GotToMachineStep((int)step);
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