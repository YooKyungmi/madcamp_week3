using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Box : MonoBehaviour
{
    bool isLive=true;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    Rigidbody2D rigid;

    void Awake()
    {
        anim=GetComponent<Animator>();
        rigid=GetComponent<Rigidbody2D>();   
        coll=GetComponent<Collider2D>();   
        spriter=GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        isLive = true;
        coll.enabled = true;
        spriter.sortingOrder = 3;
    }

    void OnTriggerExit2D(Collider2D collision){
        if(!CompareTag("Area"))return;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        

        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        
        Array typeValues = Enum.GetValues(typeof(Drop.TypeId));
        Drop.TypeId randomType = (Drop.TypeId)typeValues.GetValue(UnityEngine.Random.Range(1, typeValues.Length));

        Drop.DropBox(randomType, transform);
        gameObject.SetActive(false);
    }

}
