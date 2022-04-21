
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverView : MonoBehaviour
{
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
        gameObject.SetActive(true);
        //GetComponent<UIView>().Show();
    }

    public void onRestartButton()
    {

        SceneManager.LoadScene(1);
        //GetComponent<UIView>().Hide();
    }

}
