using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class MainMenu : MonoBehaviour
{
    [SerializeField][Scene] string firstLevel;

    public void OnStartClick()
    {
        SceneManager.LoadScene(firstLevel);
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
