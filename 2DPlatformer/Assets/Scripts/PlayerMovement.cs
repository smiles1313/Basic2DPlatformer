using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded = false;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 8f;
    private float movement = 0f;
    
    
    // Adding animator here bc it's easier to reference 
    //[SerializeField] private Animator animator;
    private Animator animator;
    
    private bool facingRight = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    
    void OnMove(InputValue value)
    {
        movement = value.Get<float>();
        Debug.Log(movement);
        
        // These get added after Flip();
        if (movement < 0 && facingRight == true)
        {
            Flip();
        }

        if (movement > 0 && facingRight == false)
        {
            Flip();
        }
            
    }

    void Update()
    {
        Move(movement);
        animator.SetFloat("Speed", Mathf.Abs(speed * movement));
        
    }
    
    private void Move(float x)
    {   
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Vector2 jumpAdder = new Vector2(rb.linearVelocity.x, jumpHeight);
        rb.AddForce(jumpAdder, ForceMode2D.Impulse);
        Debug.Log("Is Jumping");        
    }

    // Consider adding a bool for negative downward velocity in order to determine isFalling
    
    void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("Is not Jumping");
        isGrounded = false;
    }

    // Ground Check 
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // This gets added after the discussion of flipping a character -- it's not even an animator thing!
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    
}