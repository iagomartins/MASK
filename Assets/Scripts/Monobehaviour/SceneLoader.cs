using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStage(string stage) {
        SceneManager.LoadScene(stage);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
