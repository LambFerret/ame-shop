using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Script.events;
using Script.ingredient;
using Script.persistence;
using Script.persistence.data;
using Script.player;
using Script.setting;
using Script.skewer;
using Script.stage;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.customer
{
    public class CustomerBehavior : MonoBehaviour, IDataPersistence
    {
        public Customer customer;
        public Dictionary<Customer.QuoteLine, List<string>> QuoteLines = new();

        public StageController stageController;
        public GameObject billPrefab;
        public float animationDuration = 2f;
        public float idleAnimationScale = 1.03f;
        private GameObject _buttonGroup;

        private GameObject _conversation;
        private TextMeshProUGUI _conversationText;

        private GameObject _customer;
        private GameObject _bill;
        public GameObject billPlaceHolder;

        private bool _isStart;
        private bool _isAccept;
        private TextMeshProUGUI _moneyText;

        private Image _texture;
        private Image _timerImage;

        private GameData _data;

        public int value;

        private void Awake()
        {
            _conversation = transform.Find("Conversation").gameObject;
            _conversationText = _conversation.transform.Find("text").GetComponent<TextMeshProUGUI>();
            _buttonGroup = _conversation.transform.Find("Buttons").gameObject;

            _customer = transform.Find("Customer").gameObject;
            _texture = _customer.transform.Find("Texture").GetComponent<Image>();
            _timerImage = _customer.transform.Find("Timer").GetComponent<Image>();
            _moneyText = _customer.transform.Find("Money").GetComponent<TextMeshProUGUI>();

            _moneyText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isStart && customer.patience > 0)
            {
                var decrementValue = Time.deltaTime / customer.patience;
                SetTimer(_timerImage.fillAmount - decrementValue);
            }

            if (_timerImage.fillAmount <= 0) Serve(Customer.QuoteLine.TimeOut, 0);
        }

        public void SetScript(Customer script)
        {
            _isStart = true;
            customer = script;
            SetIdleAnimation(idleAnimationScale, animationDuration);
            SetRandomFavorites();
            SetTimer(1);
            SetQuote(Customer.QuoteLine.Enter);
            _conversation.SetActive(false);
        }

        private void SetRandomFavorites()
        {
            customer.ingredient = IngredientManager.Instance.GetRandomIngredient();
        }

        private void SetIdleAnimation(float scale, float duration)
        {
            var rectTransform = _texture.rectTransform;

            var sequence1 = DOTween.Sequence();
            sequence1.Append(rectTransform.DOScale(new Vector3(scale, 1 / scale, 1), duration / 2));
            var sequence2 = DOTween.Sequence();
            sequence2.Append(rectTransform.DOScale(new Vector3(scale, 1 / scale, 1), 0));
            sequence2.Append(rectTransform.DOScale(new Vector3(1 / scale, scale, 1), duration));
            sequence2.Append(rectTransform.DOScale(new Vector3(scale, 1 / scale, 1), duration));
            sequence2.SetLoops(-1);
            sequence1.OnComplete(() => sequence2.Play());
        }

        public void SwitchConversation()
        {
            SwitchConversation(!_conversation.activeSelf);
        }

        private void SwitchConversation(bool isActive)
        {
            if (_isAccept) return;
            if (isActive)
            {
                _conversation.SetActive(true);
                var canvas = gameObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 10;
                gameObject.AddComponent<GraphicRaycaster>();
            }
            else
            {
                _conversation.SetActive(false);
                if (gameObject.TryGetComponent(out GraphicRaycaster raycaster)) Destroy(raycaster);
                if (gameObject.TryGetComponent(out Canvas canvasComponent)) Destroy(canvasComponent);
            }
        }

        public void Accept()
        {
            SwitchConversation(false);
            _isAccept = true;
            SaveIntoBill();
            _conversation.SetActive(false);
            stageController.SwitchCashierMachine(false);
        }

        public void Refuse()
        {
            _isAccept = true;
            Serve(Customer.QuoteLine.Refused, 50);
        }

        private void SaveIntoBill()
        {
            _bill = Instantiate(billPrefab, billPlaceHolder.transform);
            _bill.GetComponent<BillBehavior>().MakeBill(customer, _conversationText.text);
        }

        private void ClearBill()
        {
            if (_bill is null) return;
            Destroy(_bill);
        }

        public void CheckServedSkewer(SkewerBehavior skewer)
        {
            int satisfaction = 100;
            Customer.QuoteLine currentFeeling = Customer.QuoteLine.Good;

            // 기다린 시간 계산
            satisfaction -= 30 - (int)((1 - _timerImage.fillAmount) * customer.patience);

            // 농도가 안맞을 경우
            int concentration = skewer.CheckConcentration();
            if (concentration < 0)
            {
                satisfaction -= concentration * 2;
                currentFeeling = Customer.QuoteLine.BadMildTaste;
            }
            else if (concentration > 0)
            {
                satisfaction -= concentration;
                currentFeeling = Customer.QuoteLine.BadTooSweet;
            }

            // 덜굳음
            if (skewer.IsNotEnoughDry() || !skewer.CheckTemperature())
            {
                satisfaction -= 30;
                currentFeeling = Customer.QuoteLine.BadTooWatery;
            }

            // 재료가 안맞을 경우
            if (!skewer.IsDominantIngredient(customer.ingredient))
            {
                satisfaction -= 50;
                currentFeeling = Customer.QuoteLine.BadNotMyChoice;
            }

            if (satisfaction < 0) satisfaction = 0;

            int money;

            if (currentFeeling == Customer.QuoteLine.BadNotMyChoice)
            {
                money = 500;
            }
            else if (currentFeeling == Customer.QuoteLine.Good && satisfaction >= 90)
            {
                money = (int)(skewer.GetSkewerPrice() * 1.5F);
            }
            else
            {
                money = (int)(skewer.GetSkewerPrice() * 1.1F);
            }

            Serve(currentFeeling, satisfaction, money);
        }

        public void Serve(Customer.QuoteLine feeling, int satisfaction, int money = 0)
        {
            if (feeling == Customer.QuoteLine.Enter) return;

            ClearBill();
            SetQuote(feeling);
            _buttonGroup.SetActive(false);
            _isStart = false;

            float popularity = 0;
            bool endImmediately = false;

            switch (feeling)
            {
                case Customer.QuoteLine.Refused:
                    popularity = -3;
                    endImmediately = true;
                    break;
                case Customer.QuoteLine.TimeOut:
                    popularity = -6;
                    endImmediately = true;
                    break;
                case Customer.QuoteLine.Poked:
                    if (!customer.isSlime)
                    {
                        popularity = -6;
                    }
                    else
                    {
                        GameEventManager.Instance.IngredientChanged(IngredientManager.Instance.GetRandomIngredient(),
                            customer.slimeIngredientCount);
                    }

                    endImmediately = true;
                    break;
                case Customer.QuoteLine.BadMildTaste:
                    popularity = 1 + (float)satisfaction / 100;
                    break;
                case Customer.QuoteLine.BadTooSweet:
                    popularity = 1 + (float)satisfaction / 100;

                    break;
                case Customer.QuoteLine.BadTooWatery:
                    popularity = 1 + (float)satisfaction / 100;

                    break;
                case Customer.QuoteLine.BadNotMyChoice:
                    popularity = 1 + (float)satisfaction / 100;

                    break;
                case Customer.QuoteLine.Good:
                    popularity = 1 + (float)satisfaction / 100;

                    break;
                default:
                    return;
            }

            GameEventManager.Instance.PopularityChanged(popularity);
            if (!endImmediately)
            {
                _moneyText.text = money.ToString();
                GameEventManager.Instance.MoneyChanged(money);
                MoneyAnimation();
            }

            StartCoroutine(EndCustomer());
        }

        private IEnumerator EndCustomer()
        {
            if (!gameObject.TryGetComponent(out Canvas canvasComponentBefore))
            {
                var canvas = gameObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 10;
            }

            _conversation.SetActive(true);
            var blurScreen = _conversation.transform.GetChild(0).gameObject;
            blurScreen.SetActive(false);
            yield return new WaitForSeconds(1f);
            _buttonGroup.SetActive(true);
            blurScreen.SetActive(true);
            if (gameObject.TryGetComponent(out GraphicRaycaster raycaster)) Destroy(raycaster);
            if (gameObject.TryGetComponent(out Canvas canvasComponent)) Destroy(canvasComponent);

            _isAccept = false;
            gameObject.SetActive(false);
        }


        public void LoadData(GameData data)
        {
            // value = data.ingredients[IngredientManager.GetIngredientIndex(_ingredient)customer.slimeIngredient];
        }

        public void SaveData(GameData data)
        {
            // data.ingredients[] = value;
        }

        public bool IsAccepted()
        {
            return _isAccept;
        }

        private async Task SetQuote(Customer.QuoteLine quoteLine)
        {
            var locale = LocalizationSettings.SelectedLocale;
            var stringTableOp = LocalizationSettings.StringDatabase.GetTableAsync("Customer", locale);

            // Await the operation before accessing the stringTable
            var stringTable = await stringTableOp.Task;

            if (stringTableOp.Status != AsyncOperationStatus.Succeeded)
            {
                // Handle error here, e.g. log an error message
                Debug.LogError("Failed to load string table.");
                return;
            }

            foreach (var line in Enum.GetValues(typeof(Customer.QuoteLine)))
            {
                var lineStr = stringTable[customer.id + " " + line].GetLocalizedString();
                QuoteLines[(Customer.QuoteLine)line] = new List<string>(lineStr.Split('\n'));
            }

            var lines = QuoteLines[quoteLine];
            _conversationText.text = lines[Random.Range(0, lines.Count)]
                .Replace("{o}", customer.ingredient.ingredientId);
        }


        private void SetTimer(float time)
        {
            time = Mathf.Clamp(time, 0f, 1f);
            _timerImage.fillAmount = time;
        }

        private void MoneyAnimation()
        {
            _moneyText.gameObject.SetActive(true);
            const float moveDistance = 100;
            const float duration = 3f;
            var sequence = DOTween.Sequence();
            sequence.Append(_moneyText.transform.DOMoveY(_moneyText.transform.position.y + moveDistance, duration));
            sequence.Join(_moneyText.DOColor(new Color(_moneyText.color.r, _moneyText.color.g, _moneyText.color.b, 0),
                duration));
            sequence.OnComplete(() =>
            {
                _moneyText.text = "";
                _moneyText.gameObject.SetActive(false);
                // gameObject.SetActive(false);
            });
        }
    }
}