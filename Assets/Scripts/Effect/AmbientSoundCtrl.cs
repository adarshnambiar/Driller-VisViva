using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundCtrl : MonoBehaviour {

	public AudioClip[] ambientSound;
	public AudioClip[] oneshotSound;

	AudioSource audioSource;
	int currentIndex = -1;

	void Awake(){
		audioSource = audio;
		currentIndex = -1;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayAmbientLoop(int index){
		if (currentIndex != index && index < ambientSound.Length) {
			audioSource.clip = ambientSound [index];
			audioSource.Play ();
			currentIndex = index;
		} else if (currentIndex == index && !audioSource.isPlaying) {
			audioSource.Play();
		}

	}

	public void StopAmbient(){
		audioSource.Pause ();
		//currentIndex = -1;
	}

	public float PlayOneShot(int index, float volume){
		audioSource.PlayOneShot (oneshotSound [index], volume);
		return oneshotSound [index].length;
	}

}
