using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PlanStage{
	Loading,			//an empty stage, do nothing
	Intro,
	Tutorial,
	Planning,
	BeforeDrill,
	Drilling,
	DrillingPause,
	GameOver,
}

public class PlanPathManager : MonoBehaviour {
	
	public static PlanPathManager instance;
	
	[Header("Boundaries")]
	public Transform leftBoundary;
	public Transform rightBoundary;
	public Transform bottomBoundary;
	[Header("Drawing")]
	public Transform OilRig;			//root for drill machine and drawpointer
	public Transform drawPointer;
	public float horiPlanSpeed = 7;		//speed for horizontal planning
	public float speed = 2;				//drawing speed
	public float turnspeed = 3;			//draw turning speed
	public LineRenderer trailPath;
	public float DetectDelay = 0.01f;
	public int MarkerNum = 5;
	public GameObject marker;
	[Header("Drilling")]
	public GameObject Battery;
	public Transform Drill;
	public GameObject HPBar;
	public LineRenderer drillTrail;
	public DrillMachine drillMachine;
	
	[Header("Sound")]
	public AmbientSoundCtrl ambSound;
	/*
	public AudioSource ambientSound;
	public AudioClip standby;
	public AudioClip drilling;
	public AudioClip drillstop;
	public AudioClip drillbroken;*/
	
	[Header("Level Settings")]
	public float PlanningTime = 120;	//the total amount of time for planning path
	public GameObject CoreSamplePoints;
	
	[Header("UI")]
	public OptionBoxCtrl optionBox;
	public InformBoxCtrl informBox;
	public GameObject HintBox;			//hint box game object
	public string[] hints;				//hint sentances to be shown
	public UIFadeInOut BlackFadeInOut;	//black transition effect
	//public Transform BatteryLevel;	//the battery fullfillment for indicating scores
	public GameObject GameOverUI;		//the game over ui
	//public Text gameoverText;			//the game over title text
	//public Text scoreText;			//the score text
	public UIFadeInOut EndingFadeBlack;	//the fade in at the ending part
	public GameObject TimeUI;			//the remainning time ui
	public Text markerCountDisp;		//the text for displaying marker left
	public GameObject DrillUI;			//the UI show in drilling stage
	public Text scoreDisp;				//the UI showing the percentage of energy collected
	//public Text hpDisp;
	
	PlanStage planStage;
	List<Vector3> linePoints;			//point list of planed path
	List<Vector3> drillTrailPoints;			//point list of trail
	
	float pickPointDelay;				//the time delay of taking next position
	float groundlevel;					//the ground level - to prevend drawing above ground
	float bottomlevel;					//the bottom level - to prevend going out of scene
	Vector3 horizontalLeftBoundary;		//the left boundary when choosing horizontal location
	Vector3 horizontalRightBoundary;	//the right boundary when choosing horizontal location
	float minx;							//the minimum value in x axis
	float maxx;							//the maximum value in x axis
	
	int curMoveTarget;					//the target for current movement
	float remainTime;
	
	bool inviewLog;
	bool drawButtonPressed;
	bool eraseButtonPressed;
	
	float offset;
	float lineLength;
	Material lineMat;
	
	GameObject pencil;					//pencil GO attached to draw pointer
	Animator pencAnim;					//animator attached to pencil
	
	Transform drillObject;				//the actual gameobject of drill model, apply rotation to it
	//SpriteRenderer BatteryFillmentSprite;	//used for changine the color of battery fullfillment
	float totalScore;
	//Stage3BatteryCtrl BatteryCtrl;
	BatteryControl batteryCtrl;
	
	int currentHint;					
	
	Quaternion orgDrawRotation;			//record the original rotation of drawing pencil
	Vector3 orgDrawPosition;			//record the original position of drawing pencil
	
	GameObject CurMarkers;
	
	float currentFinal = 0;
	float transitSpeed = 0.1f;
	
	bool sendBtnLight1 = false;
	bool sendBtnLight2 = false;
	
	void Awake(){
		instance = this;
		planStage = PlanStage.Loading;
	}
	
