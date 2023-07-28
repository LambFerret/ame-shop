using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Script.persistence;
using UnityEngine.SceneManagement;

namespace Script.title
{
    public class TitleScene : MonoBehaviour
    {
        public Image logo;
        public Image title;
        public Image blackScreen;
        public GameObject loading;
        public TextMeshProUGUI startText;
        public GameObject buttonGroup;


        private void Start()
        {
            DataPersistenceManager.Instance.LoadGame();
            StartCoroutine(TitleCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("CashierScene");
            } else if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene("ResultScene");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("NewsScene");
            }
        }

        public void GameStart()
        {
            SceneManager.LoadSceneAsync("CashierScene");
        }


        private IEnumerator TitleCoroutine()
        {
            blackScreen.gameObject.SetActive(true);
            loading.gameObject.SetActive(false);
            logo.gameObject.SetActive(true);
            yield return logo.DOFade(1, 2).WaitForCompletion();
            yield return new WaitForSeconds(2);
            yield return logo.DOFade(0, 2).WaitForCompletion();

            loading.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            loading.gameObject.SetActive(false);


            logo.gameObject.SetActive(false);
            yield return blackScreen.DOFade(0, 2).WaitForCompletion();
            blackScreen.gameObject.SetActive(false);

            title.gameObject.SetActive(true);
            title.rectTransform.DOAnchorPosX(-1280, 2).SetEase(Ease.OutCubic);
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