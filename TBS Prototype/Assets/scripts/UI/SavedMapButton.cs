using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Busted.Data;

namespace Busted.UI
{
    public class SavedMapButton : MonoBehaviour
    {
        [SerializeField]public SavedMap map;
        [SerializeField] public string filename;

        [SerializeField] public Text UIdescription;
        [SerializeField] public Text UImapName;
        [SerializeField] public Image UIpreview;
        [SerializeField] public RectTransform contentContainer;

        public EditorControllerUI editorControllerUI;
        public GameControllerUI gameControllerUI;

        public void clicked_Selection()
        {

            if (SceneManager.GetActiveScene().name == "editor")
            {
                editorControllerUI = GameObject.FindGameObjectWithTag("EditorControllerUI").GetComponent<EditorControllerUI>();
                MapSaveLoad.LoadMapFromFile(filename);
                //Map.GenerateMapFromFile_New(map);
                editorControllerUI.clicked_LoadMap();
            }
            else
            {
                gameControllerUI = GameObject.FindGameObjectWithTag("GameControllerUI").GetComponent<GameControllerUI>();
                gameControllerUI.clicked_LoadMap_Selection();
                //GRID.GameController.LoadNewMap(map);
                MapSaveLoad.LoadMapFromFile(filename);

            }
        }
    }
}

