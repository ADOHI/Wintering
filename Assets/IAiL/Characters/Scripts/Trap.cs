using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ingames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    Animator anim;
    
    private bool isActed;

    public float bindTime = 3f;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    async void BindCharacter(CharacterController characterController)
    {
        characterController.isControllable = false;
        //characterController.isControllable = false;

        isActed = true;

        SoundManager.Instance.PlaySFX(3);

        await UniTask.Delay((int)(bindTime * 1000f));
        
        if (IngameProgressManager.Instance.isOnGamePlaying)
        {
            characterController.isControllable = true;
        }

        GetComponent<SpriteRenderer>().DOFade(0f, 2f);
        Destroy(this.gameObject, 2.5f);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isActed)
            {
                anim.SetBool("Catchingdaramjwi", true);
                BindCharacter(collision.gameObject.GetComponent<CharacterController>());
            }
        }
    }
}