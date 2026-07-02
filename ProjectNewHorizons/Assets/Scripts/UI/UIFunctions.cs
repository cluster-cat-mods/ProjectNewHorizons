using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);   
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    public void TimeMult(int amount)
    {
        if (Time.timeScale > 1)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale *= amount;
        }
    }

    public void OpenFeedbackForm(string url)
    {
        Application.OpenURL(url);
    }
}
