using UnityEngine;

namespace Core.IceCreamMaker
{
    /// <summary>
    /// Destroys every ice cream ball that falls out of cone in trigger
    /// </summary>
    public class IceCreamBallsDeadZone : MonoBehaviour
    {
        [SerializeField] private string _ballsTag;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_ballsTag))
                Destroy(other.gameObject);
        }
    }
}


