using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;

    private bool paused = false;

    public void pause()
    {
        paused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void resume()
    {
        paused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void restart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void mainMenu()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }
}
