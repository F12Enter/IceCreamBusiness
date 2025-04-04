using UnityEngine;

namespace Core.OrderSystem
{
    public class IceCreamIdentifier : MonoBehaviour
    {
        [SerializeField] private Material _vanillaMat;
        [SerializeField] private Material _chocolateMat;
        [SerializeField] private Material _strawberryMat;

        private Flavour _flavour;

        public Flavour Flavour => _flavour;

        public void Setup(Flavour flavour)
        {
            _flavour = flavour;
            var render = GetComponent<Renderer>();

            switch (flavour)
            {
                case Flavour.Vanilla:
                    render.material = _vanillaMat;
                    break;
                case Flavour.Chocolate:
                    render.material = _chocolateMat;
                    break;
                case Flavour.Strawberry:
                    render.material = _strawberryMat;
                    break;
            }
        }
    }
}