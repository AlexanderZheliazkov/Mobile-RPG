using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    [HideInInspector] public Vector2 moveInput;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    private const float inputDelay = .1f;

    private NavMeshAgent agent;
    private Animator anim;

    private PlayerAnimation animationBehaviour;
    private CamController cam;

    private bool canMove = true;

    void Start()
    {
        cam = transform.parent.GetComponentInChildren<CamController>();

        if (GetComponent<PlayerAnimation>())
        {
            animationBehaviour = GetComponent<PlayerAnimation>();
        }
        else
        {
            Debug.Log(transform.name + " animationBehaviour is missing!");
        }
        if (GetComponent<Animator>())
            anim = GetComponent<Animator>();
        if (GetComponent<NavMeshAgent>())
            agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (animationBehaviour != null)
        {
            if (moveInput != Vector2.zero && canMove)
                animationBehaviour.SetMoveInput(true);
            else
                animationBehaviour.SetMoveInput(false);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
            Movement();
    }

    void LateUpdate()
    {
        if (canMove)
            Rotation();
    }

    void Movement()
    {
        if (moveInput.magnitude >= inputDelay)
        {
            agent.velocity = transform.TransformDirection(moveInput.magnitude * Vector3.forward * agent.speed);
        }
    }

    void Rotation()
    {
        if (moveInput.magnitude >= inputDelay)
        {
            float targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.transform.localEulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
    }

    public void CanMove()
    {
        canMove = true;
    }

    public void CanNotMove()
    {
        canMove = false;
    }
}
