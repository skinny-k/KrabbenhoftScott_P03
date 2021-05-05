using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pace : MonoBehaviour
{
    [SerializeField] float paceDistance = 5f;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float paceSpeed = 1f;

    EnemyBehavior enemy;
    GameObject art;
    CharacterController body;
    Animator animator;
    Quaternion aboutFace;
    float currentDistance = 0f;
    public bool turnAround = false;
    public bool move = false;
    
    void Start()
    {
        enemy = GetComponent<EnemyBehavior>();
        art = enemy.transform.GetChild(0).gameObject;
        body = GetComponent<CharacterController>();
        animator = art.GetComponent<Animator>();
        
        StartCoroutine(Pause());
    }
    
    void Update()
    {
        if(enemy.unpaused)
        {
            if (turnAround)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, aboutFace, 0.25f);
                if (transform.rotation.eulerAngles.y - aboutFace.eulerAngles.y <= 0.5f)
                {
                    turnAround = false;
                    move = true;
                    animator.SetBool("isPacing", true);
                }
            }
            else if (move)
            {
                body.Move(transform.forward * paceSpeed * Time.deltaTime);
                currentDistance += paceSpeed * Time.deltaTime;
                if (currentDistance >= paceDistance)
                {
                    animator.SetBool("isPacing", false);
                    move = false;
                    StartCoroutine(Pause());
                }
            }
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(waitTime);
        aboutFace = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
        turnAround = true;
    }
}
