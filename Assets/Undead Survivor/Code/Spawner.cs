using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{    
    public Transform[] spawnPoint;
    float levelTime;
    int level;
    float timer;
    float explodeTimer;
    float newEnemyTimer;

    void Awake() {
        spawnPoint = GetComponentsInChildren<Transform>();
        //levelTime = GameManager.instance.maxGameTime / spawnData.Length;
        levelTime = 30f;
        timer = 0f;
        explodeTimer = 0f;
        newEnemyTimer = 0f;
    }
    
    void Update()
    {
        if(!GameManager.instance.isLive)
            return;
        timer += Time.deltaTime;
        //level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);
        
        if (timer > getSpawnTime()){
            timer = 0;
            Spawn();
        }

        // Explode time
        explodeTimer += Time.deltaTime;
        
        if (explodeTimer > 60f){
            explodeTimer = 0;
            SpawnExplosion((int)GameManager.instance.gameTime / 60);
        }

        // newEnemy Time
        newEnemyTimer += Time.deltaTime;
        
        if (newEnemyTimer > 30f){
            newEnemyTimer = 0;
            GameManager.instance.addEnemyList();
            Box box = GameManager.instance.pool.Get(4).GetComponent<Box>();
            box.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
        }


    }

    void Spawn(){
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;  //자식 오브젝트만 포함하기 위해 1부터 시작, 0은 자기 자신
        Debug.Log(GameManager.instance.getRandomEnemyId());
        enemy.GetComponent<Enemy>().Init(GameManager.instance.spawnData[GameManager.instance.getRandomEnemyId()], getLevel());
    }
    void SpawnExplosion(int enemyPerSpawn){
        foreach( Transform sp in spawnPoint[1..]){
            for(int i = 0 ; i < enemyPerSpawn ; i++){
                GameObject enemy = GameManager.instance.pool.Get(0);
                enemy.transform.position = sp.position;  
                enemy.GetComponent<Enemy>().Init(GameManager.instance.spawnData[GameManager.instance.getRandomEnemyId()], getLevel());
            }
        }
    }


    public int getLevel(){
        level = Mathf.FloorToInt(GameManager.instance.gameTime / levelTime);
        return level;
    }

    public float getSpawnTime(){
        return 1f / (level+1);
    }

}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed; 
}
