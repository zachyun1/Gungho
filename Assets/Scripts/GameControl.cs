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

    public AudioClip[] audioClips;

    public GameObject player;
    
    AudioSource m_Audio;
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

        m_Audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pausePanel.activeInHierarchy && !screenBusy)
                {
                    Pause();
                }
                else if (pausePanel.activeInHierarchy && screenBusy)
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

        player.GetComponent<Animator>().SetTrigger("Death");
        deathPanel.SetActive(true);
        screenBusy = true;
        StartCoroutine(LateCall(2.0f));
    }

    public void PlayerVictory()
    {
        player.GetComponent<Animator>().SetTrigger("Victory");
        winPanel.SetActive(true);
        screenBusy = true;
        StartCoroutine(LateCall(2.0f));
    }

    private void Pause()
    {
        m_Audio.clip = audioClips[0];
        m_Audio.Play(0);

        Time.timeScale = 0;
        pausePanel.SetActive(true);
        screenBusy = true;
    }

    private void Continue()
    {
        m_Audio.clip = audioClips[1];
        m_Audio.Play(0);

        Time.timeScale = 1;
        pausePanel.SetActive(false);
        screenBusy = false;
    }

    public bool getPauseState()
    {
        return screenBusy;
    }

    IEnumerator LateCall(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0.0f;
    }

}