	// Use this for initialization
	void Start () {
		linePoints = new List<Vector3>();
		drillTrailPoints = new List<Vector3>();
		
		pickPointDelay = 0;
		groundlevel = drawPointer.position.y;
		minx = leftBoundary.position.x;
		maxx = rightBoundary.position.x;
		bottomlevel = bottomBoundary.position.y;
		horizontalLeftBoundary = OilRig.position;
		horizontalLeftBoundary.x = leftBoundary.position.x;
		horizontalRightBoundary = OilRig.position;
		horizontalRightBoundary.x = rightBoundary.position.x;
		
		curMoveTarget = 0;
		
		remainTime = PlanningTime;
		
		TimeUI.SetActive (false);
		DrillUI.SetActive (false);
		
		inviewLog = false;
		drawButtonPressed = false;
		eraseButtonPressed = false;
		
		lineLength = 0;
		offset = 0;
		lineMat = trailPath.renderer.material;
		
		drawPointer.gameObject.SetActive (true);
		pencil = drawPointer.GetChild (0).gameObject;
		pencAnim = pencil.GetComponentInChildren<Animator> ();
		
		orgDrawPosition = drawPointer.position;
		orgDrawRotation = drawPointer.rotation;
		
		drillObject = Drill.GetChild (0);
		HPBar.SetActive (false);
		Drill.gameObject.SetActive (false);
		//BatteryFillmentSprite = BatteryLevel.GetComponentInChildren<SpriteRenderer> ();
		scoreDisp.gameObject.SetActive (false);
		//BatteryCtrl = Battery.GetComponent<Stage3BatteryCtrl> ();
		batteryCtrl = Battery.GetComponent<BatteryControl> ();
		Battery.SetActive(false);
		HintBox.SetActive (false);
		
		StartCoroutine (UpdateBGM (BGMCtrl.instance.ChangeBGM (3)));
		
		CurMarkers = new GameObject("Markers");
		
		optionBox.CloseOptionBox ();
		informBox.gameObject.SetActive (false);
		
		currentFinal = 0;
		
		instance = this;
	}
	
	IEnumerator UpdateBGM(float waitTime){
		yield return new WaitForSeconds (waitTime);
		BGMCtrl.instance.ChangeBGMWithoutFade (4);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetButtonDown("Shelf_Right")||Input.GetButtonDown("Shelf_Left")){
			SoundFXCtrl.instance.PlaySound(13, 1);
		}
		
