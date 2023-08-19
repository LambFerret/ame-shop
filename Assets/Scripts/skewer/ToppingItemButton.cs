using System;
using System.Collections.Generic;
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
        private TextMeshProUGUI _text;

        private bool _isOnMouse;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
            _toppingPrefab = Instantiate(topping.prefab, transform);
            _button.onClick.AddListener(OnClickTopping);
        }

        /* On Topping CHanged?
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
                    // EventSystem에서 현재의 PointerEventData 생성
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = Input.mousePosition;

                    // Raycasting 결과를 저장할 리스트 생성
                    List<RaycastResult> results = new List<RaycastResult>();

                    // GraphicRaycaster로 Raycasting 수행
                    EventSystem.current.RaycastAll(pointerData, results);

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.name.Equals("Candy"))
                        {
                            var c = Instantiate(_toppingPrefab, _toppingPrefab.transform.position,
                                _toppingPrefab.transform.rotation, result.gameObject.transform);

                            c.transform.localScale = new Vector3(1, 1, 1);

                            break; // 조건을 만족하는 첫 번째 대상만 처리하고 반복 종료
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