using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] Text pauseNotice = null;
    [SerializeField] Text gameOverNotice = null;

    public bool gameIsOver = false;

    public GameObject levelAlarm = null;
    public bool unpaused = true;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameIsOver)
        {
            unpaused = !unpaused;
            pauseNotice.gameObject.SetActive(!pauseNotice.gameObject.activeSelf);
        }
        if (!unpaused && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();

            if (Input.GetKeyDown(KeyCode.R) && gameIsOver)
            {
                SceneManager.LoadScene("Level01");
            }
        }
    }

    public void GameOver()
    {
        gameIsOver = true;
        unpaused = false;
        gameOverNotice.gameObject.SetActive(true);
    }
}
