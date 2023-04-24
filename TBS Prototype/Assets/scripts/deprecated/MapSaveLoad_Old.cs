using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// /*/*
// namespace Busted.Data
// {
//     [Serializable]
//     public static class MapSaveLoad
//     {
//         public static FileStream selectedFile;
//         public static string mapFolderPath = Application.persistentDataPath + "/maps/";
// 
// 
//         public static void Save()
//         {
//             if (GRID.Map.mapName == "") GRID.Map.mapName = "Untitled";
//             MapSerial savedMap = Map.SerializeMap();
//             Debug.Log("Saving to: " + mapFolderPath);
//             BinaryFormatter bf = new BinaryFormatter();
//             / *
//             if (!File.Exists(Application.persistentDataPath + Map.instance.mapName + ".map")) //If the file doesn't exist
//             {
//                 //Create it
//                 Debug.Log("creating new file");
//                 FileStream newFile = File.Open(Application.persistentDataPath + Map.instance.mapName + ".map", FileMode.CreateNew);
//                 newFile.Close();
// 
//             }
//             * /
//             if (!Directory.Exists(mapFolderPath))
//             {
//                 Directory.CreateDirectory(mapFolderPath);
//             }
//             FileStream file = File.Open(Application.persistentDataPath + "/maps/" + GRID.Map.mapName + ".gd", FileMode.OpenOrCreate);
//             bf.Serialize(file, savedMap);
//             file.Close();
//             GRID.Map.OnSaved();
//         }
//         / *
//         public static void Load()
//         {
//             if(File.Exists (Application.persistentDataPath + "/savedMaps.gd"))
//             {
//                 MapSerial savedMaps = new MapSerial();
//                 Debug.Log("Loading!");
//                 BinaryFormatter bf = new BinaryFormatter();
//                 FileStream file = File.Open (Application.persistentDataPath + "/savedMaps.gd", FileMode.Open);
//                 savedMaps = (MapSerial)bf.Deserialize(file);
//                 Map.GenerateMapFromFile(savedMaps);
//                 file.Close ();
//             }
//         }
//         * /
//         /// <summary>
//         /// Load map from filename. Don't use a . file extension
//         /// </summary>
//         /// <param name="filename"></param>
//         public static void Load(string filename)
//         {
//             filename = filename + ".gd";
//             if (File.Exists(mapFolderPath + filename))
//             {
//                 SavedMap savedMap = new SavedMap();
//                 //Debug.Log("Loading!");
//                 BinaryFormatter bf = new BinaryFormatter();
//                 FileStream file = File.Open(mapFolderPath + filename, FileMode.Open);
//                 savedMap = (SavedMap)bf.Deserialize(file);
//                 Map.GenerateMapFromFile_New(savedMap);
//                 file.Close();
//             }
//             else Debug.Log("File not found: " + mapFolderPath + filename);
// 
//         }
// 
//         public static bool DoesMapExist(string mapName)
//         {
//             Debug.Log("Checking for exsistance: " + mapFolderPath + mapName + ".gd");
//             if (File.Exists(mapFolderPath + mapName + ".gd"))
//             {
//                 return true;
//             }
//             else return false;
//         }
// 
// 
//     }
// 
// }
// */
