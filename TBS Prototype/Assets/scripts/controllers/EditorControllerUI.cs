using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System;
using Busted.Data;

namespace Busted.UI
{
    public class EditorControllerUI : UIController
    {
        //public static EditorControllerUI instance;

        public EditorController editorController;
        public Map map;

        public GameObject buttons; //Master buttons holder\
        public int previousSib_buttons;
        public GameObject panels; //Master panels holder
        public int previousSib_panels;
        public Button btn_Terrain;
        public GameObject pnl_Terrain;
        public Button btn_Buildings;
        public GameObject pnl_Buildings;
        public Button btn_Units;
        public GameObject pnl_Units;
        public Button btn_NewMap;
        public GameObject pnl_NewMap;
        public Button btn_LoadMap;
        public Button btn_SaveMap;
        public GameObject pnl_SaveMap;
        public InputField inp_SaveMap_Name; //Content of map name in input field
        public InputField inp_NewMap_MapName;
        public InputField inp_NewMap_MapDescription;
        public InputField inp_NewMap_X;
        public InputField inp_NewMap_Y;

        public Button btn_Options;
        public GameObject pnl_Options;


        public GameObject fader;
        public RectTransform faderTrans;
        public Vector3 faderTransLock;

        public List<GameObject> editorPanels;

        public PlayerData.TeamColor currentTeam;

        public bool confirm = false;
        public bool validate = false;

        private int _resultX = 0;
        private int _resultY = 0;


        protected override void OnEnable()
        {
            Map.onMapLoaded += OnMapLoaded;
        }
        protected override void OnDisable()
        {
            Map.onMapLoaded -= OnMapLoaded;
        }

        void OnMapLoaded()
        {
            map = GRID.Map;
        }

        void Start()
        {
            previousSib_blackout = pnl_blackout.transform.GetSiblingIndex();
            previousSib_buttons = panels.transform.GetSiblingIndex();
            previousSib_panels = panels.transform.GetSiblingIndex();
            editorPanels = new List<GameObject>();
            editorPanels.Add(pnl_Terrain);
            editorPanels.Add(pnl_Units);
            editorPanels.Add(pnl_Buildings);
        }


        public void TogglePaintPanel(GameObject target)
        {
            target.SetActive(!target.activeInHierarchy);
            foreach (GameObject g in editorPanels)
            {
                if (g != target) g.SetActive(false);
            }
        }


        public void clicked_Modal_Yes()
        {
            print("Modal panel yes");
        }


        #region Map Editor
        //Map Editor

        public void clicked_ChangeTeamColor()
        {
            if (pnl_Units.activeInHierarchy == true)
            {
                if ((int)currentTeam + 1 > 3) currentTeam = 0;
                else currentTeam++;
            }
            else if (pnl_Buildings.activeInHierarchy == true)
            {
                if ((int)currentTeam + 1 > 4) currentTeam = 0;
                else currentTeam++;
            }

        }
        public void clicked_Terrain()
        {
            //pnl_Terrain.GetComponent<TogglePanel>().Toggle(false);
            TogglePaintPanel(pnl_Terrain);
        }
        public void clicked_Buildings()
        {
            TogglePaintPanel(pnl_Buildings);
        }

        public void clicked_Units()
        {
            //pnl_Units.GetComponent<TogglePanel>().Toggle(false);
            if ((int)currentTeam >= 4) currentTeam = 0;
            TogglePaintPanel(pnl_Units);
        }

        public void clicked_Terrain_Grass()
        {
            editorController.selectedTileType = Tile.TileType.GRASS;
            editorController.paintType = EditorController.PaintType.Terrain;
        }

        public void clicked_Terrain_Water()
        {
            editorController.selectedTileType = Tile.TileType.WATER;
            editorController.paintType = EditorController.PaintType.Terrain;
            //Debug.Log (editorController.selectedTile);
        }

        public void clicked_Eraser()
        {
            //editorController.selectedUnit = Unit.UnitType.Eraser;
            editorController.paintType = EditorController.PaintType.Eraser;
        }

