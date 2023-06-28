using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActionsTest actions;
    private InputAction move;
    private InputAction fire;
    private InputAction jump;
    private Vector2 movementVector;
    private Rigidbody2D rb;
    public GameObject objectToSpawn;
    public float speed;
    private Coroutine shootCoroutine;
    public bool grounded = true;
    public float jumpForce;
    public LayerMask layerMask;
    public float rayCastDistance;
    public float rayCastCircleRadius;
    public ForceMode2D jumpForceMode;
    public float speedOnGround = 0.3f;
    public float speedOnAir = 0.05f;
    public float shootCoolDown = 0.1f;
    public float maxVelocityCap = 10;
    public float deceleration = 10;

    void Awake()
    {
        actions = new PlayerActionsTest();

        move = actions.Player.Move;
        fire = actions.Player.Fire;
        jump = actions.Player.Jump;

        move.Enable();
        fire.Enable();
        jump.Enable();

        move.performed += Movement;
        move.canceled += Movement;

        fire.performed += Fire;
        fire.canceled += Fire;

        jump.performed += Jump;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //rb.MovePosition(Vector2.Lerp((Vector2)transform.position, (Vector2)transform.position + movementVector, speed)); // (Vector2)transform.position + (movementVector * speed)
        GroundedCheck();
    }

    void FixedUpdate()
    {
        // rb.MovePosition(Vector2.Lerp((Vector2)transform.position, (Vector2)transform.position + movementVector, speed)); // (Vector2)transform.position + (movementVector * speed)
        Move();
    }

    private void GroundedCheck()
    {
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, 
                                                rayCastCircleRadius, 
                                                Vector2.down, 
                                                rayCastDistance, 
                                                layerMask);
        Debug.Log(ray.collider);

        if (ray.collider)
        {
            if (ray.collider.tag == "Ground")
            {
                grounded = true;
                speed = speedOnGround;
            }
        }
        else
        {
            grounded = false;
            speed = speedOnAir;
        }
    }

    private void Move()
    {
        rb.AddForce(movementVector * speed, ForceMode2D.Impulse);
        
        if (rb.velocity.x > maxVelocityCap || rb.velocity.x < -maxVelocityCap)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 
                                    -maxVelocityCap, maxVelocityCap), 
                                    rb.velocity.y);
        }

        if (Math.Abs(movementVector.x) < 0.1f)
        {
            rb.velocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.y),
                                        new Vector2(0, rb.velocity.y),
                                        deceleration * Time.fixedDeltaTime);
        }
    }

    private void Movement(InputAction.CallbackContext context) //ADVANCED METHOD
    {
        movementVector = context.ReadValue<Vector2>();
        Debug.Log("IN MOVE");
    }
    
    private void Fire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            shootCoroutine = StartCoroutine(ShootRoutine());
        }
        else if(context.canceled)
        {
            StopCoroutine(shootCoroutine);
        }
    }

    private IEnumerator ShootRoutine()
    {
        while(true)
        {
            Instantiate(objectToSpawn, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            Debug.Log("FIRE");
            yield return new WaitForSeconds(shootCoolDown);
        }       
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(grounded)
        {
            rb.AddForce(Vector2.up * jumpForce, jumpForceMode);
            Debug.Log($"JUMP");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere((Vector2)transform.position + new Vector2(0, -0.5f), rayCastDistance);
    }
}
