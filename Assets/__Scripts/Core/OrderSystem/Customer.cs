using System.Collections.Generic;
using System.Linq;
using Core.Localization;
using Player.Interaction;
using Player.Inventory;
using TMPro;
using UnityEngine;

namespace Core.OrderSystem
{
    public class Customer : MonoBehaviour, IOnceInteractable
    {
        [SerializeField] private float _checkRadius = 1f;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Transform _iceCreamTPosition;
        
        private OrderData _order;
        private Transform _iceCreamTransform;

        public void SetOrder(OrderData order)
        {
            _order = order;
            _text.text = _order.PrintOrder();
        }

        public void SetIceCreamTransform(Transform t) => _iceCreamTransform = t;

        public void Interact()
        {
            if (!PlayerInventory.Instance.IsHolding) return;

            _iceCreamTransform = PlayerInventory.Instance.ObjectInHand.transform;
            
            PlayerInventory.Instance.ReleaseItem(false);
            
            _iceCreamTransform.SetParent(_iceCreamTPosition);
            _iceCreamTransform.localPosition = Vector3.zero;
            
            CheckOrder();
        }
        
        public void CheckOrder()
        {
            bool successed = false;
            var scoops = GetScoops();

            if (scoops.Count < _order.MinimumScoops)
            {
                ResultOrder(false);
            }
            else {

                // Calc order flavours count
                Dictionary<Flavour, int> requiredFlavoursCount = new();
                foreach (var flavour in _order.RequiredFlavours)
                {
                    if (requiredFlavoursCount.ContainsKey(flavour))
                        requiredFlavoursCount[flavour]++;
                    else
                        requiredFlavoursCount[flavour] = 1;
                }

                // Calc player flavours count
                Dictionary<Flavour, int> playerFlavoursCount = new();
                foreach (var scoop in scoops)
                {
                    if (playerFlavoursCount.ContainsKey(scoop.Flavour))
                        playerFlavoursCount[scoop.Flavour]++;
                    else
                        playerFlavoursCount[scoop.Flavour] = 1;
                }

                successed = requiredFlavoursCount.SequenceEqual(playerFlavoursCount);
                ResultOrder(successed);
            }
            Debug.Log("CheckOrder");
            
            CustomerManager.Instance.EndOrder(_order, successed);
            Destroy(_iceCreamTransform.gameObject);
            _order = null;
        }

        private void ResultOrder(bool isCorrect)
        {
            string msg = isCorrect ? TranslationManager.Instance.GetTranslation("NPC.OrderSuccess") : TranslationManager.Instance.GetTranslation("NPC.OrderFail");
            _text.text = msg;
        }
        
        private List<IceCreamIdentifier> GetScoops()
        {
            Collider[] colliders = Physics.OverlapSphere(_iceCreamTransform.position, _checkRadius);
            List<IceCreamIdentifier> scoops = new();

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out IceCreamIdentifier scoop))
                    scoops.Add(scoop);
            }
            
            return scoops;
        }
        
        
    }
}