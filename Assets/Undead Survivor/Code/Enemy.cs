using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int level;
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive=true;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;
    Animator anim;

    void Awake()
    {
        anim=GetComponent<Animator>();
        rigid=GetComponent<Rigidbody2D>();   
        coll=GetComponent<Collider2D>();   
        spriter=GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameManager.instance.isLive)
            return;
        // GetCurrentAnimatorStateInfo 현재 상태 가져오는 함수
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Debug.Log(target.position);
        Debug.Log(speed);
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 정규화 normalized, 프레임의 영향으로 결과가 달라지지  fixedDeltaTime
        Debug.Log(nextVec);
        rigid.MovePosition(rigid.position+nextVec); //플레이어의 키입력 ㄱ밧을 더한 이동 = 몬스터 방향값을 더한 이동
        rigid.velocity = Vector2.zero;  // 부딪쳐도 안 밀려남

    }

    void LateUpdate() {
        if(!GameManager.instance.isLive)
            return;

        if(!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable() {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();    
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead",false);    
        health = maxHealth;
    }

    public void Init(SpawnData data, int lv)
    {
        if (anim == null)
            Debug.Log("error");
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed * (1f + 0.3f * Mathf.Log(10, lv));
        maxHealth = data.health * lv;
        health = data.health * lv;
        level = lv;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        
        health-=collision.GetComponent<Bullet>().damage;

        if (health >0){
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead",true);
            GameManager.instance.kill++;
            //GameManager.instance.GetExp(); // 여기를 Gem making 으로 바꿔ㅓ야 함.
            Drop.DropGem(level, transform);
            if(GameManager.instance.isLive) AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }
    // 코루틴만의 반환형 인터페이스
    IEnumerator KnockBack()
    {
        yield return wait;   // 다음 하나의 물리 프레임 쉬기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        // yield return new WaitForSeconds(2f);   2초 쉬기
    }

    void Dead(){
        gameObject.SetActive(false);
    }

}
