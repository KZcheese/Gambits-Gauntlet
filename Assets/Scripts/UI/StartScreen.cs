using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class StartScreen : UIController
    {
        private HashSet<Menu> _menus;
        public Menu activeMenu;

        protected virtual void Start()
        {
            GetMenus();
            ChangeMenu(activeMenu);
        }

        protected void GetMenus()
        {
            _menus = new HashSet<Menu>(FindObjectsByType<Menu>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        }

        public void ChangeMenu(Menu newMenu)
        {
            activeMenu = newMenu;
            foreach (Menu menu in _menus)
                menu.gameObject.SetActive(menu == activeMenu);
        }
    }
}