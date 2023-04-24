using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Busted.Data
{
    public class MapSaveLoad : MonoBehaviour
    {
        public static FileStream selectedFile;
        public static string mapFolderPath = Application.persistentDataPath + "/maps/";

        public static void SaveMapToFile()
        {
            Map currentMap = GRID.Map;
            if (currentMap.mapName == "") currentMap.mapName = "Untitled";
            SavedMap savedMap = new SavedMap();
            savedMap.mapName = currentMap.mapName;
            savedMap.mapDescription = currentMap.mapDescription;
            savedMap.width = currentMap.width;
            savedMap.height = currentMap.height;

            savedMap.tiles = new SavedTile[savedMap.width, savedMap.height];

            for(int i = 0; i < savedMap.width; i++) //Loop through every tile, set convert it to a serialiazable form
            {
                for(int j = 0; j < savedMap.height; j++)
                {
                    Tile t = currentMap.tiles[i, j].GetComponent<Tile>();
                    savedMap.tiles[i, j] = new SavedTile();
                    savedMap.tiles[i, j].tileType = t.tileType;
                    savedMap.tiles[i, j].containsUnit = null;
                    savedMap.tiles[i, j].containsBuilding = null;

                    if (t.containsUnit)
                    {
                        Debug.Log("Saving unit: " + t.containsUnit.teamColor.ToString() + "" + t.containsUnit.UnitName);
                        savedMap.tiles[i, j].containsUnit = new SavedUnit();
                        savedMap.tiles[i, j].containsUnit.unitType = t.containsUnit.unitType;
                        savedMap.tiles[i, j].containsUnit.teamColor = t.containsUnit.teamColor;
                        savedMap.tiles[i, j].containsUnit.health = t.containsUnit.Health;
                        savedMap.tiles[i, j].containsUnit.ammo = t.containsUnit.Fuel;
                        savedMap.tiles[i, j].containsUnit.fuel = t.containsUnit.Fuel;
                        //Add other stuff here later, like isTurnDone
                    }
                    else savedMap.tiles[i, j].containsUnit = null;

                    if(t.containsBuilding)
                    {
                        Debug.Log("Saving building: " + t.containsBuilding.teamColor.ToString() + "" + t.containsBuilding.buildingName);

                        savedMap.tiles[i, j].containsBuilding = new SavedBuilding();
                        savedMap.tiles[i, j].containsBuilding.buildingType = t.containsBuilding.buildingType;
                        savedMap.tiles[i, j].containsBuilding.teamColor = t.containsBuilding.teamColor;
                        //
                    }
                    else savedMap.tiles[i, j].containsBuilding = null;

                }
            }

            Debug.Log("Saving to: " + mapFolderPath);
            BinaryFormatter bf = new BinaryFormatter();
            if (!Directory.Exists(mapFolderPath))
            { //Create map folder if it doesnt exist
                Directory.CreateDirectory(mapFolderPath);
            }
            FileStream file = File.Open(Application.persistentDataPath + "/maps/" + savedMap.mapName + ".gd", FileMode.Create);
            bf.Serialize(file, savedMap);
            file.Close();
            GRID.Map.OnSaved();


        }

        public static void LoadMapFromFile(string filename)
        {
            string loadThis = mapFolderPath + filename + ".gd";
            if (File.Exists(loadThis))
            {
                SavedMap savedMap = new SavedMap();
                BinaryFormatter bf = new BinaryFormatter();
                //Debug.Log("Opening: " + loadThis);
                FileStream file = File.Open(loadThis, FileMode.Open);
                savedMap = (SavedMap)bf.Deserialize(file);
                Map.GenerateMapFromFile(savedMap);
                file.Close();
            }
            else Debug.Log("File not found: " + loadThis);
        }

        public static bool DoesMapExist(string mapName)
        {
            if (File.Exists(mapFolderPath + mapName + ".gd"))
            {
                return true;
            }
            else return false;
        }



    }
}
