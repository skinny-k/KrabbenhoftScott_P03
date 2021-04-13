using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] LevelController levelControl = null;
    [SerializeField] GameObject art = null;
    [SerializeField] GameObject eyeIndicator = null;
    [SerializeField] GameObject playerBody = null;
    [SerializeField] Color visibleColor = Color.yellow;
    [SerializeField] Color hiddenColor = Color.gray;
    [SerializeField] SearchVolume search = null;
    [SerializeField] float turnBuffer = 0.5f;

    Animator animator;
    Quaternion startRotation;
    PlayerAbilities player;
    bool isLooking;
    
    public Transform eyes = null;
    public Vector3 lastKnownPosition;
    public float range = 10f;
    public float FOV = 7.5f;
    public bool unpaused;

    void Start()
    {
        animator = art.GetComponent<Animator>();
        player = playerBody.GetComponent<PlayerAbilities>();
        startRotation = transform.rotation;
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

            SetIndicatorState();

            if (search.playerInLOS && !player.isHidden)
            {
                lastKnownPosition = playerBody.transform.position;
                isLooking = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, eyes.rotation.eulerAngles.y, 0), turnBuffer);
                eyes.LookAt(lastKnownPosition);
            }

            if (search.playerInLOS && !search.canSeePlayer && isLooking)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, eyes.rotation.eulerAngles.y, 0), turnBuffer);
                StartCoroutine(Search(5f));
            }
        }
    }

    void SetIndicatorState()
    {
        eyeIndicator.transform.LookAt(playerBody.transform.position);
        
        if (search.canSeePlayer)
            {
                if (eyeIndicator.transform.localScale.y <= 0.05f)
                {
                    float newYScale = Mathf.Clamp(eyeIndicator.transform.localScale.y + Time.deltaTime / 5, 0f, 0.05f);
                    eyeIndicator.transform.localScale = new Vector3(0.05f, newYScale, 0);
                }
                eyeIndicator.GetComponent<SpriteRenderer>().color = visibleColor;
            }
            else if (search.playerInLOS)
            {
                if (eyeIndicator.transform.localScale.y < 0.05f)
                {
                    float newYScale = Mathf.Clamp(eyeIndicator.transform.localScale.y + Time.deltaTime / 5, 0f, 0.05f);
                    eyeIndicator.transform.localScale = new Vector3(0.05f, newYScale, 0);
                }
                eyeIndicator.GetComponent<SpriteRenderer>().color = hiddenColor;
            }
            else
            {
                if (eyeIndicator.transform.localScale.y > 0f)
                {
                    float newYScale = Mathf.Clamp(eyeIndicator.transform.localScale.y - Time.deltaTime / 5, 0f, 0.05f);
                    eyeIndicator.transform.localScale = new Vector3(0.05f, newYScale, 0);
                }
                eyeIndicator.GetComponent<SpriteRenderer>().color = hiddenColor;
            }
    }

    IEnumerator Search(float searchTime)
    {
        yield return new WaitForSeconds(searchTime);
        if (lastKnownPosition != playerBody.transform.position)
        {
            isLooking = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, turnBuffer);
        }
    }
}
