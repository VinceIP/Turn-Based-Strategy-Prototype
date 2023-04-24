using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    [Header("Core")]
    public GameObject Tile;
    public GameObject Cursor;
    public GameObject HumanPlayer;
    public GameObject AIPlayer;

    [Header("UI")]
    public GameObject SavedMapButton;

    [Header("Units")]
    public GameObject Infantry;
    public GameObject Tank;

    [Header("Buildings")]
    public GameObject Factory;
    public GameObject HQ;
    public GameObject City;
}