using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string firstLevel;
    [SerializeField] Canvas startMenuCanvas;
    [SerializeField] Canvas optionMenuCanvas;
    public void OnStartClick()
    {
        SceneManager.LoadScene("firstLevel");
    }
    public void OnOptionsClick()
    {
        // Placeholder code.
        startMenuCanvas.gameObject.SetActive(false);
        optionMenuCanvas.gameObject.SetActive(true);
    }
    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnExitOptionsClick()
    {
        optionMenuCanvas.gameObject.SetActive(false);
        startMenuCanvas.gameObject.SetActive(true);
    }
}
