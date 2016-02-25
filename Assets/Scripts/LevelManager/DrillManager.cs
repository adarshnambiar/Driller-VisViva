using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DrillManager : MonoBehaviour {

	public static DrillManager instance;
	public bool tutorialmode;
	public GameObject Drill;
	public GameObject SampleRobot;
	public int count = 0;
	private float tempheight;
	private Vector3 startpos;
	public GameObject TrailShow;
	public bool istrail;
	public static List<Vector3> linePoints_1;
	LineRenderer linerenderer_1;	
	public static ArrayList backRoadPoint_1;
	public bool check1,check2;
	public float fadeSpeed = 1.5f;
	public Animator drillAnimator;
	public bool intromode;
	public bool bd_lights_on;
	public bool c_lights_on;



	void Awake(){
		instance = this;
		tutorialmode = false;
	}
	
	// Use this for initialization
	void Start () {

		tempheight = Drill.transform.position.y;
		startpos=Drill.transform.position;
		linePoints_1 = new List<Vector3>();
		backRoadPoint_1= new ArrayList();
		linerenderer_1=gameObject.GetComponent<LineRenderer>();
		drillAnimator = SampleRobot.GetComponent<Animator> ();
		istrail = true;
		check1 = false;
		check2 = false;
		intromode = true;
		InvokeRepeating ("CheckforTutorialsLED",1f,1f);
		bd_lights_on = false;
		c_lights_on = false;

	}
	
	// Update is called once per frame
	void Update () {


		if (intromode) {
						
			if (Input.GetButtonDown ("NextStage")) {
								//Debug.Log ("STOPPING INTRO");
								GeologistUICtrl.instance.StopIntro ();
								intromode = false;	
						}
				} else if (tutorialmode) {
						
			if (Input.GetButtonDown ("NextStage")) {
				//Debug.Log ("STOP TUTORIAL");
				Drill.transform.position = startpos;
				TutorialControl.instance.TotalFadeOut ();
				linerenderer_1.enabled = false;
				DrillControlNew.instance.StartGameDriller ();
				tutorialmode = false;
			}
						if (TutorialControl.instance.GetCurrentIndex () == 0) {
								if ((Input.GetButton ("Horizontal"))) {
										float horiControl = Input.GetAxis ("Horizontal");
										if (horiControl > 0) {
												check1 = true;
												Drill.transform.Translate (Vector3.right * Time.deltaTime);
										} 
										if (horiControl < 0) {
												check2 = true;
												Drill.transform.Translate (-Vector3.right * Time.deltaTime);
										}


								}
								if ((check1 && check2)) {
							
										TutorialControl.instance.DisplayNext ();
				
								}

						} else
				if (TutorialControl.instance.GetCurrentIndex () == 1) {
								check1 = false;
								check2 = false;
								float vertical = Input.GetAxis ("Mouse Y") * 0.5f;
								vertical *= Time.deltaTime;

								if ((vertical > 0)) {	
										Drill.transform.Translate (0, -vertical, 0);
										linePoints_1.Add (Drill.transform.position);
										linerenderer_1.SetVertexCount (linePoints_1.Count);
										linerenderer_1.SetPosition (linePoints_1.Count - 1, new Vector3 (Drill.transform.position.x, Drill.transform.position.y + 0.6f, Drill.transform.position.z));
										if (Drill.transform.position.y <= 3.2f) {
												check1 = true;
										}
				
								} else
				if (Drill.transform.position.y >= tempheight) {
										Drill.transform.position = new Vector3 (Drill.transform.position.x, tempheight, Drill.transform.position.z);
								}
								if (check1 || (Input.GetButtonDown ("NextStage"))) {
										TutorialControl.instance.DisplayNext ();

								}
						} else
				if (TutorialControl.instance.GetCurrentIndex () == 2) {
								if ((Input.GetButton ("Sample"))) {
										//drillAnimator.SetBool ("isCollect", true);
										TutorialControl.instance.DisplayNext ();
								}
				
						} else
				if (TutorialControl.instance.GetCurrentIndex () == 3) {
				if ((Input.GetButton ("Horizontal")) || (Input.GetButtonDown ("Sample"))) {
							
										TutorialControl.instance.DisplayNext ();

								}
					
						} else
				if (TutorialControl.instance.GetCurrentIndex () == 4) {
				if ((Input.GetButton ("Horizontal")) || (Input.GetButtonDown ("Sample"))) {
					
										TutorialControl.instance.DisplayNext ();
					
								}

						} else
				if (TutorialControl.instance.GetCurrentIndex () == 5) {
								if ((Input.GetButton ("Horizontal")) || (Input.GetButtonDown ("Sample"))) {
					
										TutorialControl.instance.DisplayNext ();
										Drill.transform.position = startpos;
										TutorialControl.instance.TotalFadeOut ();
										linerenderer_1.enabled = false;
										DrillControlNew.instance.StartGameDriller ();
										tutorialmode = false;
					
								}
				
						}
				}
	}
	
	public  void StartTutorial(){
		//Debug.Log ("Starting Tutorial");
		intromode = false;
		tutorialmode = true;
		TutorialControl.instance.gameObject.SetActive (true);


	}

	IEnumerator AudioChange(float t) {
		yield return new WaitForSeconds(t);
		BGMCtrl.instance.ChangeBGMWithoutFade(2);

	}

//	void CheckforTutorial(){
//		
//		if (!tutorialmode && !DrillControlNew.instance.startgame) {
//			DrillControlNew.instance.startgame=true;
//			}
//		}

	public void StartIntro(){
		GeologistUICtrl.instance.StartIntro ();
		float time=BGMCtrl.instance.ChangeBGM(1);
		//StartCoroutine (AudioChange (time+0.5f));
	}

	public void IntroOver(){
		if (intromode) {
						StartTutorial ();
				}
	}

	public void CheckforTutorialsLED()
	{
				if (tutorialmode) {
						if (TutorialControl.instance.GetCurrentIndex () == 0 && !bd_lights_on) {
								SendData.SendToArduinoTutorial ('b');
								SendData.SendToArduinoTutorial ('d');
								bd_lights_on = true;
						}
						if (TutorialControl.instance.GetCurrentIndex () == 2 && !c_lights_on) {
								SendData.SendToArduinoTutorial ('c');	
								c_lights_on = true;
						}
				} 
		}



}
