using UnityEngine;
using System.Collections;

public class BGMCtrl : MonoBehaviour {

	public static BGMCtrl instance;

	public AudioClip[] audios;

	AudioSource audioSource;
	Animator audAnimator;

	// Use this for initialization
	void Awake () {
		audioSource = audio;
		instance = this;
		audAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayBGM(){
		audioSource.Play ();
	}

	public void FadeOut(){
		audAnimator.SetTrigger ("fadeout");
	}

	public float ChangeBGM(int index){
		StartCoroutine (ChangeBGMFade (index));
		return audios [index].length+0.5f;
	}

	IEnumerator ChangeBGMFade(int index){
		audAnimator.SetTrigger("fadeout");
		yield return new WaitForSeconds (0.5f);
		audioSource.clip = audios [index];
		audioSource.Play ();
		audAnimator.SetTrigger("fadein");
	}

	public float ChangeBGMWithoutFade(int index){
		audioSource.clip = audios [index];
		audioSource.Play ();
		return audios [index].length;
	}

}
