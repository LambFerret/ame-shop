using ingredient;
using manager;
using player;
using player.data;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace skewer
{
    public class MachineItemButton : MonoBehaviour, IDataPersistence
    {
        public int amount;

        private Button _button;
        public Ingredient ingredient;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = transform.Find("text").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var skewer = FindObjectOfType<SkewerController>();
            if (skewer.AddIngredientToSkewerInHand(ingredient))
            {
                SoundManager.Instance.FruitSound();
                amount--;
            }
        }

        private void Update()
        {
            _text.text = amount + "";
            _button.interactable = amount > 0;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }

        public void LoadData(GameData data)
        {
            amount = data.ingredients[IngredientManager.Instance.GetIngredientIndex(ingredient)];
            Debug.Log( "value : " + amount);
            Debug.Log(data.ingredients);
        }

        public void SaveData(GameData data)
        {
            data.ingredients[IngredientManager.Instance.GetIngredientIndex(ingredient)] = amount;
        }

        public void SetIngredient(Ingredient ingredient)
        {
            this.ingredient = ingredient;
        }

        private void OnIngredientChanged(Ingredient f, int value)
        {
            if (f == ingredient) amount += value;
        }
    }
}