		switch(planStage){
		case PlanStage.Intro:
			if(Input.GetButtonDown("NextStage")){
				GeologistUICtrl.instance.StopIntro();
			}
			break;
		case PlanStage.Tutorial:
			float horiAxisT = Input.GetAxis("Horizontal_Planpath");
			float vertAxisT = Input.GetAxis("Verticle_Planpath");
			
			if(TutorialControl.instance.GetCurrentIndex()!=0){
				horiAxisT = 0;
				vertAxisT = 0;
			}
			
			if(TutorialControl.instance.GetCurrentIndex()!=5){
				pencAnim.SetInteger("drawing", 0);
				if(linePoints.Count == 0){	//choosing horizontal location mode
					if(horiAxisT<0){
						OilRig.position = Vector3.MoveTowards(OilRig.position, horizontalLeftBoundary, horiPlanSpeed * Time.deltaTime * Mathf.Abs(horiAxisT));
					}else if(horiAxisT>0){
						OilRig.position = Vector3.MoveTowards(OilRig.position, horizontalRightBoundary, horiPlanSpeed * Time.deltaTime * horiAxisT);
					}
					if(vertAxisT<-0.2f){
						linePoints.Add(drawPointer.position);
						pickPointDelay = DetectDelay;
					}
				}else{						//free drawing mode
					if(Input.GetButton("Erase_Planpath")&&TutorialControl.instance.GetCurrentIndex()==3){
						drawPointer.collider2D.enabled = true;
						pencAnim.SetInteger("drawing", 2);
						if(linePoints.Count>0){
							Vector3 moveTarget = linePoints[linePoints.Count-1];
							if(drawPointer.position!=moveTarget){
								Vector3 dir = moveTarget - drawPointer.position;
								dir.z = 0;
								drawPointer.rotation = Quaternion.LookRotation(Vector3.forward, dir);
								drawPointer.position = Vector3.MoveTowards(drawPointer.position, moveTarget, 5 * speed * Time.deltaTime);
								ambSound.PlayAmbientLoop(1);
							}else{
								linePoints.RemoveAt(linePoints.Count-1);
								ambSound.StopAmbient();
							}
						}
					}else{
						drawPointer.collider2D.enabled = false;
						pencAnim.SetInteger("drawing", 1);
						Vector3 targetDir = Vector3.zero;
						if(linePoints.Count>2){
							targetDir.x = horiAxisT;
						}
						targetDir.y = vertAxisT;
						targetDir = targetDir.normalized;
						drawPointer.position -= drawPointer.up * speed * Time.deltaTime * Mathf.Clamp01(Vector3.Dot(targetDir, -drawPointer.up));
						if(targetDir!=drawPointer.up){
							Vector3 dir = Vector3.RotateTowards(drawPointer.up, -targetDir, turnspeed*Time.deltaTime, 0);
							dir.z = 0;
							//drawPointer.up = dir;
							drawPointer.rotation = Quaternion.LookRotation(Vector3.forward, dir);
						}
						Vector3 drawPosition = drawPointer.position;
						if(drawPosition.y > groundlevel)
							drawPosition.y = groundlevel;
						if(drawPosition.y < bottomlevel)
							drawPosition.y = bottomlevel;
						if(drawPosition.x < horizontalLeftBoundary.x)
							drawPosition.x = horizontalLeftBoundary.x;
						if(drawPosition.x > horizontalRightBoundary.x)
							drawPosition.x = horizontalRightBoundary.x;
						drawPointer.position = drawPosition;
						
						if(pickPointDelay > 0){
							pickPointDelay -= Time.deltaTime;
						}else{
							pickPointDelay = DetectDelay;
							if(linePoints[linePoints.Count-1] != drawPosition){
								linePoints.Add(drawPosition);
								ambSound.PlayAmbientLoop(0);
							}else{
								ambSound.StopAmbient();
							}
						}
						
						if(Input.GetButtonDown("PlaceMarker_Planpath")&&TutorialControl.instance.GetCurrentIndex()==1){
							if(DataManager.instance.CheckLayerReveal(DataManager.instance.DetectLayerAtPosition(drawPosition))){
								if(MarkerNum>0){
									MarkerNum--;
									GameObject newmarker = Instantiate(marker, drawPosition, Quaternion.identity) as GameObject;
									newmarker.transform.parent = CurMarkers.transform;
									ambSound.PlayOneShot(3, 1);
								}
							}else{
								informBox.DispInform("You can only put markers on places you have revealed", 2);
								SoundFXCtrl.instance.PlaySound(6, 1);
							}
							
						}
						
					}
					if(linePoints.Count>0){
						trailPath.SetVertexCount (linePoints.Count);
						lineLength = 0;
						trailPath.SetPosition(0, linePoints[0]);
						for(int i = 1; i < linePoints.Count; i++)
						{
							lineLength += (linePoints[i]-linePoints[i-1]).magnitude;
							trailPath.SetPosition(i, linePoints[i]);
						}
						lineMat.SetTextureScale("_MainTex",new Vector2(lineLength/0.3f,1));
						offset -= Time.deltaTime*1.5f;
						lineMat.SetTextureOffset("_MainTex", new Vector2(offset,1));
					}
				}
				
				DepthMeterCtrl.instance.SetScaledDepth(1f - (drawPointer.position.y - bottomlevel) / (groundlevel - bottomlevel));
			}
			
			switch(TutorialControl.instance.GetCurrentIndex()){
			case 0:
				if(linePoints.Count>10){
					TutorialControl.instance.DisplayNext();
					if(!sendBtnLight1){
						sendBtnLight1 = true;
						SendData.SendToArduinoTutorial('e');
					}
				}
				break;
			case 1:
				if(Input.GetButtonDown("PlaceMarker_Planpath")){
					TutorialControl.instance.DisplayNext();
				}
				break;
			case 3:
				if(linePoints.Count==0){
					TutorialControl.instance.DisplayNext();
					if(!sendBtnLight2){
						sendBtnLight2 = true;
						SendData.SendToArduinoTutorial('f');
					}
				}
				break;
			case 4:
				if(Input.GetButtonDown("StartDrill_Planpath")){
					TutorialControl.instance.DisplayNext();
				}
				break;
			case 5:
				if(Input.GetButtonDown("Shelf_Right")||Input.GetButtonDown("Shelf_Left")){
					TutorialControl.instance.DisplayNext();
					StartGame();
					TutorialControl.instance.TotalFadeOut();
				}
				break;
			default:
				if(Input.GetButtonDown("StartDrill_Planpath")||Input.GetButtonDown("Erase_Planpath")
				   ||Input.GetButtonDown("PlaceMarker_Planpath")||horiAxisT!=0||vertAxisT!=0){
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
				if(!sendBtnLight1){
					sendBtnLight1 = true;
					SendData.SendToArduinoTutorial('e');
				}
				if(!sendBtnLight2){
					sendBtnLight2 = true;
					SendData.SendToArduinoTutorial('f');
				}
			}
			
			break;
		case PlanStage.Planning:
			float horiAxis = Input.GetAxis("Horizontal_Planpath");
			float vertAxis = Input.GetAxis("Verticle_Planpath");
			pencAnim.SetInteger("drawing", 0);
			if(linePoints.Count == 0){	//choosing horizontal location mode
				if(horiAxis<0){
					OilRig.position = Vector3.MoveTowards(OilRig.position, horizontalLeftBoundary, horiPlanSpeed * Time.deltaTime * Mathf.Abs(horiAxis));
				}else if(horiAxis>0){
					OilRig.position = Vector3.MoveTowards(OilRig.position, horizontalRightBoundary, horiPlanSpeed * Time.deltaTime * horiAxis);
				}
				if(vertAxis<-0.2f){
					linePoints.Add(drawPointer.position);
					pickPointDelay = DetectDelay;
				}
			}else{						//free drawing mode
				if(Input.GetButton("Erase_Planpath")){
					drawPointer.collider2D.enabled = true;
					pencAnim.SetInteger("drawing", 2);
					if(linePoints.Count>0){
						Vector3 moveTarget = linePoints[linePoints.Count-1];
						if(drawPointer.position!=moveTarget){
							Vector3 dir = moveTarget - drawPointer.position;
							dir.z = 0;
							drawPointer.rotation = Quaternion.LookRotation(Vector3.forward, dir);
							drawPointer.position = Vector3.MoveTowards(drawPointer.position, moveTarget, 5 * speed * Time.deltaTime);
							ambSound.PlayAmbientLoop(1);
						}else{
							linePoints.RemoveAt(linePoints.Count-1);
							ambSound.StopAmbient();
						}
					}
				}else{
					drawPointer.collider2D.enabled = false;
					pencAnim.SetInteger("drawing", 1);
					Vector3 targetDir = Vector3.zero;
					if(linePoints.Count>2){
						targetDir.x = horiAxis;
					}
					targetDir.y = vertAxis;
					targetDir = targetDir.normalized;
					drawPointer.position -= drawPointer.up * speed * Time.deltaTime * Mathf.Clamp01(Vector3.Dot(targetDir, -drawPointer.up));
					if(targetDir!=drawPointer.up){
						Vector3 dir = Vector3.RotateTowards(drawPointer.up, -targetDir, turnspeed*Time.deltaTime, 0);
						dir.z = 0;
						//drawPointer.up = dir;
						drawPointer.rotation = Quaternion.LookRotation(Vector3.forward, dir);
					}
					Vector3 drawPosition = drawPointer.position;
					if(drawPosition.y > groundlevel)
						drawPosition.y = groundlevel;
					if(drawPosition.y < bottomlevel)
						drawPosition.y = bottomlevel;
					if(drawPosition.x < horizontalLeftBoundary.x)
						drawPosition.x = horizontalLeftBoundary.x;
					if(drawPosition.x > horizontalRightBoundary.x)
						drawPosition.x = horizontalRightBoundary.x;
					drawPointer.position = drawPosition;
					
					if(pickPointDelay > 0){
						pickPointDelay -= Time.deltaTime;
					}else{
						pickPointDelay = DetectDelay;
						if(linePoints[linePoints.Count-1] != drawPosition){
							linePoints.Add(drawPosition);
							ambSound.PlayAmbientLoop(0);
						}else{
							ambSound.StopAmbient();
						}
					}
					
					if(Input.GetButtonDown("PlaceMarker_Planpath")){
						if(DataManager.instance.CheckLayerReveal(DataManager.instance.DetectLayerAtPosition(drawPosition))){
							if(MarkerNum>0){
								MarkerNum--;
								GameObject newmarker = Instantiate(marker, drawPosition, Quaternion.identity) as GameObject;
								newmarker.transform.parent = CurMarkers.transform;
								ambSound.PlayOneShot(3, 1);
							}
						}else{
							informBox.DispInform("You can only put markers on places you have revealed", 2);
							SoundFXCtrl.instance.PlaySound(6, 1);
						}
					}
					
				}
				if(linePoints.Count>0){
					trailPath.SetVertexCount (linePoints.Count);
					lineLength = 0;
					trailPath.SetPosition(0, linePoints[0]);
					for(int i = 1; i < linePoints.Count; i++)
					{
						lineLength += (linePoints[i]-linePoints[i-1]).magnitude;
						trailPath.SetPosition(i, linePoints[i]);
					}
					lineMat.SetTextureScale("_MainTex",new Vector2(lineLength/0.3f,1));
					offset -= Time.deltaTime*1.5f;
					lineMat.SetTextureOffset("_MainTex", new Vector2(offset,1));
				}
			}
			
			DepthMeterCtrl.instance.SetScaledDepth(1f - (drawPointer.position.y - bottomlevel) / (groundlevel - bottomlevel));
			
			if(Input.GetButtonDown("StartDrill_Planpath")){
				ambSound.StopAmbient();
				if(MarkerNum<5){
					if(MarkerNum==0){
						optionBox.DispOptionBox("Are you sure to start drilling?");
					}else{
						optionBox.DispOptionBox("You have not used all five markers, Are you sure to start drilling?");
					}
					planStage = PlanStage.BeforeDrill;
					ambSound.PlayOneShot(3, 1);
				}else{
					informBox.DispInform("Remember to place markers on places you want to collect resources", 2);
					SoundFXCtrl.instance.PlaySound(6, 1);
				}
			}
			markerCountDisp.text = MarkerNum +" / "+5;
			break;
		case PlanStage.BeforeDrill:
			float holsel = Input.GetAxis("Horizontal_Planpath");
			if(holsel<0){
				if(optionBox.ChangeSelection(0)){
					ambSound.PlayOneShot(3, 1);
				}
			}else if(holsel>0){
				if(optionBox.ChangeSelection(1)){
					ambSound.PlayOneShot(3, 1);
				}
			}
			if(Input.GetButtonDown("StartDrill_Planpath")){
				switch(optionBox.GetCurrentSelection()){
				case 0:
					SoundFXCtrl.instance.PlaySound(11, 1);
					StartDrillingStage();
					break;
				case 1:
					SoundFXCtrl.instance.PlaySound(12, 1);
					planStage = PlanStage.Planning;
					break;
				}
				optionBox.CloseOptionBox();
				//ambSound.PlayOneShot(3, 1);
			}
			break;
		case PlanStage.Drilling:
			
			if(curMoveTarget<linePoints.Count){
				
				Vector3 target = linePoints[curMoveTarget];
				target.z = Drill.position.z;
				if(Drill.position!=target){
					Vector3 dir = Drill.position - target;
					dir.z = 0;
					
					if(dir.magnitude > 0.01f)
						drillObject.rotation = Quaternion.LookRotation(Vector3.forward, dir);
					//Drill.up = dir;
					
					Drill.position = Vector3.MoveTowards(Drill.position, target, drillMachine.GetMoveSpeed() * Time.deltaTime);
				}else{
					curMoveTarget++;
				}
				
				//update trail of drill
				if(pickPointDelay > 0){
					pickPointDelay -= Time.deltaTime;
				}else{
					pickPointDelay = DetectDelay;
					if(drillObject.position.y<groundlevel){
						drillTrailPoints.Add(Drill.position);
						drillTrail.SetVertexCount(drillTrailPoints.Count);
						drillTrail.SetPosition(drillTrailPoints.Count-1, drillObject.position);
					}
				}
			}else{
				GameOver();
			}
			
			
			//hpDisp.text = (int)DrillMachine.instance.GetRemainHP()+"";
			
			float scoreFactor = DataManager.instance.GetCurrentScore()/totalScore;
			//Vector3 batteryLevelScale = BatteryLevel.localScale;
			//batteryLevelScale.x = Mathf.Clamp01(scoreFactor);
			//BatteryLevel.localScale = batteryLevelScale;
			//BatteryFillmentSprite.color = Color.Lerp(Color.red, Color.green, scoreFactor);
			//BatteryCtrl.UpdateCurrentLevel(Mathf.Clamp01(scoreFactor));
			batteryCtrl.UpdateCurrentLevel(Mathf.Clamp01(scoreFactor));
			currentFinal = Mathf.MoveTowards(currentFinal, scoreFactor, transitSpeed);
			int finalScore = (int) (currentFinal * 7000000);
			scoreDisp.text = string.Format("{0:n0}", finalScore)+" kWh";
			//scoreDisp.text = string.Format("{0:0.0}", scoreFactor*100)+"%";
			DepthMeterCtrl.instance.SetScaledDepth(1f - (Drill.position.y - bottomlevel) / (groundlevel - bottomlevel));
			
			break;
		}
	}
	
