using UnityEngine;

namespace Core.OrderSystem
{
    public class CustomerModelRandomizer : MonoBehaviour
    {
        [SerializeField] GameObject[] _customerModels;
    
        public void ChangeRandomModel()
        {
            foreach (var model in _customerModels)
            {
                model.SetActive(false);
            }
            
            _customerModels[Random.Range(0, _customerModels.Length)].SetActive(true);
        }
    }

}
