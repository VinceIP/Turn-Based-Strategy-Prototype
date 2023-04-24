using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Busted;

public class TitleScreen : MonoBehaviour
{

    public void LaunchMapEditor()
    {
        GameController.SetScene(GameController.GameControllerSceneState.Editor);
    }

    public void clicked_WarGames()
    {
        GameController.SetScene(GameController.GameControllerSceneState.Game);
    }

    public void clicked_Exit()
    {
        Application.Quit();
    }
}
