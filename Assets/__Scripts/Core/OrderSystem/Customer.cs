using System.Collections;
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
        [SerializeField] private float _endDelay = 3f;
        [SerializeField] private float _waitTime = 60f;
        [SerializeField] private float _checkRadius = 1f;
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Transform _iceCreamTPosition;
        
        private OrderData _order;
        private Transform _iceCreamTransform;

        private bool _isAbleToCheck = false;

        public void SetOrder(OrderData order)
        {
            _order = order;
            _text.text = _order.PrintOrder();

            _isAbleToCheck = true;
            StartCoroutine(WaitOrder());
        }

        public void SetIceCreamTransform(Transform t) => _iceCreamTransform = t;

        public void Interact()
        {
            if (!PlayerInventory.Instance.IsHolding || !_isAbleToCheck) return;

            _iceCreamTransform = PlayerInventory.Instance.ObjectInHand.transform;
            
            PlayerInventory.Instance.ReleaseItem(false);
            
            _iceCreamTransform.SetParent(_iceCreamTPosition);
            _iceCreamTransform.localPosition = Vector3.zero;
            
            CheckOrder();
        }
        
        public void CheckOrder()
        {
            StopCoroutine(WaitOrder());
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

            StartCoroutine(EndOrder(successed));
        }

        private IEnumerator EndOrder(bool successed)
        {
            _isAbleToCheck = false;
            
            yield return new WaitForSeconds(_endDelay);
            
            CustomerManager.Instance.EndOrder(_order, successed);
            if (_iceCreamTransform) Destroy(_iceCreamTransform.gameObject);
            _order = null;
        }

        private IEnumerator WaitOrder()
        {
            float waitedTime = 0f;

            while (waitedTime < _waitTime)
            {
                waitedTime += Time.deltaTime;

                if (waitedTime >= _waitTime)
                {
                    ResultOrder(false, true);
                    StartCoroutine(EndOrder(false));
                    yield break;
                }
                
                yield return null;
            }
        }
        
        private void ResultOrder(bool isCorrect, bool timeOut = false)
        {
            var manager = TranslationManager.Instance;
            string msg = isCorrect ? manager.GetTranslation("NPC.OrderSuccess") : timeOut ? manager.GetTranslation("NPC.TimedOut") : manager.GetTranslation("NPC.OrderFail");
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