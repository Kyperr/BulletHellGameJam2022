using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    Button resumeButton;
    [SerializeField]
    Button mainButton;
    [SerializeField]
    Button restartButton;
    public void pause()
    {
        pauseButton.gameObject.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void resume()
    {

        pauseButton.gameObject.SetActive(true);
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
        pauseButton.onClick.AddListener(pause);
        resumeButton.onClick.AddListener(resume);
        mainButton.onClick.AddListener(mainMenu);
        restartButton.onClick.AddListener(restart);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
