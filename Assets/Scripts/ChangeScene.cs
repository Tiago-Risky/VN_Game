using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // Use this for initialization
    public void nextScene(string sceneName) {
        SceneManager.LoadScene(sceneName: sceneName);
    }
}
