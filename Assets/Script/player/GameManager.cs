using Script.customer;
using Script.setting;
using Script.title;
using UnityEngine;

namespace Script.player
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public CustomerManager customerManager;
        public IngredientManager ingredientManager;
        public GameObject gamePausedPanel;
        public GameObject warningMessagePrefab;

        public bool isDebug;

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

            customerManager = transform.Find("CustomerManager").GetComponent<CustomerManager>();
            ingredientManager = transform.Find("IngredientManager").GetComponent<IngredientManager>();
            ingredientManager.gameManager = this;

            if (isDebug)
            {
                Debug.Log(
                    "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
                Debug.Log("Debug mode is on.");
                Debug.Log(
                    "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.timeScale == 0)
                {
                    gamePausedPanel.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    gamePausedPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }


            if (!isDebug) return;
            if (Input.GetKeyDown(KeyCode.U))
            {
                LoadingScreen.Instance.LoadScene("NewsScene");
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                LoadingScreen.Instance.LoadScene("CashierScene");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                LoadingScreen.Instance.LoadScene("ResultScene");
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                LoadingScreen.Instance.LoadScene("TitleScene");
            }
        }
    }
}