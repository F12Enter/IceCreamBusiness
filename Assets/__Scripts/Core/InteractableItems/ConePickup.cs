using UnityEngine;
using Player.Interaction;

namespace Core.InteractableItems
{
    public class ConePickup : MonoBehaviour, IOnceInteractable
    {
        public void Interact()
        {
            Player.Inventory.PlayerInventory.Instance.Pickup(gameObject);
        }
    }
}

