using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl control;
    public GameObject pausePanel;
    public GameObject deathPanel;
    public GameObject winPanel;

    private bool screenBusy = false;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }

        pausePanel.SetActive(false);
        deathPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pausePanel.activeInHierarchy && !screenBusy)
                {
                    Pause();
                }
                else if (pausePanel.activeInHierarchy && !screenBusy)
                {
                    Continue();
                }
            }
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0;
        deathPanel.SetActive(true);
        screenBusy = true;
    }

    public void PlayerVictory()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
        screenBusy = true;        
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    private void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

}
