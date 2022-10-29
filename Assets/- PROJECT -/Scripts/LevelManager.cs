using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Awake() {
        if (PlayerPrefs.HasKey("Level"))
        {
            if (PlayerPrefs.HasKey("RandomLevel"))
                SceneManager.LoadScene(PlayerPrefs.GetInt("RandomLevel"));
            else
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            SceneManager.LoadScene(1);
        }
    }
}
