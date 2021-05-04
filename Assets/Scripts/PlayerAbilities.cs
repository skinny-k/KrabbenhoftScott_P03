using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    
    public TallGrassBehavior hidingIn;
    public EnemyBehavior target;
    public bool isConcealed = false;
    public bool isHidden = false;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isConcealed && !movement.isMoving)
        {
            isHidden = true;
        }
        else if (isConcealed && movement.isMoving || !isConcealed)
        {
            isHidden = false;
        }

        if (target!= null && Vector3.Distance(transform.position, target.gameObject.transform.position) > target.assassinateRadius)
        {
            target = null;
        }
        if (Input.GetButtonDown("Attack"))
        {
            movement.animator.SetTrigger("attack");

            if (target != null)
            {
                StartCoroutine(target.Die());
                target = null;
            }
        }
    }
}