	//fade in fade out and display small battery animation
	void StartDrillingStage(){
		BlackFadeInOut.FadeIn ();
		planStage = PlanStage.Loading;
	}
	
	public float GetRemainTime(){
		return remainTime;
	}
	
	public PlanStage GetStage(){
		return planStage;
	}
	
	public void GameOver(){
		StartCoroutine(GameOverProcess());
	}
	
	IEnumerator GameOverProcess(){
		planStage = PlanStage.GameOver;
		CameraShaking.instance.SwitchEffect (CameraEffect.None);
		//GameOverUI.SetActive (true);
		//scoreText.text = DataManager.instance.GetCurrentScore ()+"";
		if (drillMachine.GetRemainHP () > 0) {
			//gameoverText.text = "DONE";
			//ambientSound.Stop();
			//ambientSound.PlayOneShot(drillstop, 1);
			ambSound.StopAmbient();
			ambSound.PlayOneShot(1, 1);
			HintCtrl.instance.UpdateHintText("End of path reached");
		}else{
			//gameoverText.text = "BROKEN";
			//ambientSound.Stop();
			//ambientSound.PlayOneShot(drillbroken, 1);
			ambSound.StopAmbient();
			ambSound.PlayOneShot(2, 1);
			HintCtrl.instance.UpdateHintText("Bit wore out");
		}
		//DrillUI.SetActive (false);
		yield return new WaitForSeconds (3);
		EndingFadeBlack.FadeIn ();
	}
	
