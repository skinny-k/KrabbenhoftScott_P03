using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseLook : MonoBehaviour
{
    [SerializeField] LevelController levelControl = null;
    [SerializeField] GameObject follow = null;
    [SerializeField] float lookSens = 50f;

    Vector3 focus;
    float playerSens;
    float distance;
    bool unpaused;
    
    void Start()
    {
        distance = Vector3.Distance(focus, transform.position);
        Cursor.visible = false;
    }
    
    void Update()
    {
        unpaused = levelControl.unpaused;
        focus = follow.transform.position + new Vector3(0, 1, 0);
        
        CheckCursorVis();
        
        float mouseX = Input.GetAxis("Mouse X") * lookSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * -lookSens * Time.deltaTime;

        float targetRotation = transform.eulerAngles.x + mouseY;
        if (targetRotation >= 180)
        {
            targetRotation -= 360f;
        }
        if (targetRotation > 90f)
        {
            mouseY = Mathf.Clamp(mouseY, Mathf.NegativeInfinity, 0f);
        }
        else if (targetRotation < -30f)
        {
            mouseY = Mathf.Clamp(mouseY, 0f, Mathf.Infinity);
        }

        if (unpaused)
        {
            transform.RotateAround(focus, Vector3.up, mouseX);
            transform.RotateAround(focus, transform.right, mouseY);
        }
    }

    void CheckCursorVis()
    {
        if (unpaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
