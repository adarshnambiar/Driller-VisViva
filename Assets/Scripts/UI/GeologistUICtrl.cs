using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class DialogMsg{
	public string text;
	public float length;
}

public class GeologistUICtrl : MonoBehaviour {

	public static GeologistUICtrl instance;
	[Header("Intro Resources")]
	public AudioClip IntroVoice;
	public DialogMsg[] Introdialogs;
	public GameObject CallBackGO;
	[Header("In Game Resources")]
	public AudioClip[] voice;
	public DialogMsg[] dialogs;
	[Header("Setup")]
	public Animator GeologistAnimator;
	public CanvasGroup DialogCanvas;
	public Text DialogText;
	public float DialogTransitSpeed = 2;

	Queue dialogQueue;
	AudioSource audioSource;
	float textSwitchTime;
	int dialogStatus;						//0 - hide, 1 - fade in, 2 - fade out, 3 - show
	float speakTime;						//the time before last voice is over
	bool forceSpeaking;						//force the character in speaking state

	void Awake(){
		instance = this;
		audioSource = GetComponent<AudioSource> ();
		dialogQueue = new Queue ();
		DialogCanvas.alpha = 0;
		textSwitchTime = 0;
		dialogStatus = 0;
		speakTime = 0;
		forceSpeaking = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		switch (dialogStatus) {
		case 0:
			if(dialogQueue.Count > 0){
				DialogMsg message = dialogQueue.Dequeue() as DialogMsg;
				textSwitchTime = message.length;
				DialogText.text = message.text;
				dialogStatus = 1;
			}
			break;
		case 1:
			DialogCanvas.alpha = Mathf.MoveTowards(DialogCanvas.alpha, 1, DialogTransitSpeed * Time.deltaTime);
			if(DialogCanvas.alpha == 1){
				dialogStatus = 3;
			}
			break;
		case 2:
			DialogCanvas.alpha = Mathf.MoveTowards(DialogCanvas.alpha, 0, DialogTransitSpeed * Time.deltaTime);
			if(DialogCanvas.alpha == 0){
				dialogStatus = 0;
			}
			break;
		case 3:
			textSwitchTime -= Time.deltaTime;
			if(textSwitchTime < 0){
				dialogStatus = 2;
			}
			break;
		}

		if (speakTime > Time.time || dialogStatus == 3||forceSpeaking) {
			GeologistAnimator.SetInteger ("state", 1);
		} else {
			GeologistAnimator.SetInteger ("state", 0);
		}

	}

	//play a voice clip, return the length of the clip
	public float PlayVoice(int index){
		/*
		if (index < voice.Length && audioSource.clip!=voice[index]) {
			audioSource.clip = voice[index];
			audioSource.Play();
			//audioSource.PlayOneShot (voice [index], 1);
			speakTime = Time.time + voice[index].length;
			return voice[index].length;
		}else
			return 0;
			*/
		audioSource.clip = voice[index];
		audioSource.Play();
		//audioSource.PlayOneShot (voice [index], 1);
		speakTime = Time.time + voice[index].length;
		return voice[index].length;
	}

	//display a text in bubble
	public void DisplayDialog(string text, float length){
		DialogMsg message = new DialogMsg ();
		message.text = text;
		message.length = length;
		dialogQueue.Enqueue (message);
	}

	public float DisplayDialogNow(int index){
		dialogQueue.Clear ();
		if (dialogStatus != 0) {
			dialogStatus = 2;
		}
		DialogMsg message = new DialogMsg ();
		message.text = dialogs [index].text;
		message.length = dialogs [index].length;
		dialogQueue.Enqueue (message);
		return message.length;
	}

	//display the dialog indicated by index
	public void DisplayDialog(int index){
		dialogQueue.Enqueue (dialogs [index]);
	}

	public void StartIntro(){
		audioSource.clip = IntroVoice;
		audioSource.Play ();
		speakTime = Time.time + IntroVoice.length;
		foreach (DialogMsg message in Introdialogs) {
			dialogQueue.Enqueue(message);
		}
		StartCoroutine (IntroEnd(IntroVoice.length));
	}

	IEnumerator IntroEnd(float introLength){
		yield return new WaitForSeconds (introLength);
		StopIntro ();
	}

	public void StopIntro(){
		StopAllCoroutines ();
		dialogStatus = 2;
		audioSource.Stop ();
		speakTime = Time.time;
		dialogQueue.Clear ();
		CallBackGO.SendMessage("IntroOver");
	}

	public void ForceSpeak(){
		forceSpeaking = true;
	}

	public void StopSpeaking(){
		forceSpeaking = false;
	}



}
