using System.Collections.Generic;
using ingredient;
using manager;
using player;
using player.data;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace skewer
{
    public class PackageButton : MonoBehaviour
    {
        public SkewerController skewer;
        public GameObject prefab;
        public BoardBehavior currentBoard;

        private Button _button;
        private GameObject _toppingPrefab;
        private TextMeshProUGUI _text;
        private bool _isOnHead;

        private bool _isOnMouse;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
            _toppingPrefab = Instantiate(prefab, transform);
            _button.onClick.AddListener(OnClickTopping);
        }

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

                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.name.Equals("Candy"))
                    {
                        GameObject bone = result.gameObject;
                        RectTransform headRect = bone.transform.Find("Head").GetComponent<RectTransform>();
                        RectTransform tailRect = bone.transform.Find("Tail").GetComponent<RectTransform>();

                        if (RectTransformUtility.RectangleContainsScreenPoint(headRect, pointerData.position))
                        {
                            SoundManager.Instance.PlaySFX(SoundManager.SFX.Package1);

                            _isOnHead = true;
                        }
                        else if (_isOnHead &&
                                 RectTransformUtility.RectangleContainsScreenPoint(tailRect, pointerData.position))
                        {
                            Debug.Log("AND ON TAIL!!!");
                            Packing();
                            _isOnMouse = false;
                            _isOnHead = false; // 상태 초기화
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

        private void Packing()
        {
            currentBoard.GiveSkewerToController();
            skewer.PackUp();
        }
    }
}