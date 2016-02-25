using UnityEngine;
using System.Collections;

public enum GeoStage{
	Prepare,
	Intro,
	Tutorial,
	InGame,
	GameOver,
}

public class GeoGameManager : MonoBehaviour {

	public static GeoGameManager instance;

	[Header("Scene Prefab Setup")]
	/*****Boundaries, used for determind the horizontal limitation of bounce back wave*******/
	public Transform LeftBound;					//transform of left map boundary
	public Transform RightBound;				//transform of right map boundary
	public GameObject DetectWave;				//prefab for detection wave
	public Transform DetectWaveSendPos;			//position of sending detect wave
	public GameObject RespondWave;				//prefab for responding wave
	public GameObject FlashEffect;				//prefab for layer flash effect
	public GameObject Player;					//prefab of player
	public GameObject TargetMarker;				//prefab for marking wave target position

	[Header("Level Parameters")]
	public float totalTime = 180;				//the total amount of time for this gameplay		
	public AudioClip[] audioForLayers;			//audio pool for different layers


	[Header("UI Game Objects")]
	//public GameObject LoadingScreen;
	public GameObject TimeCount;
	public GameObject ObjectiveWindow;

	Transform waves;			//Gameobject to put all waves
	float horRespownPos;		//the horizontal limit area for respond waves
	
	float remainTime = 0;		//the remain time of this level
	GeoStage gameStage;		//current game stage

	Hashtable LayerAudioTable;
	int revealedLayerCount;
	int totalLayerCount;
	bool arduinoSend = false;

	void Awake(){
		instance = this;

		horRespownPos = RightBound.position.x - LeftBound.position.x;
		waves = new GameObject ("waves").transform;

		gameStage = GeoStage.Prepare;
		remainTime = totalTime;

		Player.SetActive (false);
		revealedLayerCount = 0;
		//LoadingScreen.SetActive (true);
	}

	// Use this for initialization
	void Start () {
		LayerAudioTable = new Hashtable ();
		//randomize the audios
		for (int i = 0; i<audioForLayers.Length; i++) {
			int r = Random.Range(0, audioForLayers.Length);
			AudioClip temp = audioForLayers[i];
			audioForLayers[i] = audioForLayers[r];
			audioForLayers[r] = temp;
		}
		LayerAudioTable.Add (LayerType.HardLayer, audioForLayers[0]);
		LayerAudioTable.Add (LayerType.SoftLayer, audioForLayers[1]);
		LayerAudioTable.Add (LayerType.Oil, audioForLayers[2]);

		LayerAudioTable.Add (LayerType.Limestone, audioForLayers[0]);
		LayerAudioTable.Add (LayerType.Sandstone, audioForLayers[1]);
		LayerAudioTable.Add (LayerType.Conglomerate, audioForLayers[2]);
		LayerAudioTable.Add (LayerType.Shale, audioForLayers[0]);
		LayerAudioTable.Add (LayerType.Siltstone, audioForLayers[1]);
		LayerAudioTable.Add (LayerType.Clay, audioForLayers[2]);

		TimeCount.SetActive (false);
		ObjectiveWindow.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetButtonDown ("SendWave")) {
			GeologistUICtrl.instance.DisplayText("hello:" + Time.time, 1);
		}
		*/

		switch (gameStage) {
		case GeoStage.Intro:
			if(Input.GetButtonDown("NextStage")){
				GeologistUICtrl.instance.StopIntro();
			}
			break;
		case GeoStage.Tutorial:

			switch(TutorialControl.instance.GetCurrentIndex()){
			case 0:				//rotate dial to change direction
				float crossHairMovement = Input.GetAxis("Horizontal_Stage1");
				if(Mathf.Abs(crossHairMovement)>0.3f){
					TutorialControl.instance.DisplayNext();
					WaveGenerator.instance.canSendWave = true;
					if(!arduinoSend){
						SendData.SendToArduinoTutorial('a');
						arduinoSend = true;
					}
				}
				break;
			case 1:
				if(Input.GetButtonDown ("SendWave") && WaveGenerator.instance.GetCurrentPower()>0.05f){
					TutorialControl.instance.DisplayNext();
				}
				break;
				/*
			case 1:				//yell into mic - need to loud enough to go across half of the depth
				if(WaveGenerator.instance.GetUnscaledSoundPower()>0.3f){
					TutorialControl.instance.DisplayNext();
				}
				break;
			case 2:				//send wave while yelling
				if(Input.GetButtonDown ("SendWave") && WaveGenerator.instance.GetCurrentPower()>0.05f){
					TutorialControl.instance.DisplayNext();
				}
				break;
				*/
			default:
				if(Input.GetButtonDown ("SendWave")){
					if(TutorialControl.instance.DisplayNext()){
						StartGame();
						TutorialControl.instance.TotalFadeOut();
					}
				}
				break;
			}

			if(Input.GetButtonDown ("NextStage")){
				StartGame();
				TutorialControl.instance.TotalFadeOut();
				if(!arduinoSend){
					SendData.SendToArduinoTutorial('a');
					arduinoSend = true;
				}
			}
			break;
		case GeoStage.InGame:
			//count down
			if(remainTime>0){
				remainTime -= Time.deltaTime;
				if(remainTime<=0){
					Application.LoadLevel("Driller");
				}
			}
			break;
		}

		/*
		if (Input.GetButtonDown ("NextStage")) {
			Application.LoadLevel("Driller");
		}
		*/

