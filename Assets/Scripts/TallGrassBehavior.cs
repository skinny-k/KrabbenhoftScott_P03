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
        else if (other.gameObject.tag == "Enemy")
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            enemy.isConcealed = true;
            enemy.hidingIn = this;
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
        else if (other.gameObject.tag == "Enemy")
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy.hidingIn == this)
            {
                enemy.isConcealed = false;
            }
        }
    }
}
