using UnityEngine;
using Player.Interaction;
using Core.OrderSystem;

namespace Core.IceCreamMaker.FlavourPicker
{
    public class FlavourPickerMachine : MonoBehaviour, IOnceInteractable
    {
        [SerializeField] private GameObject _greenCube;
        [SerializeField] private Vector3 _cubeOffset;
        [SerializeField] private Flavour _currentFlavour;

        public void Interact()
        {
            IceCreamMachine.Instance.SetFlavour(_currentFlavour);
            _greenCube.transform.position = transform.position + _cubeOffset;
        }
    }
}

