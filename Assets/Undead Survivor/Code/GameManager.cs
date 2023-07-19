using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("# Play Info")]
    public float health;
    public float maxHealth=100;
    public int level;
    public int kill;
    public int exp;
    //public int[] nextExp = {3,5,10,30,60,100,150,210,280,360,450,600};
    [Header("# Game Object")]
    public Player player;
    public LevelUp uiLevelUp;
    public PoolManager pool;
    public Result uiResult;
    public GameObject enemyCleaner;
    public int playerId;
    public int bomb;
    WaitForSecondsRealtime waitDrop;
    WaitForSecondsRealtime waitCooltime;
    bool bombIsCool;
    public GameObject magnet;  //이거ㅓ 나중에 배정 해줘야 함
    public GameObject bombButton;
    public GameObject bombButtonLock;
    public List<int> enemyList;


    void Awake() {
        instance = this;
        isLive = false;
        waitDrop = new WaitForSecondsRealtime(0.1f);
        waitCooltime = new WaitForSecondsRealtime(5f);
        bombIsCool = false;
        enemyList = new List<int>();
    }
    public void GameStart(int id) {
        health = maxHealth;
        playerId = id;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);  // 임시 스크립트 (첫번째 캐릭터 선택)
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBgm(true);
        bomb = 0;
        setBombButton(false);
        Resume();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator GameOverRoutine()
    {
        isLive=false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
        AudioManager.instance.PlayBgm(false);
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive=false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        Stop();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
        AudioManager.instance.PlayBgm(false);
    }


    void FixedUpdate()
    {
        if(!isLive)
            return;

        gameTime += Time.deltaTime;

        // if (gameTime > maxGameTime){
        //     gameTime = maxGameTime;
        //     //GameVictory(); // 여기도 바꿔야 함
        // }
    }

    public void GetExp(int addExp){
        if(!isLive)
            return;
        exp += addExp;

        if (exp >= getNextExp()){
            level++;
            exp=0;
            uiLevelUp.Show();
        }
    }
    public void GetExp()
    {
        GetExp(1);
    }


    public void Stop()
    {
        isLive=false;
        Time.timeScale = 0; // 시간 멈추기
    }

    public void Resume()
    {
        isLive=true;
        Time.timeScale = 1;
    }

    public int getNextExp(){
        int BASIC_EXP = 10;
        return  BASIC_EXP * Mathf.FloorToInt(Mathf.Pow(1.5f, level)); //1.5^level만큼 경험치 필요
    }

    public void hpUp(float hp, bool IsPercent){
        if (IsPercent){//이경우엔 비율이 hp로 옴 ( 0 ~ 1 )
            health += hp * maxHealth;
        }
        else{ 
            health += hp;
        }
        
        health = Mathf.Max(health, maxHealth);
    }

    public void maxHpUp(float hp, bool IsPercent){  // max hp 가 증가하면 그 증가분 만큼 현재 체력 또한 증가함.
        if (IsPercent){//이경우엔 비율이 hp로 옴 ( 0 ~ 1 )
            health += hp * maxHealth;
            maxHealth *= hp;
        }
        else{ 
            health += hp;
            maxHealth += hp;
        }
        
        health = Mathf.Max(health, maxHealth);
    }
    public void bombUp(){
        bomb ++;
        //bomb button 활서ㅓㅇ화 하는 코드
        if(!bombIsCool){
            setBombButton(true);
            //activate bomb button
        }
    }

    public IEnumerator magnetRoutine(){
		magnet.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);  // 자석 소리 넣어도 됨
		yield return waitDrop;
		magnet.SetActive(false);
	}

    public IEnumerator bombRoutine(){
		enemyCleaner.SetActive(true);
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); //여기 폭탄 소리 넣어ㅓ도 됨
		yield return waitDrop;
		enemyCleaner.SetActive(false);
	}

        public IEnumerator bombCooltime(){
		setBombButton(false); // Bomb button 비활성화 및 lockbomb 활성화
        bombIsCool = true;
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); //여기 폭탄 소리 넣어ㅓ도 됨
		yield return waitCooltime;
		setBombButton(bomb > 0); // Bomb button 활성화 및 lockbomb 비활성화
        bombIsCool = false;
    }

    public void BombClick() {
        if(bomb==0 || bombIsCool)return;
        bomb--;
        StartCoroutine(bombRoutine());
        StartCoroutine(bombCooltime());
    }
    public void setBombButton(bool activate){
        bombButton.SetActive(activate);
        bombButtonLock.SetActive(!activate);
    }

    public int getRandomEnemyId(){
        int randomIndex = UnityEngine.Random.Range(0, enemyList.Count);
        int randomValue = enemyList[randomIndex];
        return randomValue;
    }

    public void addEnemyList(){
        int MAX_ENEMY_VARIATION = 4;
        if (enemyList.Count >= MAX_ENEMY_VARIATION)enemyList.Clear();
        while(true){
            int rand = Random.Range(0, Spawner.spawnData.Length);
            if (enemyList.Contains(rand))continue;
            enemyList.Add(rand);
            return;
        }
    }

}
