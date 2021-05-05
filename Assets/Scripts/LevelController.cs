using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] Text pauseNotice = null;
    [SerializeField] Text gameOverNotice = null;
    [SerializeField] GameObject alert = null;

    public GameObject levelAlarm = null;
    public int alertCounter = 0;
    public bool unpaused = true;
    public bool gameIsOver = false;
    
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

        if (alertCounter > 0)
        {
            alert.SetActive(true);
        }
        else
        {
            alert.SetActive(false);
        }
    }

    public void GameOver()
    {
        gameIsOver = true;
        unpaused = false;
        alert.SetActive(false);
        gameOverNotice.gameObject.SetActive(true);
    }
}
