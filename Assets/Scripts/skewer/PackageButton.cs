using System.Collections.Generic;
using setting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace skewer
{
    public class PackageButton : MonoBehaviour
    {
        public SkewerController skewer;
        public GameObject prefab;
        public BoardBehavior currentBoard;
        public ScrollRect scrollbar;

        public RectTransform myHand;

        private Button _button;
        // private GameObject _toppingPrefab;
        private TextMeshProUGUI _text;
        private bool _isOnHead;

        private bool _isOnMouse;

        private void Awake()
        {
            _button = transform.GetComponent<Button>();
            _text = _button.GetComponentInChildren<TextMeshProUGUI>();
            _button.onClick.AddListener(OnClickTopping);
        }

        private void OnClickTopping()
        {
            switch (skewer.whatsOnHand)
            {
                case SkewerController.WhatsOnHand.None:
                    var toppingPrefab = Instantiate(prefab, transform);
                    myHand = toppingPrefab.GetComponent<RectTransform>();
                    skewer.whatsOnHand = SkewerController.WhatsOnHand.Topping;
                    _isOnMouse = true;
                    break;
                case SkewerController.WhatsOnHand.Topping:
                    Destroy(myHand);
                    _isOnMouse = false;
                    skewer.whatsOnHand = SkewerController.WhatsOnHand.None;
                    break;
            }
        }


        private void Update()
        {
            scrollbar.horizontal = !_isOnMouse;

            if (_isOnMouse)
            {
                FollowMouse(myHand);

                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("CandyHead"))
                    {
                        _isOnHead = true;
                    }
                    else if (_isOnHead && result.gameObject.CompareTag("CandyTail"))
                    {
                        Packing();
                        _isOnMouse = false;
                        _isOnHead = false; // 상태 초기화
                    }
                }
            }
        }

        private void FollowMouse(RectTransform objectRect)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                objectRect.transform.parent as RectTransform,
                Input.mousePosition, null, out Vector2 localPoint);
            objectRect.anchoredPosition = localPoint;
        }

        private void Packing()
        {
            SoundManager.Instance.PlaySFX(Random.Range(0, 2) == 0
                ? SoundManager.SFX.Package1
                : SoundManager.SFX.Package2);
            currentBoard.GiveSkewerToController();
            skewer.PackUp();
        }
    }
}