	public void InViewLog(){
		inviewLog = true;
	}
	
	public void CloseViewLog(){
		inviewLog = false;
	}
	
	public void StartIntro(){
		planStage = PlanStage.Intro;
		GeologistUICtrl.instance.StartIntro ();
		
		//put sample points on stage
		object[] samples = DataManager.instance.GetCollectedSamples ();
		int i = 0;
		foreach (CoreSample sample in samples) {
			i++;
			GameObject point = Instantiate(CoreSamplePoints, sample.position, Quaternion.identity) as GameObject;
			point.GetComponentInChildren<TextMesh>().text = i+"";
		}
	}
	//called from GeologistUICtrl script
	public void IntroOver(){
		StartTutorial ();
	}
	
	
	public void StartTutorial(){
		Debug.Log("start tutorial");
		if (planStage == PlanStage.Intro) {
			planStage = PlanStage.Tutorial;
			TutorialControl.instance.gameObject.SetActive (true);
		}
	}
	
	public void StartGame(){
		planStage = PlanStage.Planning;
		TimeUI.SetActive (true);
		HintBox.SetActive (true);
		InvokeRepeating ("UpdateHintDisp", 0, 4);
		//plan prepare ambient sound
		//ambientSound.clip = standby;
		//ambientSound.Play ();
		//the total score of current level
		Transform layers = GameObject.Find ("/Layers").transform;
		int scoreEntityNum = 0;
		foreach (Transform child in layers) {
			float layerScore = child.GetComponent<LayerProperty>().GetScoreFactor();
			if(layerScore>0){
				totalScore += layerScore;
				scoreEntityNum++;
			}
		}
		DataManager.instance.SetAvgScore (totalScore / scoreEntityNum);
		totalScore *= (3f / 4f);
		//totalScore += 1000;	//total score of stage 2
		DataManager.instance.SetTotalScore (totalScore);	//save a copy to total score in DataManager
		DataManager.instance.score = 0;
	}
	
