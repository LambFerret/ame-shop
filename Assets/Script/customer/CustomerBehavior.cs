using System;
using System.Collections.Generic;
using System.Linq;
using Script.player;
using Script.skewer;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Script.customer
{
    public class CustomerBehavior : MonoBehaviour
    {
        [SerializeField] private Customer customer;

        public float textureIdleDuration = 2f;
        public float textureIdleScale = 1.03f;

        private Player _player;

        private GameObject _conversation;
        private TextMeshProUGUI _conversationText;
        private GameObject _buttonGroup;

        private GameObject _customer;
        private Image _texture;
        private Image _timerImage;
        private TextMeshProUGUI _moneyText;

        private bool _isStart;

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
        }

        public void SetScript(Customer script)
        {
            _isStart = true;
            customer = script;
            _conversation.SetActive(false);
            Debug.Log("currently calling this : " + customer.customerName);

            var rectTransform = _texture.rectTransform;


            // Create a sequence for the first action
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Append(rectTransform.DOScale(new Vector3(textureIdleScale, 1 / textureIdleScale, 1),
                textureIdleDuration / 2));

            // Create a sequence for the second and third action
            Sequence sequence2 = DOTween.Sequence();
            sequence2.Append(rectTransform.DOScale(new Vector3(textureIdleScale, 1 / textureIdleScale, 1), 0));
            sequence2.Append(rectTransform.DOScale(new Vector3(1 / textureIdleScale, textureIdleScale, 1),
                textureIdleDuration));
            sequence2.Append(rectTransform.DOScale(new Vector3(textureIdleScale, 1 / textureIdleScale, 1),
                textureIdleDuration));
            sequence2.SetLoops(-1); // infinite loop

            // Start the second sequence after the first one completes
            sequence1.OnComplete(() => sequence2.Play());
        }

        public void SwitchConversation()
        {
            _conversation.SetActive(!_conversation.activeSelf);
        }

        public void SetTexture(Sprite sprite)
        {
            _texture.sprite = sprite;
        }


        public void FailToServe()
        {
            _player.AddPopularity(customer.minPopularity);
            EndCustomer();
        }

        public void SuccessToServe()
        {
            int money = customer.maxMoney - customer.minMoney;
            int moneyCal = (int)(money * _timerImage.fillAmount) + customer.minMoney;
            _moneyText.text = moneyCal.ToString();
            _player.AddMoney(moneyCal);
            _player.AddPopularity((int)((customer.maxPopularity - customer.minPopularity) * _timerImage.fillAmount));
            EndCustomer();
        }

        public void CheckServedSkewer(Skewer skewer)
        {
            int skewerScore = CalculateScore(skewer);
            Debug.Log("score is : " + skewerScore);
        }

        private int CalculateScore(Skewer skewer)
        {
            int scoreA = CountCommonElements(skewer.GetFirstIngredients(), customer.firstIngredients);
            int scoreB = CountCommonElements(skewer.GetSecondIngredients(), customer.secondIngredients);
            int scoreC = CountCommonElements(skewer.GetThirdIngredients(), customer.thirdIngredients);
            return scoreA + scoreB + scoreC;
        }

        public int CountCommonElements<T>(List<T> list1, List<T> list2)
        {
            var counter1 = list1.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            var counter2 = list2.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            int commonCount = 0;
            foreach (var kvp in counter1)
            {
                int countInList2;
                if (counter2.TryGetValue(kvp.Key, out countInList2))
                {
                    commonCount += Math.Min(kvp.Value, countInList2);
                }
            }

            return commonCount;
        }


        private void SetTimer(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _timerImage.fillAmount = value;
        }

        private void EndCustomer()
        {
            _isStart = false;

            const float moveDistance = 100;
            const float duration = 3f;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(transform.position.y + moveDistance, duration));
            sequence.Join(_moneyText.DOColor(new Color(_moneyText.color.r, _moneyText.color.g, _moneyText.color.b, 0),
                duration));
            sequence.OnComplete(() =>
            {
                _moneyText.text = "";
                gameObject.SetActive(false);
            });
        }

        private void Update()
        {
            if (_isStart && customer.patience > 0)
            {
                float decrementValue = Time.deltaTime / customer.patience;
                SetTimer(_timerImage.fillAmount - decrementValue);
            }

            if (_timerImage.fillAmount <= 0)
            {
                FailToServe();
            }
        }
    }
}