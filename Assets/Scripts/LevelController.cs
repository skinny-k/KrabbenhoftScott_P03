using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] Text pauseNotice = null;
    
    public bool unpaused = true;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            unpaused = !unpaused;
            pauseNotice.gameObject.SetActive(!pauseNotice.gameObject.activeSelf);
        }
        if (!unpaused && Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
