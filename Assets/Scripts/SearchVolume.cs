using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchVolume : MonoBehaviour
{
    GameObject origin;
    EnemyBehavior enemy;
    PlayerAbilities playerAbilities;
    float range;
    
    public GameObject player = null;
    public bool playerInLOS = false;
    public bool canSeePlayer = false;

    void Start()
    {
        origin = transform.parent.gameObject;
        enemy = origin.GetComponent<EnemyBehavior>();
        range = enemy.range;
        transform.localScale = new Vector3(enemy.FOV, 1f, range);
    }
    
    void Update()
    {
        if (playerInLOS && player != null && !playerAbilities.isHidden)
        {
            RaycastHit hit;
            enemy.eyes.LookAt(player.transform.position);
            Physics.Raycast(enemy.eyes.position, enemy.eyes.transform.forward, out hit, range);
            if (hit.transform.gameObject.tag == "Player")
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerAbilities = player.GetComponent<PlayerAbilities>();
            playerInLOS = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerInLOS = false;
        }
    }
}
