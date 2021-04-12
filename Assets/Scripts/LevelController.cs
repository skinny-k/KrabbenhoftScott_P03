using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public bool unpaused = true;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            unpaused = !unpaused;
        }
        if (!unpaused && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            Debug.Log("Game Quit.");
        }
    }
}
