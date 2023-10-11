using System.Collections;
using DG.Tweening;
using player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace title
{
    public class TitleScene : MonoBehaviour
    {
        public Image title;
        public TextMeshProUGUI startText;
        public GameObject buttonGroup;

        private void Awake()
        {
            float canvasWidth = title.canvas.pixelRect.width;
            float titleWidth = title.rectTransform.rect.width;

            // Set the starting position
            float startX = -0.5f * canvasWidth - 0.5f * titleWidth;
            title.rectTransform.anchoredPosition = new Vector2(startX, title.rectTransform.anchoredPosition.y);

            // Do the movement (This has been moved from the coroutine to Awake for demonstration)
            float endX = -0.5f * canvasWidth + 0.5f * titleWidth;
            title.rectTransform.DOAnchorPosX(endX, 2).SetEase(Ease.OutCubic);
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