using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void SratGame()
    {
        SceneManager.LoadScene(1);
        Map.MapManager.ClearSavedMap();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
