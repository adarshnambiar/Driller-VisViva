using UnityEngine;
using System.Collections;

public class SkylineCtrl : MonoBehaviour {

	public Transform WindowLightContainer;
	public Transform StarLightContainer;
	public GameObject CityOutline;
	public UIFadeInOut BlackMask;
	public float LightDelay = 0.5f;
	public SkylineBattery batteryCtrl;
	public ScoreEffect scoreEffectCtrl;
	public Animator GeologistAnim;
	public AudioClip[] endingBGM;	//0-bad, 1-soso, 2-good
	public SendData sendInterface;

	ArrayList LightObjects;
	float scoreCostPerLight;

	// Use this for initialization
	void Start () {
		int windowLightNum = WindowLightContainer.childCount;
		int starLightNum = StarLightContainer.childCount;
		LightObjects = new ArrayList ();
		foreach (Transform child in WindowLightContainer) {
			LightObjects.Add(child.gameObject);
			child.gameObject.SetActive(false);
		}
		StarLightContainer.gameObject.SetActive (false);
		CityOutline.SetActive (false);
		/*
		foreach (Transform child in StarLightContainer) {
			LightObjects.Add(child.gameObject);
			child.gameObject.SetActive(false);
		}
		*/
		/*
		//randomize the order
		for(int i = 0; i < windowLightNum; i++){
			int randSwap = Random.Range(0, windowLightNum);
			object temp = LightObjects[randSwap];
			LightObjects[randSwap] = LightObjects[i];
			LightObjects[i] = temp;
		}
		*/
		/*
		for (int i = windowLightNum; i < windowLightNum + starLightNum; i++) {
			int randSwap = Random.Range(windowLightNum, windowLightNum + starLightNum);
			object temp = LightObjects[randSwap];
			LightObjects[randSwap] = LightObjects[i];
			LightObjects[i] = temp;
		}
		*/
		scoreCostPerLight = DataManager.instance.GetTotalScore () / LightObjects.Count;

		scoreEffectCtrl.gameObject.SetActive (false);
		BGMCtrl.instance.FadeOut ();
		BlackMask.FadeOut ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("NextStage")) {
			Destroy(DataManager.instance.gameObject);
			Destroy(SoundFXCtrl.instance.gameObject);
			Application.LoadLevel("GameMenu");
		}
	}

	void OnApplicationQuit() {
		SendData.ClearLEDStrip ();
		SendData.ClearLEDButtons ();
	}
	
	void OnDestroy(){
		SendData.ClearLEDStrip ();
		SendData.ClearLEDButtons ();
	}

	void FadeOutFinish(GameObject sender){
		switch (sender.name) {
		case "BlackMask":
			StartCoroutine(TurnOnLight());
			break;
		}
	}

	IEnumerator TurnOnLight(){
		//precentage of score
		float scoreFactor = Mathf.Clamp01(DataManager.instance.GetCurrentScore () / DataManager.instance.GetTotalScore ());

		batteryCtrl.BatteryJumpOut ();
		SoundFXCtrl.instance.PlaySound (endingBGM [3], 1);
		yield return new WaitForSeconds (0.7f);		//wait for battery jump out animation

		float scoreTime = scoreEffectCtrl.DisplScore () * 0.75f + 1;
		float soundTime = SoundFXCtrl.instance.PlaySound (endingBGM [4], 1);

		yield return new WaitForSeconds (Mathf.Max(scoreTime, soundTime));	//wait for bouncing score display
		if (scoreFactor < 0.3f) {
			SoundFXCtrl.instance.PlaySound (endingBGM [0], 1);
		} else if (scoreFactor < 0.7f) {
			SoundFXCtrl.instance.PlaySound (endingBGM [1], 1);
		} else {
			SoundFXCtrl.instance.PlaySound (endingBGM [2], 1);
		}

		/*Light up city*/
		yield return new WaitForSeconds (LightDelay*2);
		CityOutline.SetActive (true);
		yield return new WaitForSeconds (LightDelay);

		SendData.SendToArduino ((int)(scoreFactor * 9));

		float playerScore = DataManager.instance.GetCurrentScore ();
		int i = 0;
		while(playerScore>0 && i<LightObjects.Count){
			((GameObject)LightObjects[i]).SetActive(true);
			playerScore -= scoreCostPerLight;
			i++;
			yield return new WaitForSeconds(LightDelay);
		}
		StarLightContainer.gameObject.SetActive (true);
		foreach (Transform child in StarLightContainer) {
			StarBlink sb = child.gameObject.AddComponent<StarBlink>();
			sb.BlinkDelay = Random.Range(3f, 6f);
		}

		yield return new WaitForSeconds (1);
		GeologistAnim.SetTrigger("jumpin");
		yield return new WaitForSeconds (1);

		if (scoreFactor < 0.3f) {
			GeologistUICtrl.instance.PlayVoice (2);
			GeologistUICtrl.instance.DisplayDialog (2);
		} else if (scoreFactor < 0.7f) {
			GeologistUICtrl.instance.PlayVoice (1);
			GeologistUICtrl.instance.DisplayDialog (1);
		} else {
			GeologistUICtrl.instance.PlayVoice (0);
			GeologistUICtrl.instance.DisplayDialog (0);
		}

	}

}
