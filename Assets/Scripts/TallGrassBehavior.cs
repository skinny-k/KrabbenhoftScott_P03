using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrassBehavior : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();
            player.isConcealed = true;
            player.hidingIn = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerAbilities player = other.GetComponent<PlayerAbilities>();
            if (player.hidingIn == this)
            {
                player.isConcealed = false;
            }
        }
    }
}
