using UnityEngine;

namespace Core.IceCreamMaker
{
    public class IceCreamBall : MonoBehaviour
    {
        [Header("Ball Settings")] 
        [SerializeField] private float _timeToFreeze;
        
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Invoke(nameof(Freeze), _timeToFreeze);
        }

        private void Freeze()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.freezeRotation = true;
        }
    }
}