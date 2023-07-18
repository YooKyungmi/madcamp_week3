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
    public int[] nextExp = {3,5,10,30,60,100,150,210,280,360,450,600};
    [Header("# Game Object")]
    public Player player;
    public LevelUp uiLevelUp;
    public PoolManager pool;
    public Result uiResult;
    public GameObject enemyCleaner;
    public int playerId;


    void Awake() {
        instance = this;
        isLive = false;
    }
    public void GameStart(int id) {
        health = maxHealth;
        playerId = id;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);  // 임시 스크립트 (첫번째 캐릭터 선택)
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.PlayBgm(true);
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


    void Update()
    {
        if(!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime){
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if(!isLive)
            return;
        exp++;

        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)]){
            level++;
            exp=0;
            uiLevelUp.Show();
        }
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
}
