using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button ThisBtn;
    public int sceneToLoad;
    void Start()
    {
        Button btn = ThisBtn;
        btn.onClick.AddListener(StartThaVideogameBoss);
    }
    public void StartThaVideogameBoss()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
