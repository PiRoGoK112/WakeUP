using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Levels");
    }

     public void StartLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }   

    public void ExitGame()
    {
        Application.Quit();
    }
}
