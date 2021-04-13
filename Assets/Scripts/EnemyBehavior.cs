using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] LevelController levelControl = null;
    [SerializeField] GameObject art = null;
    [SerializeField] GameObject eyeIndicator = null;
    [SerializeField] GameObject player = null;
    [SerializeField] Color visibleColor = Color.yellow;
    [SerializeField] Color hiddenColor = Color.gray;
    [SerializeField] SearchVolume search = null;

    Animator animator;
    Vector3 lastKnownPosition;
    bool unpaused;
    
    public Transform eyes = null;
    public float range = 10f;
    public float FOV = 7.5f;

    void Start()
    {
        animator = art.GetComponent<Animator>();
    }

    void Update()
    {
        unpaused = levelControl.unpaused;
        
        if (!unpaused)
        {
            animator.speed = 0f;
        }

        if (unpaused)
        {
            animator.speed = 1f;
            eyeIndicator.transform.LookAt(player.transform.position);
            if (search.canSeePlayer)
            {
                eyeIndicator.transform.localScale = new Vector3(0.05f, 0.05f, 0);
                eyeIndicator.GetComponent<SpriteRenderer>().color = visibleColor;
            }
            else if (search.playerInLOS)
            {
                eyeIndicator.transform.localScale = new Vector3(0.05f, 0.05f, 0);
                eyeIndicator.GetComponent<SpriteRenderer>().color = hiddenColor;
            }
            else
            {
                eyeIndicator.transform.localScale = new Vector3(0.05f, 0, 0);
                eyeIndicator.GetComponent<SpriteRenderer>().color = hiddenColor;
            }

            if (search.canSeePlayer)
            {
                lastKnownPosition = player.transform.position;
            }
        }
    }
}
