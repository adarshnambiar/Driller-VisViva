using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroCtrl : MonoBehaviour {

	public RawImage DisplayImage;

	public Texture openningStart;
	public MovieTexture openningVideo;
	public Texture openningEnd;
	public AudioClip introBGM;

	public GameObject startBtn;

	public Animator CharacterPositionAnimator;
	public Animator CharacterAnimator;

	float movieEndTime;
	AudioSource audioSource;


	// Use this for initialization
	void Start () {
		DisplayImage.texture = openningStart;
		audioSource = GetComponent<AudioSource> ();
		openningVideo.Stop ();
		movieEndTime = -1;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Sample")&&startBtn.activeSelf) {
			StartVideo();
		}

		if (DisplayImage.texture == openningVideo && movieEndTime < Time.time) {
			DisplayImage.texture = openningEnd;
			openningVideo.Stop ();
			CharacterPositionAnimator.SetTrigger("JumpOut");
			audioSource.PlayOneShot(introBGM, 0.5f);
		}


		if (Input.GetButtonDown ("NextStage")) {
			Application.LoadLevel("Geologist");
		}

	}

	public void StartVideo(){
		DisplayImage.texture = openningVideo;
		openningVideo.Play();
		movieEndTime = Time.time + openningVideo.duration;
		startBtn.SetActive (false);
	}

	void IntroOver(){
		//Application.LoadLevel("Geologist");
		StartCoroutine (NextLevel ());
	}

	IEnumerator NextLevel(){
		yield return new WaitForSeconds (1);
		Application.LoadLevel ("Geologist");
	}

}
