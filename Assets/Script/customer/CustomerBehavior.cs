using System;
using Script.player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.customer
{
    public class CustomerBehavior : MonoBehaviour
    {
        public TextMeshProUGUI conversationText;
        public GameObject buttonGroup;
        public Customer customer;

        private Player _player;
        private Image _texture;
        private Image _timerImage;
        private Text _moneyText;
        private GameObject _textLine;
        private bool _isStart;

        private void Start()
        {
            _player = Player.Instance;
            // var a = transform.Find("Customer");
            // _texture = transform.Find("Image").GetComponent<Image>();
            _timerImage = transform.Find("Satisfaction").GetComponent<Image>();
            _moneyText = transform.Find("Money").GetComponent<Text>();
        }

        public void SetScript(Customer script)
        {
            _isStart = true;
            customer = script;
            // conversationText.text q = customer.lines[0];
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

        private void SetTimer(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _timerImage.fillAmount = value;
        }

        private void EndCustomer()
        {
            _isStart = false;
            Destroy(gameObject);
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