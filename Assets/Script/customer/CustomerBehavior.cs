using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Script.player;
using Script.skewer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.customer
{
    public class CustomerBehavior : MonoBehaviour
    {
        [SerializeField] private Customer customer;

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

        private Player _player;
        private Image _texture;
        private Image _timerImage;

        private GameObject _bill;

        private void Awake()
        {
            _player = Player.Instance;

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

            if (_timerImage.fillAmount <= 0) FailToServe();
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
            customer.secondIngredients.Clear();
            customer.thirdIngredients.Clear();

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
        }

        public void Refuse()
        {
            _isAccept = true;
            FailToServe(true);
        }

        private void SaveIntoBill()
        {
            _bill = billBehavior.MakeBill(customer, _conversationText.text);
        }

        private void ClearBill(bool isServed)
        {
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

        private void FailToServe(bool areYouSorry = false)
        {
            ClearBill(false);
            SetQuote(Customer.QuoteLine.Bad);
            _player.AddPopularity(areYouSorry ? customer.minPopularity / 2 : customer.minPopularity);
            EndCustomer();
        }

        private void SuccessToServe()
        {
            ClearBill(true);
            var money = customer.maxMoney - customer.minMoney;
            var moneyCal = (int)(money * _timerImage.fillAmount) + customer.minMoney;
            _moneyText.text = moneyCal.ToString();
            _player.AddMoney(moneyCal);
            _player.AddPopularity((int)(customer.maxPopularity * _timerImage.fillAmount));
            EndCustomer();
        }

        public bool IsAccepted()
        {
            return _isAccept;
        }

        public void CheckServedSkewer(Skewer skewer)
        {
            var skewerScore = CalculateScore(skewer);
            Debug.Log("score is : " + skewerScore);

            if (skewerScore >= customer.scoreLimitation)
            {
                Debug.Log("score good");
                SetQuote(Customer.QuoteLine.Good);
            }
            else
            {
                Debug.Log("score bad");
                SetQuote(Customer.QuoteLine.Bad);
            }

            SuccessToServe();
        }

        private int CalculateScore(Skewer skewer)
        {
            var scoreA = CountCommonElements(skewer.GetFirstIngredients(), customer.firstIngredients);
            var scoreB = CountCommonElements(skewer.GetSecondIngredients(), customer.secondIngredients);
            var scoreC = CountCommonElements(skewer.GetThirdIngredients(), customer.thirdIngredients);
            return scoreA + scoreB + scoreC;
        }

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

        private void EndCustomer()
        {
            _isStart = false;
            MoneyAnimation();
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