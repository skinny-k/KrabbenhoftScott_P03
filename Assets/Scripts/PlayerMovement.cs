using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] LevelController levelControl = null;
    [SerializeField] GameObject art = null;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float turnBuffer = 0.25f;
    [SerializeField] float sprintScale = 2.5f;
    [SerializeField] float groundDistance = 0.05f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float terminalVelocity = 200000f;

    Animator animator;
    CharacterController body;
    LayerMask groundMask;
    Vector3 velocity = Vector3.zero;
    float moveX = 0f;
    float moveZ = 0f;
    bool unpaused;
    bool isGrounded = true;
    bool lastFrameGrounded = true;
    bool isSprinting = false;

    public bool isMoving;
    
    void Start()
    {
        animator = art.GetComponent<Animator>();
        body = GetComponent<CharacterController>();
        groundMask = LayerMask.GetMask("Ground");
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
            
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            isGrounded = Physics.CheckSphere(art.transform.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < -1.5f)
            {
                velocity.y = -1.5f;
            }

            bool isMovingX = Input.GetAxis("Horizontal") != 0;
            bool isMovingZ = Input.GetAxis("Vertical") != 0;
            isMoving = isMovingX || isMovingZ;
            Quaternion targetRotation = Quaternion.Euler(0, Camera.main.transform.localRotation.eulerAngles.y, 0);
            if ((Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.5f) && isMovingX || (Mathf.Abs(Input.GetAxis("Vertical")) <= 0.5f) && isMovingZ)
            {
                art.transform.rotation = Quaternion.Slerp(art.transform.rotation, targetRotation, turnBuffer);
            }
            else if (isMoving)
            {
                art.transform.rotation = targetRotation;
            }

            if (isGrounded)
            {
                moveX = Input.GetAxis("Horizontal");
                moveZ = Input.GetAxis("Vertical");
            }
            Vector3 movement = art.transform.right * moveX + art.transform.forward * moveZ;
            body.Move(movement * moveSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded && !isMovingX && !isMovingZ)
            {
                animator.SetBool("isHighJumping", true);
                StartCoroutine(Jump(0.4f));
            }
            if (Input.GetButtonDown("Jump") && isGrounded && (isMovingX || isMovingZ))
            {
                animator.SetBool("isLongJumping", true);
                StartCoroutine(Jump(0f));
            }
            if (velocity.y <= 0f && !isGrounded)
            {
                animator.SetBool("isHighJumping", false);
                animator.SetBool("isLongJumping", false);
                lastFrameGrounded = false;
            }
            if (isGrounded && !lastFrameGrounded)
            {
                animator.SetBool("hasLanded", true);
                animator.SetBool("isWalking", isMovingX || isMovingZ);
                animator.SetBool("isCrouching", !isMovingX && !isMovingZ);
                StartCoroutine(Landing());
            }
            velocity.y = Mathf.Clamp(velocity.y - gravity * Time.deltaTime, -terminalVelocity, Mathf.Infinity);
            body.Move(velocity * Time.deltaTime);

            if (Input.GetButtonDown("Sprint") && isGrounded && !isSprinting)
            {
                moveSpeed *= sprintScale;
                isSprinting = true;
            }
            if (Input.GetButtonUp("Sprint") && isGrounded && isSprinting)
            {
                moveSpeed /= sprintScale;
                isSprinting = false;
            }
        }
    }
    
    IEnumerator Jump(float wait)
    {
        yield return new WaitForSeconds(wait);
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity);
    }

    IEnumerator Landing()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("hasLanded", false);
        animator.SetBool("isCrouching", false);
    }
}