        [EnumAction(typeof(Tile.TileType))]
        public void clicked_Terrain_selection(int type)
        {
            editorController.paintType = EditorController.PaintType.Terrain;
            editorController.selectedTileType = (Tile.TileType)type;
        }

        [EnumAction(typeof(Unit.UnitType))]
        public void clicked_Unit_selection(int type)
        {
            editorController.paintType = EditorController.PaintType.Unit;
            editorController.selectedUnit = (Unit.UnitType)type;
        }

        [EnumAction(typeof(Building.BuildingType))]
        public void clicked_BuildingType(int buildingType)
        {
            editorController.paintType = EditorController.PaintType.Building;
            editorController.selectedBuilding = (Building.BuildingType)buildingType;
        }



        public void clicked_NewMap()
        {
            //pnl_NewMap.GetComponent<TogglePanel>().Toggle(true);
            inp_NewMap_MapName.text = "Untitled";
            inp_NewMap_X.text = map.width.ToString();
            inp_NewMap_Y.text = map.height.ToString();
            //inp_NewMap_MapDescription.text = map.mapDescription;
            ToggleMenu(pnl_NewMap);

        }

        public void clicked_NewMap_Submit()
        {
            string inpX = inp_NewMap_X.text;
            string inpY = inp_NewMap_Y.text;
            if (inpX != "" && inpY != "") //Valid input
            {
                //try
                //{
                    _resultX = Convert.ToInt32(inpX);
                    _resultY = Convert.ToInt32(inpY);

                    if (!MapSaveLoad.DoesMapExist(map.mapName))
                    {
                        modalPanel.Choice("You haven't saved this map. Are you sure you wish to continue and lose unsaved progress?", ev_NewMap_Submit_Confirm, ev_GenericCancel);
                    }
                    else if (MapSaveLoad.DoesMapExist(map.mapName))
                    {
                        if (map.hasUnsavedChanges)
                        {
                            modalPanel.Choice("This map has unsaved changes. Are you sure you wish to continue and lose unsaved progress?", ev_NewMap_Submit_Confirm, ev_GenericCancel);
                        }
                        else ev_NewMap_Submit_Confirm();
                    }
                //}
                /*
                catch
                {
                    modalPanel.Message("Something went wrong.", ev_GenericCancel);
                    print("Something went wrong.");
                }*/
            }
            else //Invalid input
            {
                modalPanel.Message("Invalid map dimensions.", ev_GenericCancel);
            }
        }

        void ev_NewMap_Submit_Confirm()
        {
            Map.CreateMap(_resultX, _resultY);
            map.mapName = inp_NewMap_MapName.text;
            map.mapDescription = inp_NewMap_MapDescription.text;
            ev_GenericCancel();
            clicked_NewMap();
        }

        UnityAction ev_NewMap_Submit_ContinueWithoutSaving_Yes()
        {
            string fieldX = inp_NewMap_X.text;
            string fieldY = inp_NewMap_Y.text;
            int resultX = Convert.ToInt32(fieldX);
            int resultY = Convert.ToInt32(fieldY);
            Map.CreateMap(resultX, resultY);
            map.mapName = inp_NewMap_MapName.text;
            return null;
        }

        void ev_GenericCancel()
        {
            //print("generic cancel");
            modalPanel.ClosePanel();
        }

        public void clicked_SaveMap() //Toggle save map panel
        {
            savedMapList = editorController.gameObject.GetComponent<LoadSavedMapList>();
            savedMapList.UpdateMaps();
            inp_SaveMap_Name.text = map.mapName;
            ToggleMenu(pnl_SaveMap);
        }

        public void clicked_SaveMap_Submit()
        {
            map.mapName = inp_SaveMap_Name.text;
            MapSaveLoad.SaveMapToFile();
            //savedMapList = editorController.gameObject.GetComponent<LoadSavedMapList>();
            //savedMapList.UpdateMaps();
            ToggleMenu(pnl_SaveMap);

        }



        public void clicked_Options()
        {
            ToggleMenu(pnl_Options);
        }

        public void clicked_Options_toggleCampaign()
        {
            //map.isCampaign = !map.isCampaign;
        }

        #endregion





    }
}

