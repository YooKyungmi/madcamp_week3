using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//7:45
	public static AudioManager instance;

	[Header("#BGM")]
	public AudioClip bgmClip;
	public float bgmVolume;
	AudioSource bgmPlayer;
	
	[Header("#SFX")]
	public AudioClip[] sfxClips; // 오디오클립들
	public float sfxVolume;  // 볼륨
	public int channels; // 소리 채널들
	AudioSource[] sfxPlayers; // 오디오 클립들을 재생할 플레이어들
	int channelIndex;
    AudioHighPassFilter bgmEffect;
//17:13
    public enum Sfx {Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win}
    // WIn 에 마우스 갖다 댔을때 Win=9나오면 성공

    //22:01
    public void PlaySfx(Sfx sfx){
        for(int index=0; index<sfxPlayers.Length; index++){
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;

            //28:30
            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee){
                ranIndex = Random.Range(0,2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
        
    }
	void Awake()
	{
		instance = this;
		Init();	
	}
	//13:26
	void Init(){
		// 배경음 플레이어 초기화
		GameObject bgmObject = new GameObject("BgmPlayer");
		bgmObject.transform.parent = transform;
		bgmPlayer = bgmObject.AddComponent<AudioSource>();
		bgmPlayer.playOnAwake = false;
		bgmPlayer.loop = true;
		bgmPlayer.volume = bgmVolume;
		bgmPlayer.clip = bgmClip;

		//효과음 플레이어 초기화
		GameObject sfxObject = new GameObject("SfxPlayer");
		sfxObject.transform.parent = transform;
		sfxPlayers = new AudioSource[channels];
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
		for (int index=0; index < sfxPlayers.Length; index++){
			sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
			sfxPlayers[index].playOnAwake = false;
			sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].bypassListenerEffects = true;
		}
	}
    //AudioManager
    public void PlayBgm(bool isPlay){
        if(isPlay){
            bgmPlayer.Play();
        }
        else{
            bgmPlayer.Stop();
        }
    }

    //33:36
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }
}