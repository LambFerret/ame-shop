using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.title
{
    public class NewsScene : MonoBehaviour
    {
        public Image blackScreen;
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
            yield return blackScreen.DOFade(0, 1).WaitForCompletion();

            clickToContinue.gameObject.SetActive(true);
            StartCoroutine(BlinkingCoroutine());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            gameStartButton.SetActive(true);
            gameStartIdle.Play("blog");
            yield return new WaitForSeconds(1);

            clickToContinue.gameObject.SetActive(true);
            StartCoroutine(BlinkingCoroutine());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return blackScreen.DOFade(1, 1).WaitForCompletion();
            SceneManager.LoadScene("CashierScene");
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