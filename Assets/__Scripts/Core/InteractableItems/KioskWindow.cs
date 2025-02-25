using UnityEngine;
using Player.Interaction;
using Core.Worktime;

namespace Core.InteractableItems
{
    public class KioskWindow : MonoBehaviour, IOnceInteractable
    {
        [SerializeField] Animator _animator;
        
        private bool _isOpened = false;
        
        public void Interact()
        {
            if (_isOpened)
            {
                _animator.Play("close");
            }
            else
            {
                _animator.Play("open");
                WorktimeManager.Instance.StartDay();
            }
            _isOpened = !_isOpened;
        }
    }
}

