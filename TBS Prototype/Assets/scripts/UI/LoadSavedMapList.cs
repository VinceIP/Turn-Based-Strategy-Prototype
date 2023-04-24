using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Busted.Data
{
    public class LoadSavedMapList : MonoBehaviour
    {
        public Transform container; //Container objects for buttons to parent to
        public List<SavedMap> serializedMaps; //All the maps we will display in the GUI
        public List<string> nameList = new List<string>();


        private string mapPath;

        void Start()
        {
            //serializedMaps = new List<MapSerial>();
            mapPath = Busted.Data.MapSaveLoad.mapFolderPath;
            serializedMaps = new List<SavedMap>();
            //UpdateMaps();

        }



        public void UpdateMaps()
        {
            DirectoryInfo dir = new DirectoryInfo(mapPath); //Reference to the maps directory
            FileInfo[] info = dir.GetFiles("*.gd"); //Get all map files from it
                                                    //Debug.Log(info.Length + " files found in: " + dir.FullName);
            BinaryFormatter bf = new BinaryFormatter();

            foreach (FileInfo f in info) //For every file found
            {
                FileStream fileToOpen;
                //print("Opening file: " + f.Name);
                //Open and deserialize it
                fileToOpen = File.Open(mapPath + "/" + f.Name, FileMode.Open);
                SavedMap deserializedMap = (SavedMap)bf.Deserialize(fileToOpen);
                if (!nameList.Contains(deserializedMap.mapName))
                {
                    nameList.Add(deserializedMap.mapName);
                    serializedMaps.Add(deserializedMap); //Put it in the list
                    AddMapButton(deserializedMap);
                }
                fileToOpen.Close();

            }
        }

        public void AddMapButton(SavedMap map)
        {
            //container = GameObject.FindGameObjectWithTag("SavedMapButtonContainer").transform;
            GameObject newButton = Instantiate(GRID.Prefabs.SavedMapButton, container) as GameObject;
            Busted.UI.SavedMapButton buttonData = newButton.GetComponent<Busted.UI.SavedMapButton>();
            buttonData.filename = map.mapName;
            buttonData.map = map;
            buttonData.UImapName.text = map.mapName;
            buttonData.UIdescription.text = map.mapDescription;

        }

    }

}
