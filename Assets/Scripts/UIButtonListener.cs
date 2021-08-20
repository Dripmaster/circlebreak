using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonListener : MonoBehaviour
{
    public void switchToGameScene() {
        SceneManager.LoadScene("SampleScene");
    }

    public void exitGame() {
        Application.Quit();
    }
    
}
