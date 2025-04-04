using UnityEngine;
using Player.Interaction;

namespace Core.IceCreamMaker
{
    public class ConeSpawner : MonoBehaviour, IOnceInteractable
    {
        public static ConeSpawner Instance { get; private set; }
        
        [SerializeField] private GameObject _conePrefab;
        [SerializeField] private Transform _spawnPos;
        
        public bool IsConeExist => _cone != null;
        public GameObject Cone => _cone;
        
        private GameObject _cone;
    
        public void Interact()
        {
            if (_cone != null) return;
            
            _cone = Instantiate(_conePrefab, _spawnPos.position, _spawnPos.rotation);
            IceCreamPlayerController.Instance.SetCone(_cone);
            
        }
        
        public void DeleteCone() => _cone = null;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }
    }
}

