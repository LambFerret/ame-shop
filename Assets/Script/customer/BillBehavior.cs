using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.customer
{
    public class BillBehavior : MonoBehaviour
    {
        private Image _image;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = gameObject.transform.Find("text").GetComponent<TextMeshProUGUI>();
            _image = gameObject.transform.Find("icon").GetComponent<Image>();
        }

        public void MakeBill(Customer customer, string quote)
        {
            Transform t = transform;
            Vector3 origin = t.position;
            t.DOMove(origin + new Vector3(Screen.width, 0, 0), 0);
            t.DOMove(origin, 0.5f).SetEase(Ease.InCubic);
            _text.text = quote;
        }
    }
}