	public void Restart(){
		Destroy (SoundFXCtrl.instance.gameObject);
		Destroy (DataManager.instance.gameObject);
		Application.LoadLevel("GameMenu");
	}
	
	public float GetTotalLevelScore(){
		return totalScore;
	}
	
	public void PauseDrill(int dialogIndex){
		GeologistUICtrl.instance.PlayVoice (dialogIndex);
		StartCoroutine (PauseDrillImp (GeologistUICtrl.instance.DisplayDialogNow(dialogIndex)));
	}
	
	IEnumerator PauseDrillImp(float duration){
		planStage = PlanStage.DrillingPause;
		ambSound.StopAmbient ();
		CameraShaking.instance.SwitchEffect (CameraEffect.None);
		yield return new WaitForSeconds (duration);
		planStage = PlanStage.Drilling;
		ambSound.PlayAmbientLoop (2);
		
	}
	
	public void StartDrillPhase(){
		planStage = PlanStage.Drilling;
		scoreDisp.gameObject.SetActive (true);
		HPBar.SetActive (true);
		CameraShaking.instance.SwitchEffect (CameraEffect.Shake);
		ambSound.PlayAmbientLoop (2);
		DepthMeterCtrl.instance.SetScaledDepth (0);
		//ambientSound.clip = drilling;
		//ambientSound.Play ();
	}
	
