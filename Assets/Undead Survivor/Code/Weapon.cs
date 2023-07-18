using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    float timer;
    Player player;

    private void Awake() {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if(!GameManager.instance.isLive)
            return;
        switch (id){
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed){
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage=damage* Character.Damage;
        this.count += count;

        if (id==0)
            Batch();
        
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon" +data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++){
            // 스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
            if (data.projectile == GameManager.instance.pool.prefabs[index]){
                prefabId = index;
                break;
            }
        }

        switch (id){
            case 0:
                speed = -150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }


        // Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int index=0; index <count; index++){
            Transform bullet;

            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            if (index < transform.childCount){
                bullet = transform.GetChild(index);
            }
            else{
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward *360 *index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // 이동방향은 Space.World 기준으로
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 이 근접

        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
            // 방향 결정
            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;
        
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;  //위치 결정
        bullet.rotation = Quaternion.FromToRotation(Vector3.up,dir);  //회전 결정
        bullet.GetComponent<Bullet>().Init(damage,count,dir);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
