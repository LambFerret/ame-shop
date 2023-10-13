using DG.Tweening;
using ingredient;
using manager;
using setting;
using stage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class SkewerController : MonoBehaviour
    {
        public enum WhatsOnHand
        {
            None,
            Skewer,
            Topping,
            Weapon
        }

        public StageController stageController;

        public GameObject skewerPrefab;
        public GameObject skewerPlaceHolder;
        public GameObject pickUpDesk;
        public GameObject weaponPlaceHolder;
        public GameObject weapon;

        [Header("Test Field")] [SerializeField]
        public WhatsOnHand whatsOnHand;

        public SkewerBehavior currentSkewer;
        public GameObject currentSkewerObject;


        private void Awake()
        {
            weapon = weaponPlaceHolder.transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            if (GameManager.Instance.gameState != GameManager.GameState.Playing) return;
            switch (whatsOnHand)
            {
                case WhatsOnHand.None:
                    break;
                case WhatsOnHand.Weapon:
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(weapon.transform.parent as RectTransform,
                        Input.mousePosition, null, out Vector2 localPoint);
                    weapon.transform.localPosition = localPoint;
                    break;
                case WhatsOnHand.Skewer:
                    currentSkewer.MouseFollow();
                    break;
            }
        }

        private void SetWeaponOnHand(bool toHand)
        {
            if (!toHand)
            {
                weapon.transform.DOLocalMove(Vector3.zero, 0.5f);
                whatsOnHand = WhatsOnHand.None;
                return;
            }

            switch (whatsOnHand)
            {
                case WhatsOnHand.None:
                    whatsOnHand = WhatsOnHand.Weapon;
                    break;
                case WhatsOnHand.Skewer:
                    GameManager.Instance.MakeWarningMessage(GameManager.WarningState.FullHand);
                    return;
                case WhatsOnHand.Weapon:
                    return;
            }
        }

        public void EquipWeapon()
        {
            SetWeaponOnHand(whatsOnHand != WhatsOnHand.Weapon);
        }

        public void CreateNewSkewer(int size)
        {
            if (currentSkewerObject != null) return;
            // SetWeaponOnHand(false);
            var skewer = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
            skewer.GetComponent<SkewerBehavior>().SetSize(size);
            SetHand(skewer);
        }

        public bool AddIngredientToSkewerInHand(Ingredient type)
        {
            if (currentSkewerObject == null)
            {
                GameManager.Instance.MakeWarningMessage(GameManager.WarningState.NoSkewerInHand);
                return false;
            }

            if (IsSkewerLengthMax(type.size))
            {
                GameManager.Instance.MakeWarningMessage(GameManager.WarningState.NoSpaceInSkewer);
                return false;
            }

            currentSkewer.AddIngredient(type);

            return true;
        }


        private bool IsSkewerLengthMax(int size)
        {
            int currentSize = 0;
            foreach (var element in currentSkewer.GetIngredients()) currentSize += element.size;

            return currentSize + size > currentSkewer.maxLength - currentSkewer.skewerOffsetLength;
        }

        public bool ReceiveSkewerFromBoiler(GameObject skewerGameObject)
        {
            if (currentSkewerObject != null)
            {
                Debug.Log("my hand is already full");
                return false;
            }

            SetHand(skewerGameObject);
            currentSkewerObject.transform.SetParent(skewerPlaceHolder.transform);
            GoToStep(Step.Third);
            return true;
        }

        public void GiveSkewerToBoiler(out GameObject skewerGameObject)
        {
            if (currentSkewerObject == null)
            {
                skewerGameObject = null;
                Debug.Log("No skewer to give.");
                return;
            }

            skewerGameObject = currentSkewerObject;
            SetHand(null);
        }

        public int GetCurrentSize()
        {
            return currentSkewer.GetSize();
        }

        public void AddTemperature(int temperature, int concentration)
        {
            currentSkewer.AddTemperature(temperature, concentration);
        }

        public void Blend()
        {
            // _currentSkewer.AddBlendIngredient();
            GoToStep(Step.First);
        }

        public void PackUp()
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFX.PackageRing);
            currentSkewer.SwitchToPackUp();
            currentSkewerObject.transform.SetParent(pickUpDesk.transform);
            currentSkewerObject.transform.position = new Vector3(0, 0, 0);
            stageController.SwitchCashierMachine(true);
            SetHand(null);
            GoToStep(Step.Close);
        }

        public void Destroy()
        {
            if (whatsOnHand != WhatsOnHand.Skewer) return;
            Destroy(currentSkewerObject);
            SetHand(null);
            GoToStep(Step.First);
        }

        private void SetHand(GameObject skewerGameObject)
        {
            if (skewerGameObject is null)
            {
                if (currentSkewer is not null) currentSkewer.SetSkewerFocused(false);
                currentSkewerObject = null;
                currentSkewer = null;
                whatsOnHand = WhatsOnHand.None;
            }
            else
            {
                currentSkewerObject = skewerGameObject;
                currentSkewer = currentSkewerObject.GetComponent<SkewerBehavior>();
                currentSkewer.SetSkewerFocused(true);
                whatsOnHand = WhatsOnHand.Skewer;
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