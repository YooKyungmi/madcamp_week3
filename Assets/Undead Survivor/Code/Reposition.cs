using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{    
    Collider2D coll;

    private void Awake() {
        coll = GetComponent<Collider2D>();
    }
//Reposition
//2:29
    void OnTriggerExit2D(Collider2D collision){
        if(!collision.CompareTag("Area")) return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch(transform.tag){
            case "Ground":
                float diffX = playerPos.x - myPos.x;	
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                
                int distance = 60;

                if (diffX>diffY){
                    transform.Translate(Vector3.right * dirX * distance);
                }
                else if (diffX < diffY){
                    transform.Translate(Vector3.up * dirY * distance);
                }
                else{
                    transform.Translate(Vector3.up * dirY * distance);
                    transform.Translate(Vector3.right * dirX * distance);
                }

                GameObject box = GameManager.instance.pool.Get(5);
                Vector3 ran = new Vector3(Random.Range(-10,10), Random.Range(-10,10), 0);
                box.transform.position = ran + transform.position; 

                break;
            // 4:11
            case "Enemy":
                if(coll.enabled){
                    Vector3 dist = playerPos - myPos;
                    ran = new Vector3(Random.Range(-3,3), Random.Range(-3,3), 0);
                    transform.Translate(dist * 2 + ran);
                }
                break;
            case "Drop":
                gameObject.SetActive(false);  //일정 범위 나가면 사라지도록 
                break;

        }
    }

}
