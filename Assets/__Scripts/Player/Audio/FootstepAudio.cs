using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Audio
{
    public class FootstepAudio : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip[] _footstepSounds;
        [SerializeField] float _stepDistance = 2.0f;
    
        private CharacterController _characterController;
        private Vector3 _lastPosition;
        private float _distanceTraveled = 0f;
        private bool _wasInAir = false;
    
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _lastPosition = transform.position;
        }
    
        void Update()
        {
            if (_characterController.isGrounded)
            {
                // �������� �� �����������
                if (_wasInAir)
                {
                    PlayRandomFootstep();
                    _wasInAir = false;
                }
    
                _distanceTraveled += Vector3.Distance(transform.position, _lastPosition);
    
                if (_distanceTraveled >= _stepDistance)
                {
                    PlayRandomFootstep();
                    _distanceTraveled = 0f;
                }
            }
            else
            {
                _wasInAir = true;
            }
    
            _lastPosition = transform.position;
        }
    
        void PlayRandomFootstep()
        {
            if (_footstepSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, _footstepSounds.Length);
                //if (audioSource.clip == footstepSounds[randomIndex]) { PlayRandomFootstep(); return; }
                //audioSource.clip = footstepSounds[randomIndex];
                //audioSource.Play();
                _audioSource.PlayOneShot(_footstepSounds[randomIndex]);
            }
        }
    
    }

}
