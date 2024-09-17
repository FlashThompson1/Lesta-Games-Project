using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject _helpPanel;

    public static LoadScene instance;


    private void Awake()
    {
        instance = this;
        Time.timeScale = 1.0f;
    }

    public void LoadNextScene(int ID)
    {
       Time.timeScale = 1;
        SceneManager.LoadSceneAsync(ID);
    }

    public void ShowHelpPanel() { 
        _helpPanel.SetActive(true);
    }

    public void ExitGame()
    {

        Application.Quit();

    }


}
