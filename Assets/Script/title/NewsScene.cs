using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Script.title
{
    public class NewsScene : MonoBehaviour
    {
        public TextMeshProUGUI clickToContinue;
        public GameObject gameStartButton;
        public Animator gameStartIdle;

        private void Start()
        {
            gameStartIdle = gameStartButton.GetComponent<Animator>();
            StartCoroutine(SequenceCoroutine());
        }

        private IEnumerator SequenceCoroutine()
        {
            clickToContinue.gameObject.SetActive(true);
            StartCoroutine(BlinkingCoroutine());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            gameStartButton.SetActive(true);
            gameStartIdle.Play("blog");
            yield return new WaitForSeconds(1);

            clickToContinue.gameObject.SetActive(true);
            StartCoroutine(BlinkingCoroutine());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            LoadingScreen.Instance.LoadScene("CashierScene");
        }

        private IEnumerator BlinkingCoroutine()
        {
            while (true)
            {
                clickToContinue.DOFade(0.5f, 0.5f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(1);
            }
        }
    }
}