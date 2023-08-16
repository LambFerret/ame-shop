using System.Collections;
using DG.Tweening;
using Script.persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }

        private void Start()
        {
            DataPersistenceManager.Instance.LoadGame();
            StartCoroutine(TitleCoroutine());
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