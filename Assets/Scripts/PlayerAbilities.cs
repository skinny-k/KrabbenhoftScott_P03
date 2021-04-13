using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    PlayerMovement movement;
    
    public TallGrassBehavior hidingIn;
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
    }
}
