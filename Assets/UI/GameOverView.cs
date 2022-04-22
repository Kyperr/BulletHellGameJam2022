
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverView : MonoBehaviour
{
    public GameObject gameoverObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGameOver()
    {
        StartCoroutine(showGameover());
        //GetComponent<UIView>().Show();
    }

    IEnumerator showGameover()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameoverObject.SetActive(true);

    }

    public void onRestartButton()
    {

        SceneManager.LoadScene(1);
        //GetComponent<UIView>().Hide();
    }

}
