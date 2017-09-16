using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerController2D : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private LayerMask m_WhatIsGround;

    public float speed = 5.0f;

    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;

    private float lookx;
    private bool m_FacingRight = true;
    private bool m_Grounded;
    private Transform m_GroundCheck;
    const float k_GroundedRadius = .2f;
    private float animDelay = 0.2f;
    private bool freeInput = true;
    private float stunDuration = 0;
    private GameObject prefab;
    private SpriteRenderer SpriteR;
    private bool release = false;

    // Use this for initialization
    void Start () {
        prefab = (GameObject)Resources.Load("SwordImpact");
        m_GroundCheck = transform.Find("GroundCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if(stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
            freeInput = false;
        }
        else if(release)
        {
            freeInput = true;
            release = false;
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        if (freeInput)
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            if (h != 0)
            {
                Move(h);
            }
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (v != 0)
            {
                MoveVert(v);
            }
        }
        // Pass all parameters to the character control script.
        

    //    m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
     //   Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        if (colliders[i].gameObject != gameObject)
    //            m_Grounded = true;
    //    }
        // m_Anim.SetBool("Ground", m_Grounded);
        m_Anim.SetBool("Ground", true);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

    }


    public void Move(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("Speed", Mathf.Abs(move));

        // Move the character
        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
            lookx = 1;
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
            lookx = -1;
        }
    }


    public void MoveVert(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("Speed", Mathf.Abs(move));

        // Move the character
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, move * m_MaxSpeed);

    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public float getLookX()
    {
        return lookx;
    }

    public bool getFacing()
    {
        return m_FacingRight;
    }

    public bool getInputState()
    {
        return freeInput;
    }

    public void setInputState(bool state)
    {
        freeInput = state;
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody2D.velocity = m_Anim.deltaPosition / Time.deltaTime;
    }

    public void stunPlayer(float duration)
    {
        release = true;
        stunDuration = duration;
    }

}
