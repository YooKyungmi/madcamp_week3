using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public enum TypeId { Exp=0, Potion=1, Magnet=2, Bomb=3 }
    TypeId typeId;
    Rigidbody2D rigid;
    int level ;
    bool touched;

    public static void DropBox(TypeId typeId, Transform trans){
        if (typeId == TypeId.Exp)return;
        Drop drop = GameManager.instance.pool.Get(1 + (int) typeId ).GetComponent<Drop>();
        drop.transform.position = trans.position;
        drop.typeId = typeId;
    }
    public static void DropGem(int level, Transform trans){
        Drop drop = GameManager.instance.pool.Get(1).GetComponent<Drop>();
        drop.setLevel(level);
        drop.transform.position = trans.position;
    }

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        touched = false;
    }

    void Init(TypeId tid){
        typeId = tid;
    }

    void setLevel(int level){this.level = level;}

    void OnTriggerStay2D(Collider2D collider) {  //Magnet 범위 안에 들어오면 끌려감 
        if (!collider.CompareTag("Magnet") || touched) return;
        touched = true;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(!collider.CompareTag("PlayerDrop")) return;

        switch(typeId){
            case TypeId.Exp:
                GameManager.instance.GetExp(getExp());
                break;
            case TypeId.Potion:
                GameManager.instance.hpUp(0.3f, true);
                break;
            case TypeId.Bomb:
                GameManager.instance.bombUp();
                break;
            case TypeId.Magnet:
                StartCoroutine(GameManager.instance.magnetRoutine());
                break;
            default: break;    
        }
    }

    void FixedUpdate(){ // touched true 일 때 플레이어로 인력 추가
        if(!touched) return;
        
        Rigidbody2D player = GameManager.instance.player.rigid;
        float gravityScale = 1.0f;

        Vector2 gravityDirection = (transform.position - player.transform.position).normalized;
        Vector2 gravityForce = gravityDirection * gravityScale;
        rigid.AddForce(gravityForce);
    }
    int getExp(){
        int weight = 3;
        int bias = 10;
        return Mathf.FloorToInt(Mathf.Pow(level, 2)) * weight + bias;
    }
}
