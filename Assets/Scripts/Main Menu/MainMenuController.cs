using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneController.Instance.LoadNewScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
