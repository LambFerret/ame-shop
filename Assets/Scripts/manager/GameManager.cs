using System;
using System.Collections.Generic;
using DG.Tweening;
using title;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameObject gamePausedPanel;
        public GameObject warningMessagePrefab;
        public GameObject warningMessagePanel;

        public bool isDebug;
        public GameState gameState;

        public enum GameState
        {
            Playing,
            Paused,
            GameOver
        }

        public enum WarningState
        {
             FullHand, NoSkewerInHand, NoSpaceInSkewer
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            if (isDebug)
            {
                Debug.Log(
                    "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                Debug.Log("Debug mode is on.");
                Debug.Log(
                    "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        private void Start()
        {
            gameState = GameState.Playing;

        }

        private void MakeWarningMessage(string text)
        {
            var warningMessage = Instantiate(warningMessagePrefab, warningMessagePanel.transform);
            warningMessage.GetComponent<TextMeshProUGUI>().text = text;
            warningMessage.transform.DOBlendableMoveBy(new Vector3(0, 200, 0), 1)
                .OnComplete(() => Destroy(warningMessage));
        }

        public void MakeWarningMessage(WarningState state)
        {
            var locale = LocalizationSettings.SelectedLocale;
            var stringTableOp = LocalizationSettings.StringDatabase.GetTableAsync("UI", locale);
            stringTableOp.Completed += op =>
            {
                var stringTable = op.Result;
                var text = stringTable.GetEntry(state.ToString()).GetLocalizedString();
                MakeWarningMessage(text);
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale == 0)
                {
                    gameState = GameState.Playing;
                    gamePausedPanel.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    gameState = GameState.Paused;
                    gamePausedPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }

            if (!isDebug) return;
            if (Input.GetKeyDown(KeyCode.U))
                LoadingScreen.Instance.LoadScene("NewsScene");
            else if (Input.GetKeyDown(KeyCode.I))
                LoadingScreen.Instance.LoadScene("CashierScene");
            else if (Input.GetKeyDown(KeyCode.O))
                LoadingScreen.Instance.LoadScene("ResultScene");
            else if (Input.GetKeyDown(KeyCode.P)) LoadingScreen.Instance.LoadScene("TitleScene");
        }
    }
}