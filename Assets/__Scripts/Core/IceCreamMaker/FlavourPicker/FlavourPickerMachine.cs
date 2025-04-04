using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Interaction;
using Core.OrderSystem;

namespace Core.IceCreamMaker.FlavourPicker
{
    public class FlavourPickerMachine : MonoBehaviour, IOnceInteractable
    {
        [SerializeField] private Flavour _currentFlavour;

        public void Interact()
        {
            IceCreamMachine.Instance.SetFlavour(_currentFlavour);
        }
    }
}

