using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Script.persistence;

namespace Script.title
{
    public class TitleScene : MonoBehaviour
    {
        public Image title;
        public TextMeshProUGUI startText;
        public GameObject buttonGroup;

        private void Awake()
        {
            title.rectTransform.DOAnchorPosX(-title.rectTransform.rect.width, 0);
            buttonGroup.transform.DOLocalMoveY(-buttonGroup.transform.localPosition.y, 0);
        }

        private void Start()
        {
            DataPersistenceManager.Instance.LoadGame();
            StartCoroutine(TitleCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LoadingScreen.Instance.LoadScene("CashierScene");
            } else if (Input.GetKeyDown(KeyCode.W))
            {
                LoadingScreen.Instance.LoadScene("ResultScene");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                LoadingScreen.Instance.LoadScene("NewsScene");
            }
        }

        public void GameStart()
        {
            LoadingScreen.Instance.LoadScene("NewsScene");
        }


        private IEnumerator TitleCoroutine()
        {
            title.gameObject.SetActive(true);
            title.rectTransform.DOAnchorPosX(0, 2).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(1);
            buttonGroup.gameObject.SetActive(true);
            buttonGroup.transform.DOLocalMoveY(570, 2).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(2);

            startText.gameObject.SetActive(true);
            while (true)
            {
                yield return startText.DOFade(0.5f, 1).WaitForCompletion();
                yield return startText.DOFade(1f, 1).WaitForCompletion();
            }
        }
    }
}