using System.Collections;
using System.Collections.Generic;
using customer;
using DG.Tweening;
using player;
using player.data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace scene
{
    public class NewsScene : MonoBehaviour, IDataPersistence
    {
        [Header("Setting")] public List<News> newsList;

        [Header("News text")] public TextMeshProUGUI newsTitle;

        public TextMeshProUGUI newsContent;
        public Image newsImage;

        [Header("Game Object")]
        public Button clickToContinue;

        public GameObject gameStartButton;
        public Animator gameStartIdle;

        [Header("Info")] public int day;

        private TextMeshProUGUI _clickToContinueText;
        private bool _isClickToContinue;

        private void Start()
        {
            _clickToContinueText = clickToContinue.GetComponentInChildren<TextMeshProUGUI>();
            gameStartIdle = gameStartButton.GetComponent<Animator>();
            SetNewsText();
            StartCoroutine(SequenceCoroutine());
        }

        public void LoadData(GameData data)
        {
            day = data.playerLevel;
        }

        public void SaveData(GameData data)
        {
        }

        private void SetNewsText()
        {
            if (newsList.Count == 0)
            {
                newsTitle.text = "No news";
                return;
            }

            List<int> newsIndex = new List<int>();
            foreach (var n in newsList)
                if (day < n.dayMax && day > n.dayMin)
                    newsIndex.Add(newsList.IndexOf(n));
            News news = newsList[newsIndex[Random.Range(0, newsIndex.Count)]];

            newsTitle.text = news.title;
            newsContent.text = news.content;
            newsImage.sprite = news.image;
        }

        private IEnumerator SequenceCoroutine()
        {
            clickToContinue.gameObject.SetActive(true);
            StartCoroutine(BlinkingCoroutine());

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            gameStartButton.SetActive(true);
            gameStartIdle.Play("blog");
            yield return new WaitForSeconds(1);

            StartCoroutine(BlinkingCoroutine());
        }

        private IEnumerator BlinkingCoroutine()
        {
            _isClickToContinue = true;
            clickToContinue.gameObject.SetActive(true);
            clickToContinue.onClick.AddListener(ClickToContinueClicked);

            while (true)
            {
                _clickToContinueText.DOFade(0.5f, 0.5f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(1);
            }
        }

        private void ClickToContinueClicked()
        {
            if (!_isClickToContinue) return;
            LoadingScreen.Instance.LoadScene("CashierScene");
        }
    }
}