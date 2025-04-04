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
            if (WorktimeManager.Instance.IsWorkDayNow) return;

            if (_isOpened)
            {
                _animator.Play("close");
                WorktimeManager.Instance.EndDay();
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

