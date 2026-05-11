using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayButtonPressed(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
