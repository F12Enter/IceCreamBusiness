using UnityEngine;

namespace Core.MainMenu
{
    public class MainMenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject[] _menus;

        public void OpenMenu(int index)
        {
            for (int i = 0; i < _menus.Length; i++)
                if (_menus[i] == _menus[index])
                    _menus[i].SetActive(true);
                else
                    _menus[i].SetActive(false);
        }
    }
}

