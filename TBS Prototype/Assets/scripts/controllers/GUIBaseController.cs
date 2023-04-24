using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Base class of all GUI Controllers
/// </summary>
public class GUIBaseController : MonoBehaviour
{
    public bool inMenu;

    public RectTransform GUIparent;

    public GameObject[] tier1;
    public List<CanvasGroup> panelList;



    #region General
    //General
    public void clicked_ReturnToMainMenu()
    {
        SceneManager.LoadScene("title");
    }


    #endregion
}
