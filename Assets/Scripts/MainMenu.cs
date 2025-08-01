using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string firstLevel;
    public void OnStartClick()
    {
        SceneManager.LoadScene(firstLevel);
    }
    public void OnExitClick()
    {
        Application.Quit();
    }
}
