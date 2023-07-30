using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.customer
{
    public class CustomerBehavior : MonoBehaviour, IDataPersistence
    {
        public Customer customer;

        public StageController stageController;
        public BillBehavior billBehavior;
        public float animationDuration = 2f;
        public float idleAnimationScale = 1.03f;
        private GameObject _buttonGroup;

        private GameObject _conversation;
        private TextMeshProUGUI _conversationText;

        private GameObject _customer;

        private bool _isStart;
        private bool _isAccept;
        private TextMeshProUGUI _moneyText;

        private Image _texture;
        private Image _timerImage;

        private GameObject _bill;
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

            if (_timerImage.fillAmount <= 0) Serve(Customer.QuoteLine.TimeOut);
        }

        public void SetScript(Customer script)
        {
            _isStart = true;
            customer = script;
            SetIdleAnimation(idleAnimationScale, animationDuration);
            SetRandomFavorites();
            SetQuote(Customer.QuoteLine.Enter);
            _conversation.SetActive(false);
        }

        private void SetRandomFavorites()
        {
            customer.firstIngredients.Clear();
            var a = GameManager.Instance.ingredientManager.GetRandomFirstIngredient();
            customer.firstIngredients.Add(a);
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
            Serve(Customer.QuoteLine.Refused);
        }

        private void SaveIntoBill()
        {
            _bill = billBehavior.MakeBill(customer, _conversationText.text);
        }

        private void ClearBill(bool isServed = true)
        {
            if (_bill is null) return;
            if (isServed)
            {
                _bill.transform.DOFlip();
            }
            else
            {
                _bill.transform.DOShakeRotation(0.5f, 10, 10);
            }

            Destroy(_bill);
        }

        public void SetTexture(Sprite sprite)
        {
            _texture.sprite = sprite;
        }

        public void Serve(Customer.QuoteLine feeling)
        {
            if (feeling == Customer.QuoteLine.Enter) return;

            ClearBill();
            SetQuote(feeling);
            _buttonGroup.SetActive(false);
            _isStart = false;

            switch (feeling)
            {
                case Customer.QuoteLine.Refused:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity / 4);

                    break;
                case Customer.QuoteLine.TimeOut:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity);

                    break;
                case Customer.QuoteLine.Poked:
                    if (!customer.isSlime)
                    {
                        Debug.Log("쨔잔 슬라임이 아니었습니다 ㅃ~");
                        GameEventManager.Instance.PopularityChanged(customer.minPopularity * 2);
                    }
                    else
                    {
                        Debug.Log("쨔잔 슬라임");
                        GameEventManager.Instance.IngredientChanged(customer.slimeIngredient,
                            customer.slimeIngredientCount);
                    }

                    break;
                case Customer.QuoteLine.BadMildTaste:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity);

                    break;
                case Customer.QuoteLine.BadTooSweet:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity);

                    break;
                case Customer.QuoteLine.BadTooWatery:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity);

                    break;
                case Customer.QuoteLine.BadNotMyChoice:
                    GameEventManager.Instance.PopularityChanged(customer.minPopularity);

                    break;
                case Customer.QuoteLine.Good:
                    var money = customer.maxMoney - customer.minMoney;
                    var moneyCal = (int)(money * _timerImage.fillAmount) + customer.minMoney;
                    _moneyText.text = moneyCal.ToString();
                    GameEventManager.Instance.MoneyChanged(moneyCal);
                    GameEventManager.Instance.PopularityChanged((int)(customer.maxPopularity * _timerImage.fillAmount));
                    MoneyAnimation();
                    break;
                default:
                    return;
            }

            StartCoroutine(EndCustomer());
        }

        private IEnumerator EndCustomer()
        {
            yield return new WaitForSeconds(1f);
            _buttonGroup.SetActive(true);
            gameObject.SetActive(false);
        }


        public void LoadData(GameData data)
        {
            value = data.Ingredients[customer.slimeIngredient];
        }

        public void SaveData(GameData data)
        {
            data.Ingredients[customer.slimeIngredient] = value;
        }

        public bool IsAccepted()
        {
            return _isAccept;
        }

        public void CheckServedSkewer(SkewerBehavior skewer)
        {
            var a = FindDominantIngredient(skewer.GetFirstIngredients());
            if (a == null) Serve(Customer.QuoteLine.BadNotMyChoice);

            if (skewer.IsNotEnoughDry() || !skewer.CheckTemperature()) Serve(Customer.QuoteLine.BadTooWatery);
            if (skewer.CheckConcentration() < 0)
            {
                Serve(Customer.QuoteLine.BadMildTaste);
            }
            else if (skewer.CheckConcentration() > 0)
            {
                Serve(Customer.QuoteLine.BadTooSweet);
            }
            Serve(Customer.QuoteLine.Good);
        }

        public Ingredient FindDominantIngredient(IEnumerable<Ingredient> ingredients)
        {
            // Group the ingredients by their name and calculate the total size for each group
            var ingredientGroups = ingredients.GroupBy(i => i.ingredientName)
                .Select(g => new
                {
                    Ingredient = g.First(),
                    TotalSize = g.Sum(i => i.size)
                })
                .ToList();

            // Calculate the total size of all ingredients
            int totalSize = ingredientGroups.Sum(g => g.TotalSize);

            // Find a group where the total size is 80% or more of the total size
            var dominantGroup = ingredientGroups.FirstOrDefault(g => g.TotalSize >= totalSize * 0.8f);

            // If such a group exists, return the ingredient, otherwise return null
            return dominantGroup != null ? dominantGroup.Ingredient : null;
        }

        // private int CalculateScore(Skewer skewer)
        // {
        // var scoreA = CountCommonElements(skewer.GetFirstIngredients(), customer.firstIngredients);
        // var scoreB = CountCommonElements(skewer.GetSecondIngredients(), customer.secondIngredients);
        // var scoreC = CountCommonElements(skewer.GetThirdIngredients(), customer.thirdIngredients);
        // return scoreA + scoreB + scoreC;
        // }

        private static int CountCommonElements<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var counter1 = list1.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            var counter2 = list2.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            var commonCount = 0;
            foreach (var kvp in counter1)
            {
                int countInList2;
                if (counter2.TryGetValue(kvp.Key, out countInList2)) commonCount += Math.Min(kvp.Value, countInList2);
            }

            return commonCount;
        }

        private void SetQuote(Customer.QuoteLine quoteLine)
        {
            var lines = customer.QuoteLines[quoteLine];
            _conversationText.text = lines[Random.Range(0, lines.Count)]
                .Replace("{o}", customer.firstIngredients[0].ToString());
        }

        private void SetTimer(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _timerImage.fillAmount = value;
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
                gameObject.SetActive(false);
            });
        }
    }
}