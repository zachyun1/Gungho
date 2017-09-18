using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OnRetryClick : MonoBehaviour {

	public void Retry()
    {
        GameControl.control.UnpauseForMenu();
        GameControl.control.LoadCheckpoint();
    }

    public void BackToMenu()
    {
        GameControl.control.UnpauseForMenu();
        GameControl.control.ResetDeathScreen();
        GameControl.control.ResetVictoryScreen();
        GameControl.control.LoadMainMenu();
    }

    public void Restart()
    {
        GameControl.control.UnpauseForMenu();
        GameControl.control.ResetVictoryScreen();
        GameControl.control.LoadLevelStart();
    }

}
