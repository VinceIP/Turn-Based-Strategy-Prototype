using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Busted.Data;

namespace Busted.UI
{
    public class UIController : MonoBehaviour
    {
        public GameController gameController;
        public List<GameObject> menuPanels;

        public GameObject pnl_LoadMap;
        public GameObject pnl_LoadMap_Container;
        public GameObject pnl_blackout;
        public int previousSib_blackout;
        public bool inMenu;

        public delegate void MenuToggled();
        public static MenuToggled menuToggled;

        public ModalPanel modalPanel;

        public UnityAction modalYesAction;
        public UnityAction modalNoAction;
        public UnityAction modalCancelAction;

        protected LoadSavedMapList savedMapList;

        protected virtual void Awake()
        {
            gameController = GRID.GameController;
            savedMapList = gameObject.AddComponent<LoadSavedMapList>();
            savedMapList.container = pnl_LoadMap_Container.transform;
            menuToggled += OnMenuToggled;

        }

        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
            menuToggled -= OnMenuToggled;
        }

        protected void OnMenuToggled()
        {
            inMenu = !inMenu;
        }

        protected void ToggleMenu(GameObject target)
        {
            foreach (GameObject r in menuPanels)
            {   //Disable any other open menus
                if (r != target && r.activeInHierarchy)
                {
                    r.SetActive(false);
                }

            }
            if (target.activeInHierarchy == true)
            {
                pnl_blackout.SetActive(false); //if panel is active now, we muct be turning it off, so turn off blackout
            }
            else pnl_blackout.SetActive(true);
            //pnl_blackout.transform.SetSiblingIndex(target.transform.GetSiblingIndex());
            target.SetActive(!target.activeInHierarchy);
            menuToggled();
        }

        public void clicked_ExitToTitle()
        {
            SceneManager.LoadScene("title");
        }

        public void clicked_LoadMap()
        {
            ToggleMenu(pnl_LoadMap);
            savedMapList.UpdateMaps();

        }

        public virtual void clicked_LoadMap_Cancel()
        {

        }
    }
}
