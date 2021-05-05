using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] GameObject art = null;
    [SerializeField] GameObject eyeIndicator = null;
    [SerializeField] GameObject assassinateText = null;
    [SerializeField] GameObject playerBody = null;
    [SerializeField] Color alertColor = Color.red;
    [SerializeField] Color visibleColor = Color.yellow;
    [SerializeField] Color hiddenColor = Color.gray;
    [SerializeField] AudioClip stabSFX = null;
    [SerializeField] AudioClip gruntSFX = null;
    [SerializeField] AudioClip fallSFX = null;
    [SerializeField] SearchVolume search = null;
    [SerializeField] float fleeSpeed = 4f;
    [SerializeField] float alarmRadius = 1.2f;

    Animator animator;
    CharacterController body;
    Quaternion startRotation;
    PlayerAbilities player;
    AudioSource sfxPlayer;
    GameObject alarm;
    Transform target;
    bool isLooking;
    bool isAlive = true;
    
    public LevelController levelControl = null;
    public TallGrassBehavior hidingIn;
    public Transform eyes = null;
    public Vector3 lastKnownPosition;
    public float turnBuffer = 0.5f;
    public float assassinateRadius = 2.0f;
    public float range = 10f;
    public float FOV = 7.5f;
    public bool unpaused;
    public bool isConcealed;
    public bool isAlerted = false;

    void Start()
    {
        animator = art.GetComponent<Animator>();
        body = GetComponent<CharacterController>();
        player = playerBody.GetComponent<PlayerAbilities>();
        sfxPlayer = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        startRotation = transform.rotation;
        alarm = levelControl.levelAlarm;
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

            if (isAlive)
            {
                SetIndicatorState();

                if (Vector3.Distance(transform.position, playerBody.transform.position) < assassinateRadius)
                {
                    assassinateText.SetActive(true);
                    assassinateText.transform.LookAt(playerBody.transform.position);
                    player.target = this;
                }
                else
                {
                    assassinateText.SetActive(false);
                }

                if (!isAlerted)
                {
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

                if (isAlerted)
                {
                    
                    animator.SetBool("isAlerted", true);
                    transform.LookAt(alarm.transform.position);
                    body.Move(transform.forward * fleeSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, alarm.transform.position) < alarmRadius)
                    {
                        levelControl.GameOver();
                    }
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, turnBuffer);
            }
        }
    }

    void SetIndicatorState()
    {
        eyeIndicator.transform.LookAt(playerBody.transform.position);
        
        if (isAlerted)
        {
            if (eyeIndicator.transform.localScale.y <= 0.05f)
            {
                float newYScale = Mathf.Clamp(eyeIndicator.transform.localScale.y + Time.deltaTime / 5, 0f, 0.05f);
                eyeIndicator.transform.localScale = new Vector3(0.05f, newYScale, 0);
            }
            eyeIndicator.GetComponent<SpriteRenderer>().color = alertColor;
        }
        else if (search.canSeePlayer)
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

    public IEnumerator Die()
    {
        sfxPlayer.PlayOneShot(stabSFX);
        isAlive = false;
        target = playerBody.transform.GetChild(1);
        eyeIndicator.SetActive(false);
        assassinateText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isDead", true);
        sfxPlayer.PlayOneShot(gruntSFX);
        Pace movement = GetComponent<Pace>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        CorpseBehavior corpse = GetComponent<CorpseBehavior>();
        GetComponent<CorpseBehavior>().enabled = true;
        GetComponent<CorpseBehavior>().sfxPlayer = sfxPlayer;
        GetComponent<CorpseBehavior>().fallSFX = fallSFX;
        GetComponent<CorpseBehavior>().isConcealed = isConcealed;
        if (isAlerted)
        {
            levelControl.alertCounter--;
        }
        this.enabled = false;
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
