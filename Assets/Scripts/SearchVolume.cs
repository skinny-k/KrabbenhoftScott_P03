using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchVolume : MonoBehaviour
{
    GameObject origin;
    EnemyBehavior enemy;
    PlayerAbilities playerAbilities;
    LayerMask mask;
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
        mask = LayerMask.GetMask("Player", "Ground");
    }
    
    void Update()
    {
        if (enemy.unpaused)
        {
            if (playerInLOS)
            {
                RaycastHit hit;
                enemy.eyes.LookAt(enemy.lastKnownPosition);
                bool ray = Physics.Raycast(enemy.eyes.position, enemy.eyes.forward, out hit, range, mask);
                if (ray && hit.transform.gameObject.tag == "Player" && !playerAbilities.isHidden)
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            playerAbilities = player.GetComponent<PlayerAbilities>();
            enemy.lastKnownPosition = player.transform.position;
            playerInLOS = true;
        }
        if (other.gameObject.tag == "Corpse")
        {
            CorpseBehavior corpse = other.GetComponent<CorpseBehavior>();
            if (!corpse.isConcealed)
            {
                enemy.isAlerted = true;
                enemy.levelControl.alertCounter++;
            }
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
