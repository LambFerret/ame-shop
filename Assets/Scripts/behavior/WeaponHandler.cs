using customer;
using skewer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace behavior
{
    public class WeaponHandler : MonoBehaviour, IPointerDownHandler
    {
        public SkewerController skewerController;

        private void Awake()
        {
            skewerController = FindObjectOfType<SkewerController>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (skewerController.whatsOnHand != SkewerController.WhatsOnHand.Weapon) return;
            var hitCollider = Physics2D.OverlapPoint(eventData.position);
            if (hitCollider != null && hitCollider.CompareTag("Customer"))
            {
                CustomerBehavior customer = hitCollider.transform.parent.GetComponent<CustomerBehavior>();
                if (customer.IsAccepted()) return;
                Debug.Log("u poke customer with skewer.");
                customer.Serve(Customer.QuoteLine.Poked, 0);
            }
        }
    }
}