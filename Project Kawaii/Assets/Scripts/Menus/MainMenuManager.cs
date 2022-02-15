using UnityEngine;
using MikelW.Menus;
using MikelW.Statics;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        MenuFunctions.LoadScene("LoadingScene");
    }

    public void LoadGame()
    {
        //Add in loading functions and assign to StaticData.sceneIndexToLoad
        MenuFunctions.LoadScene("LoadingScene");
    }

    public void ExitGame()
    {
        MenuFunctions.ExitProject();
    }
}