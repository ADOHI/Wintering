using Ingames;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [ShowInInspector]
    public bool isControllable;
    public BoolEventReference onControllable;


    public BoolReference isLaddering;
    public FloatReference groundHeight;
    public FloatReference staminaPoint;
    public float staminaSpendSpeed;
    public float staminaRecoverySpeed;


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

        staminaPoint.Value = 1f;
    }

    private void Start()
    {
        onControllable.Event.Register(b => isControllable = b);
    }

    void Update()
    {
        if (!isControllable)
        {
            anim.SetBool("isRunning", false);
            return;
        }

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
        if (IsGround() && Mathf.Abs(rigid.velocity.x) > 0.1f)
        {
            SoundManager.Instance.PlaySFX(0);
        }
        else
        {
            SoundManager.Instance.StopSFX(0);

        }

        if (!isControllable)
        {
            rigid.velocity.Set(0f, rigid.velocity.y);
            isLaddering.Value = false;
            rigid.gravityScale = 4f;
            
            return;
        }

        CharacterMove();    // 캐릭터 움직임

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 100f, LayerMask.GetMask("Ground"));
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
            if (!(ver > 0) || staminaPoint <= 0f)
            {
                SoundManager.Instance.StopSFX(1);

                Debug.Log("Slide");
                rigid.gravityScale = 0.5f;

                if (IsGround())
                {
                    //rigid.gravityScale = 4f;
                    anim.SetBool("isLaddering", false);
                    staminaPoint.Value = Mathf.Clamp(staminaPoint.Value + staminaRecoverySpeed * Time.fixedDeltaTime, 0f, 1f);
                }

                return;

            }
            else
            {
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x, ver * moveSpeed);
                if (ver > 0)
                {
                    anim.SetBool("isLaddering", true);
                    staminaPoint.Value = Mathf.Clamp(staminaPoint.Value - staminaSpendSpeed * Time.fixedDeltaTime, 0f, 1f);
                    SoundManager.Instance.PlaySFX(1);
                }

            }


        }
        else
        {
            rigid.gravityScale = 4f;
            anim.SetBool("isLaddering", false);
            staminaPoint.Value = Mathf.Clamp(staminaPoint.Value + staminaRecoverySpeed * Time.fixedDeltaTime, 0f, 1f);
            SoundManager.Instance.StopSFX(1);

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

    bool IsGround()
    {
        return groundHeight.Value < 0.5f;
    }

    public void SetSpecialIdle(bool specialIdle)
    {
        anim.SetBool("isSpecialIdle", specialIdle);
    }

    public void Sleep(bool isSleep)
    {
        anim.SetBool("isSleep", isSleep);
    }

    public void Die()
    {
        isControllable = false;
        anim.SetBool("isDead", true);
    }

    public void Pause()
    {
        isControllable = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Release()
    {
        //isControllable = true;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void DoFinal()
    {
        anim.SetBool("isFinal", true);
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