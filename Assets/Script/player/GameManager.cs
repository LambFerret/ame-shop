using System.Collections.Generic;
using Script.customer;
using Script.setting;
using Script.skewer;
using Script.stage;
using UnityEngine;

namespace Script.player
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public StageController stageController;
        public CustomerManager customerManager;
        public SkewerController skewerController;
        public IngredientManager ingredientManager;

        public GameObject gamePausedPanel;

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

            stageController = transform.Find("StageController").GetComponent<StageController>();
            customerManager = transform.Find("CustomerManager").GetComponent<CustomerManager>();
            skewerController = transform.Find("SkewerController").GetComponent<SkewerController>();
            ingredientManager = transform.Find("IngredientManager").GetComponent<IngredientManager>();
            stageController.gameManager = this;
            customerManager.gameManager = this;
            skewerController.gameManager = this;
            ingredientManager.gameManager = this;
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
        }
    }
}