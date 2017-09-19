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
    public GameObject missionPanel;
    public GameObject player;

    public AudioClip[] audioClips;
    public GameObject[] checkpoints;


    
    AudioSource m_Audio;
    private bool screenBusy = false;
    int activeCheckpoint = 0;

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
        missionPanel.SetActive(false);

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

        if (!player)
        {
            if (!checkpoints[0])
            {
                GameObject points = GameObject.FindGameObjectWithTag("Checkpoints");
                if (points)
                {
                    for (int i = 0; i < points.transform.childCount; i++)
                    {
                        checkpoints[i] = points.transform.GetChild(i).gameObject;
                    }
                }
            }

            player = GameObject.FindGameObjectWithTag("Player");
            if(player)
            {
                player.transform.position = checkpoints[activeCheckpoint].transform.position;
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }

        }

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

    public void MissionScreen()
    {
        Time.timeScale = 0;
        missionPanel.SetActive(true);
        screenBusy = true; 
    }

    public void ExitMissionScreen()
    {
        Time.timeScale = 1;
        missionPanel.SetActive(false);
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

    public void LoadCheckpoint()
    {
        player.GetComponent<Animator>().SetTrigger("Reactivate");
        deathPanel.SetActive(false);
        screenBusy = false;

        Time.timeScale = 1.0f;
        StopAllCoroutines();

        SceneManager.LoadScene(1);


    }

    public void LoadMainMenu()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);
    }

    public void LoadLevelStart()
    {
        player.GetComponent<Animator>().SetTrigger("Reactivate");
        deathPanel.SetActive(false);
        winPanel.SetActive(false);
        screenBusy = false;

        Time.timeScale = 1.0f;
        StopAllCoroutines();

        activeCheckpoint = 0;

        SceneManager.LoadScene(1);
        MissionScreen();
    }

    public void SetActiveCheckpoint(int point)
    {
        activeCheckpoint = point;
    }

    public void UnpauseForMenu()
    {
        Continue();
    }

    public void ResetDeathScreen()
    {
        StopAllCoroutines();
        deathPanel.SetActive(false);
        screenBusy = false;
        Time.timeScale = 1.0f;
    }

    public void ResetVictoryScreen()
    {
        StopAllCoroutines();
        winPanel.SetActive(false);
        screenBusy = false;
        Time.timeScale = 1.0f;
    }



}
