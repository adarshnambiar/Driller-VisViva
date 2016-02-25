using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundFXCtrl : MonoBehaviour {
	
	public static SoundFXCtrl instance;
	
	public AudioClip[] soundFX;
	public AudioClip[] sequenceFX;
	public float sequenceReset = 1;
	
	private AudioSource audioSource;
	private float nextPlayTime = 0;
	
	private int sequenceIndex = 0;
	private float sequenceResetTime = 0;
	private float nextSequenceTime = 0;
	private int sequenceWaiting = 0;
	
	// Use this for initialization
	void Awake () {
		audioSource = audio;
		sequenceResetTime = sequenceReset;
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy(gameObject);
		}
	}
	
	void Update () {
		if(nextPlayTime>0){
			nextPlayTime -= Time.time;
		}
		
		if(nextSequenceTime > 0){
			nextSequenceTime -= Time.deltaTime;
		}else{
			
			if(sequenceWaiting>0){
				audioSource.PlayOneShot(sequenceFX[sequenceIndex]);
				sequenceIndex++;
				if(sequenceIndex>=sequenceFX.Length){
					sequenceIndex--;
				}
				nextSequenceTime = sequenceFX[sequenceIndex].length<0.3f?sequenceFX[sequenceIndex].length:0.3f;
				sequenceResetTime = sequenceReset;
				sequenceWaiting--;
			}else{
				if(sequenceResetTime > 0 && sequenceIndex != 0){
					sequenceResetTime -= Time.deltaTime;
					if(sequenceResetTime<=0){
						sequenceResetTime = sequenceReset;
						sequenceIndex = 0;
					}
				}
			}
		}
	}
	
	public float PlaySound(int index, float volumn){
		audioSource.PlayOneShot (soundFX[index], volumn);
		return soundFX [index].length;
	}

	public float PlaySound(AudioClip clip, float volumn){
		audioSource.PlayOneShot (clip, volumn);
		return clip.length;
	}
	
	public float PlaySoundSingleton(int index, float volumn){
		if(nextPlayTime > 0){
			return -1;
		}else{
			audioSource.PlayOneShot (soundFX[index], volumn);
			nextPlayTime = soundFX[index].length;
			return nextPlayTime;
		}
	}
	
	public void PlaySoundSequence(){
		sequenceWaiting++;
	}
	
}
