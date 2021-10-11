using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDist;
    public LayerMask groundLayerMask;

    private Rigidbody2D charaRB;
    private Animator charaAnim;
    private CharacterSoundController sound;

    private bool isJumping;
    private bool isOnGround;
    

    private void Start()
    {
        charaRB = GetComponent<Rigidbody2D>();
        charaAnim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDist, groundLayerMask);
        if(hit)
        {
            if (!isOnGround && charaRB.velocity.y <= 0) isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        Vector2 velocityVector = charaRB.velocity;
        if(isJumping)
        {
            velocityVector.y += jumAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        charaRB.velocity = velocityVector;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;
                sound.PlayJump();
            }
        }
        charaAnim.SetBool("isOnGround", isOnGround);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDist), Color.white);
    }
}
