using DG.Tweening;
using ingredient;
using manager;
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

        public GameManager gameManager;

        private void Awake()
        {
            weapon = weaponPlaceHolder.transform.GetChild(0).gameObject;
        }

        private void Update()
        {
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
                    MakeWarningMessage("Cannot hold weapon with skewer");
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
            SetWeaponOnHand(false);
            if (currentSkewerObject != null) return;
            var skewer = Instantiate(skewerPrefab, skewerPlaceHolder.transform);
            skewer.GetComponent<SkewerBehavior>().SetSize(size);
            SetHand(skewer);
        }

        public bool AddIngredientToSkewerInHand(Ingredient type)
        {
            if (currentSkewerObject == null)
            {
                MakeWarningMessage("No skewer in hand");
                return false;
            }

            if (IsSkewerLengthMax(type.size))
            {
                MakeWarningMessage("Not enough space in skewer");
                return false;
            }

            currentSkewer.AddIngredient(type);

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
            foreach (var element in currentSkewer.GetIngredients()) currentSize += element.size;

            return currentSize + size > currentSkewer.maxLength;
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
            currentSkewer.SwitchToPackUp();
            currentSkewerObject.transform.SetParent(pickUpDesk.transform);
            stageController.SwitchCashierMachine(true);
            SetHand(null);
            GoToStep(Step.Close);
        }

        public void Destroy()
        {
            Destroy(currentSkewerObject);
            SetHand(null);
            GoToStep(Step.First);
        }

        private void SetHand(GameObject skewerGameObject)
        {
            if (skewerGameObject == null)
            {
                if (currentSkewer != null) currentSkewer.SetSkewerFocused(false);
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