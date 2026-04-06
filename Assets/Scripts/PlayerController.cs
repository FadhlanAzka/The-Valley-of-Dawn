using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    [SerializeField] float RotationSpeed = 500f;

    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;

    Quaternion targetRotation;

    Animator animator;
    CameraController cameraController;
    CharacterController characterController;

    public Collider swordCollider;
    public GameObject SwordDraw;
    public GameObject SwordSheath;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveAmount = 1f;
            MovementSpeed = 5f;
        }
        else
        {
            moveAmount = Mathf.Clamp01(moveAmount * 0.5f);
            MovementSpeed = 2f;
        }

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        Debug.Log("isGrounded = " + isGrounded);
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * MovementSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            if(SwordDraw.activeSelf)
            {
                animator.SetBool("isAttack", true);
                swordCollider.enabled = true;
                SwordSheath.SetActive(false);
            }
            else
            {
                animator.SetTrigger("isSheath");
                SwordDraw.SetActive(true);
                animator.SetTrigger("isIdle");
            }
            
        }
        else
        {
            animator.SetBool("isAttack", false);
            swordCollider.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            if (SwordDraw.activeSelf)
            {
                animator.SetBool("isBlock", true);
            }
        }
        else
        {
            animator.SetBool("isBlock", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("isSheath");
            if (SwordDraw.activeSelf)
            {                
                SwordDraw.SetActive(false);
                SwordSheath.SetActive(true);                
            }
            else
            {                
                SwordDraw.SetActive(true);
                SwordSheath.SetActive(false);                
            }
            animator.SetTrigger("isIdle");
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);

        // Check if the sword collider is active
        if (swordCollider != null && swordCollider.enabled)
        {
            // Draw a wireframe around the sword collider
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireMesh(swordCollider.GetComponent<MeshFilter>().sharedMesh, swordCollider.transform.position, swordCollider.transform.rotation, swordCollider.transform.lossyScale);
        }
    }

}