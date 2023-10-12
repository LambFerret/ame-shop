using System;
using System.Collections.Generic;
using DG.Tweening;
using ingredient;
using manager;
using player;
using player.data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace skewer
{
    public class ToppingItemButton : MonoBehaviour, IDataPersistence
    {
        public SkewerController skewer;
        public Topping topping;
        public int usedAmount;

        private Button _button;
        private GameObject _toppingPrefab;
        private RectTransform _toppingPosition;
        private TextMeshProUGUI _text;

        private bool _isOnMouse;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
            _toppingPrefab = Instantiate(topping.prefab, transform);
            _toppingPosition = _toppingPrefab.GetComponent<RectTransform>();
            _button.onClick.AddListener(OnClickTopping);
        }

        /* On Topping Changed?
        private void Start()
        {
            GameEventManager.Instance.OnIngredientChanged += OnIngredientChanged;
        }

        private void OnDestroy()
        {
            GameEventManager.Instance.OnIngredientChanged -= OnIngredientChanged;
        }
        */

        public void OnClickTopping()
        {
            if (skewer.whatsOnHand == SkewerController.WhatsOnHand.None)
            {
                _isOnMouse = true;
            }
            else if (skewer.whatsOnHand == SkewerController.WhatsOnHand.Topping)
            {
                _toppingPrefab.transform.localPosition = Vector3.zero;
                _isOnMouse = false;
                skewer.whatsOnHand = SkewerController.WhatsOnHand.None;
            }
        }


        private void Update()
        {
            if (_isOnMouse)
            {
                FollowMouse();

                if (Input.GetMouseButtonDown(0))
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();

                    EventSystem.current.RaycastAll(pointerData, results);
                    bool continueFlag = false;
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.transform.parent.parent.name is "ChoppingBoard")
                        {
                            continueFlag = true;
                        }
                    }

                    if (!continueFlag)
                    {
                        OnClickTopping();
                        return;
                    }

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.name.Equals("Candy"))
                        {
                            var c = Instantiate(_toppingPrefab, _toppingPrefab.transform.position,
                                _toppingPrefab.transform.rotation, result.gameObject.transform);

                            c.transform.localScale = new Vector3(1, 1, 1);
                            break;
                        }
                    }
                }
            }
        }

        private void FollowMouse()
        {
            skewer.whatsOnHand = SkewerController.WhatsOnHand.Topping;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_toppingPrefab.transform.parent as RectTransform,
                Input.mousePosition, null, out Vector2 localPoint);
            _toppingPrefab.transform.localPosition = localPoint;
        }


        public void LoadData(GameData data)
        {
        }

        public void SaveData(GameData data)
        {
        }
    }
}