		if (Input.GetKeyDown (KeyCode.F1)) {
			Application.LoadLevel("GameMenu");
		}
		if (Input.GetKeyDown (KeyCode.F2)) {
			Application.LoadLevel("Driller");
		}
		if (Input.GetKeyDown (KeyCode.F3)) {
			Application.LoadLevel("PlanPath");
		}
	}

	public void CreateDetectWave(float targetDepth, Vector3 backDir){
		SoundFXCtrl.instance.PlaySound (0, 1);
		GameObject newwave = Instantiate(DetectWave, DetectWaveSendPos.position, Quaternion.identity) as GameObject;
		newwave.transform.parent = waves;
		newwave.transform.up = backDir;
		newwave.GetComponent<DetectWave>().MaxDistance = targetDepth;

		GameObject targetMarker = Instantiate (TargetMarker, DetectWaveSendPos.position - (backDir * targetDepth), Quaternion.identity) as GameObject;
		targetMarker.transform.up = backDir;
		newwave.GetComponent<DetectWave> ().relatedTargetMarker = targetMarker;
	}

	public void CreateRespondWave(Vector3 position, string layerName){
		//flash effect
		GameObject layer = GameObject.Find ("/LayerMask/"+layerName);
		GameObject effect = Instantiate (FlashEffect, layer.transform.position, Quaternion.identity) as GameObject;
		effect.transform.parent = layer.transform.parent;
		SpriteRenderer effectSprite = effect.GetComponent<SpriteRenderer> ();
		SpriteRenderer layerSprite = layer.GetComponent<SpriteRenderer> ();
		effectSprite.sortingOrder = layerSprite.sortingOrder + 1;
		effectSprite.sprite = layerSprite.sprite;

		SoundFXCtrl.instance.PlaySound ((AudioClip)LayerAudioTable[DataManager.instance.GetLayerProperty(layerName).type], 1);
		//create respond wave
		Vector3 randomReceivePos = LeftBound.position;
		randomReceivePos.x += Random.Range (0f, horRespownPos);
		GameObject respondWave = Instantiate (RespondWave, position, Quaternion.identity) as GameObject;
		RespondWave respondScript = respondWave.GetComponent<RespondWave> ();
		respondScript.layername = layerName;
		respondScript.wavecolor = DataManager.instance.GetColorOfLayerName (layerName);
		respondWave.transform.up = randomReceivePos-position;
		respondWave.transform.parent = waves;
	}

	public void CreateRespondWaveInDir(Vector3 position, string layerName, Vector3 dir){
		//flash effect
		GameObject layer = GameObject.Find ("/LayerMask/"+layerName);
		GameObject effect = Instantiate (FlashEffect, layer.transform.position, Quaternion.identity) as GameObject;
		effect.transform.parent = layer.transform.parent;
		SpriteRenderer effectSprite = effect.GetComponent<SpriteRenderer> ();
		SpriteRenderer layerSprite = layer.GetComponent<SpriteRenderer> ();
		effectSprite.sortingOrder = layerSprite.sortingOrder + 1;
		effectSprite.sprite = layerSprite.sprite;
		
		SoundFXCtrl.instance.PlaySound ((AudioClip)LayerAudioTable[DataManager.instance.GetLayerProperty(layerName).type], 1);
		//create respond wave
		GameObject respondWave = Instantiate (RespondWave, position, Quaternion.identity) as GameObject;
		RespondWave respondScript = respondWave.GetComponent<RespondWave> ();
		respondScript.layername = layerName;
		//respondScript.wavecolor = DataManager.instance.GetColorOfLayerName (layerName);
		//respondWave.transform.up = -dir;
		respondScript.wavecolor = Color.white;
		respondWave.transform.rotation = Quaternion.LookRotation (Vector3.forward, dir);
		respondWave.transform.parent = waves;
	}

	public void RespondCatch(string layername){
		//SoundFXCtrl.instance.PlaySound (2, 1);

		LayerColorMask.instance.ChangeLayerColor (layername, DataManager.instance.GetRevealDefaultColor());
		//revealedLayers.Add (layername);
		if (DataManager.instance.AddRevealedLayer (layername)) {
			revealedLayerCount++;
			if(revealedLayerCount >= totalLayerCount){
				Application.LoadLevel("Driller");
			}
		}
		/*
		if (revealedLayerCount >= LayerColorMask.instance.GetLayerCount ()) {
			Application.LoadLevel("Driller");
		}
		*/
		/*
			SpriteRenderer spRender = layer.GetComponent<SpriteRenderer>();
			Color col = spRender.color;
			col.a = Mathf.MoveTowards(col.a, 0, 0.5f);
			spRender.color = col;
		*/

	}
	/*
	public void CloseLoadingScreen(){
		LoadingScreen.GetComponent<UIFadeInOut> ().FadeOut ();
	}
	*/
	public void StartIntro(){
		BGMCtrl.instance.PlayBGM ();
		gameStage = GeoStage.Intro;
		GeologistUICtrl.instance.StartIntro ();

		totalLayerCount = LayerColorMask.instance.GetLayerCount ();
	}
	//called from GeologistUICtrl script
	public void IntroOver(){
		StartTutorial ();
	}

	public void StartTutorial(){
		Player.SetActive (true);
		gameStage = GeoStage.Tutorial;
		TutorialControl.instance.gameObject.SetActive (true);
		//MicroPhoneInput.instance.StartRecording ();	//enable microphone after game start
	}

	//function for starting the game, do prepare and instantiate
	public void StartGame(){

		gameStage = GeoStage.InGame;
		ObjectiveWindow.SetActive (true);
		TimeCount.SetActive (true);
		WaveGenerator.instance.canSendWave = true;
	}

	public GeoStage GetCurrentGameStage(){
		return gameStage;
	}

	public float GetRemainingTime(){
		return remainTime;
	}

}
