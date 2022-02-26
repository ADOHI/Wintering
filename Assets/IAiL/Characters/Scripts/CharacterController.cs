using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BoolReference isLaddering;
    public FloatReference groundHeight;

    public float moveSpeed;
    public float maxSpeed;
    public float jumpPower;
    SpriteRenderer spriterenderer;
    Rigidbody2D rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Jummp
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // Direction Sprite
        if(Input.GetButton("Horizontal"))
            spriterenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Animation
        if (Mathf.Abs (rigid.velocity.x) < 0.3)
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        CharacterMove();    // 캐릭터 움직임

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Ground"));
        groundHeight.Value = rayHit.distance;

        // Landing Platform
        if (rigid.velocity.y <= 0)
        {
            Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }

        isLaddering.Value = isLadder;

        // 사다리 타기
        if (isLadder)
        {

            float ver = Input.GetAxis("Vertical");
            if (!(ver > 0))
                return;
            rigid.gravityScale = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, ver * moveSpeed);
            if (ver > 0)
                anim.SetBool("isLaddering", true);
        }
        else
        {
            rigid.gravityScale = 4f;
            anim.SetBool("isLaddering", false);
        }
    }

    // 캐릭터 움직임
    void CharacterMove()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max speed
        if (rigid.velocity.x > maxSpeed)    // right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < maxSpeed*(-1))    // left max speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
    }

    // 사다리 접촉 여부
    public bool isLadder;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isLadder = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            isLadder = false;
    }
}