	void UpdateHintDisp(){
		HintCtrl.instance.UpdateHintText (hints[currentHint]);
		currentHint++;
		if (currentHint >= hints.Length) {
			currentHint = 0;
		}
	}
	
	void FadeInFinish(GameObject sender){
		switch (sender.name) {
		case "FadeBlack":
			BlackFadeInOut.FadeOut();
			//planStage = PlanStage.Drilling;
			
			TimeUI.SetActive (false);
			DrillUI.SetActive (true);
			
			pencil.SetActive (false);
			Drill.gameObject.SetActive (true);
			Battery.SetActive (true);
			
			//HintBox.SetActive(false);
			CancelInvoke("UpdateHintDisp");
			HintCtrl.instance.UpdateHintText("Goal: 7,671,598 kWh");
			
			pickPointDelay = 0;
			
			Vector3 drillpos = drillObject.position;
			drillpos.y = groundlevel;
			drillTrailPoints.Add(drillpos);
			drillTrail.SetVertexCount(drillTrailPoints.Count);
			drillTrail.SetPosition(drillTrailPoints.Count-1, drillpos);
			
			break;
		case "EndingFadeBlack":
			Application.LoadLevel("EndingSkyline");
			break;
		}
	}
	
	void FadeOutFinish(GameObject sender){
		switch (sender.name) {
		case "FadeBlack":
			//BatteryCtrl.StartGathering();
			StartDrillPhase();
			break;
		}
	}
	
	
}
