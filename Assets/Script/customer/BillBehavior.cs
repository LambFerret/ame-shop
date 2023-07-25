using DG.Tweening;
using Script.player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.customer
{
    public class BillBehavior : MonoBehaviour
    {
        public GameObject billPrefab;

        public GameObject MakeBill(Customer customer, string quote)
        {
            var a = Instantiate(billPrefab, transform);
            // var image = a.transform.Find("icon").GetComponent<Image>();
            var line = a.transform.Find("text").GetComponent<TextMeshProUGUI>();
            Vector3 origin = a.transform.position;
            a.transform.DOMove(origin + new Vector3(Screen.width, 0, 0), 0);
            a.transform.DOMove(origin, 0.5f).SetEase(Ease.InCubic);


            line.text = quote;

            return a;
        }
    }
}