using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseBehavior : MonoBehaviour
{
    public bool isConcealed = false;

    void Start()
    {
        gameObject.tag = "Corpse";
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }
}
