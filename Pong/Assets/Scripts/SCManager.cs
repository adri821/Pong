using UnityEngine;
using UnityEngine.SceneManagement;

public class SCManager : MonoBehaviour
{
    public static SCManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame() {
        #if UNITY_STANDALONE_WIN
        Application.Quit();
        #endif
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
