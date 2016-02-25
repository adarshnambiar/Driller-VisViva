using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrillControlNew : MonoBehaviour {
	public static DrillControlNew instance;	//singleton reference
	[Header("Sound")]
	public AudioSource ambientSound;
	public AudioClip drillStopSFX;
	public AudioClip takeSampleSFX;
	public AudioClip endBtnSFX;
	[Header("Other")]
	public SamplesCollected sampleCollectedCount;
	public Score score;
	Vector3 backTarget;				//current target if moving backword
	ArrayList backRoadPoint;
	ArrayList tempPoint;
	public float BackSpeed = 2;		//speed for moving backward
	public float maxWheelRotation=2f; //restricting the wheel rotation 
	Vector3 direction;				//direction of the drill
	public float speed =0.5f;		//speed of the drill
	public bool drillbacktrack;
	public bool drillforward;
	public bool movehorizontal;
	public bool issampling;
	public bool istrail;
	public float totalTime;
	public float groundlevel=4.81f;
	public GameObject TrailObject;
	public GameObject TrailShow;
	public GameObject Driller;
	public float vertical;
	private bool gamestarted;
	public bool traildrawn;
	public  LayerProperty layerProperty;
	public int samplecollected;
	private Vector3 tempheight;
	public GameObject CoreSamplePoints;
	public GameObject mark;
	//public GameObject TrailStart;
	//private LayerType type;
	Animator drillAnimator;
	//public Hashtable LayerName;
	public bool startgame;
	public GameObject drillbase;
	public int count;//Drill Wait count for next drilling 
	public bool drilliswaiting;
	public bool check_layer;
	private GameObject upperboundary;
	private GameObject leftboundary;
	private GameObject rightboundary;
	void Awake()
	{
		
		startgame = false;
		instance = this;
	}
	// Use this for initialization
	void Start () {
		
		//backRoadPoint= new ArrayList();
		tempPoint=new ArrayList();
		drillforward = true;
		drillbacktrack = false;
		issampling = false;
		istrail = false;
		movehorizontal = true;
		drillAnimator = Driller.GetComponent<Animator> ();
		//LayerName = new Hashtable ();
		samplecollected = 0;
//		vertical = 0;
		gamestarted = false;
		traildrawn = false;
		drilliswaiting = false;
		count = 0;
		InvokeRepeating ("CheckforGameStart",1f,1f);
		upperboundary = GameObject.Find ("UpperBottomBoundary");
		leftboundary= GameObject.Find ("UpperLeftBound");
		rightboundary=GameObject.Find ("UpperRightBound");

		
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(!gamestarted){
		transform.position=new Vector3(transform.position.x, groundlevel,transform.position.z);
			}
		vertical = (float)Input.GetAxis ("Mouse Y") * speed;
		if (startgame) {
						drillAnimator.speed=0.5f;
						gamestarted = true;
						float horiControl = Input.GetAxis ("Horizontal");
						if (vertical >= maxWheelRotation) {
								vertical = maxWheelRotation;
						}
						if (vertical <= -maxWheelRotation) {
								vertical = -maxWheelRotation;
						}
						if (horiControl > 0 && movehorizontal) {
								transform.Translate (Vector3.right * Time.deltaTime);
							if(transform.position.x>=rightboundary.transform.position.x)
							{
								transform.position = new Vector3(rightboundary.transform.position.x,transform.position.y,transform.position.z);
							}
							if(transform.position.x<=leftboundary.transform.position.x)
							{
								transform.position = new Vector3(leftboundary.transform.position.x,transform.position.y,transform.position.z);
							}
						} else
						if (horiControl < 0 && movehorizontal) {
								transform.Translate (-Vector3.right * Time.deltaTime);
							if(transform.position.x>=rightboundary.transform.position.x)
							{
							transform.position = new Vector3(rightboundary.transform.position.x,transform.position.y,transform.position.z);
							}
							if(transform.position.x<=leftboundary.transform.position.x)
							{
							transform.position = new Vector3(leftboundary.transform.position.x,transform.position.y,transform.position.z);
							}
						}
						vertical *= Time.deltaTime;
						if( !DrillManager.instance.c_lights_on){
								SendData.SendToArduinoTutorial('b');
								SendData.SendToArduinoTutorial('d');
								SendData.SendToArduinoTutorial('c');
								DrillManager.instance.bd_lights_on=true;
								DrillManager.instance.c_lights_on=true;
						}
						if (drillforward ) {	
								movehorizontal = false;
								issampling = true;
								
								//drillAnimator.SetBool ("isDrill", true);
									
								if (!istrail && !movehorizontal && groundlevel > transform.position.y) {			
										GameObject inst = Instantiate (TrailShow, new Vector3 (transform.position.x, groundlevel, transform.position.z), Quaternion.identity)as GameObject;
										inst.transform.parent = TrailObject.transform;
										istrail = true;
										ambientSound.Play ();
								}
								if (vertical > 0) {
										drillAnimator.speed=25.0f;
										rigidbody2D.transform.Translate (0, -vertical, 0);
								if(transform.position.y<=upperboundary.transform.position.y)
									{
								transform.position = new Vector3(transform.position.x,upperboundary.transform.position.y,transform.position.z);
									}
								
										tempPoint.Clear ();
										if (traildrawn) {
												if (transform.position.y < tempheight.y) {
														traildrawn = false;
												}

										}
								}
								if (groundlevel > transform.position.y && vertical < 0) {
									drillAnimator.speed=0.5f;
										traildrawn = true;
									if(TrailDraw.linePoints.Count>0){
										tempPoint.Add (TrailDraw.backRoadPoint [TrailDraw.linePoints.Count - 1]);
										tempheight = (Vector3)tempPoint [0];
					}
										rigidbody2D.transform.Translate (0, -vertical, 0);

								} else if (groundlevel <= transform.position.y) {
										movehorizontal = true;
										issampling = false;
										istrail = false;
										traildrawn = false;
										if (TrailDraw.trail == null) {
												// Debug.Log ("No Trail Drawn");
										} else
												TrailDraw.trail.enabled = false;
					
								}
			
				
				
						}
						layerProperty=CheckClawCollider.layerProperty;
						if (Input.GetButton ("Sample") && issampling) { // button clicked to sample current layer	

								if (layerProperty != null) {

										//	if(!LayerName.ContainsKey(layerProperty.name)){
									//	SmallBatteries battery = GameObject.Find ("Battery Holder").GetComponent<SmallBatteries>();
									//	battery.RemoveBattery();
										RockPopup rock = GameObject.Find("RockManager").GetComponent<RockPopup>();
										 check_layer = rock.Popups (CheckClawCollider.layerProperty);
										score.score -= 100;
										if (check_layer) {
						
												ambientSound.Pause ();
												ambientSound.PlayOneShot (drillStopSFX, 1);
												ambientSound.PlayOneShot (takeSampleSFX, 1);
												samplecollected++;
												sampleCollectedCount.collected++;
											//	SoundFXCtrl.instance.PlaySound (4, 1);
												//LayerName.Add(layerProperty.name,samplecollected);
												drillforward = false;	
												drillAnimator.SetBool ("isCollect", true);
										} else {
													samplecollected++;
													sampleCollectedCount.collected++;
													//Vector3 pointPos = new Vector3 (transform.position.x, transform.position.y , transform.position.z);
													//CoreSample sample = new CoreSample ("None", pointPos);
													//DataManager.instance.AddCollectedSamples (sample);
													drillAnimator.SetBool ("isCollect", true);
													ambientSound.Pause ();
													ambientSound.PlayOneShot (drillStopSFX, 1);
													SoundFXCtrl.instance.PlaySound (6, 1);
													//drillbacktrack = true;
										}
										issampling = false;
										istrail = false;
										drillforward = false;


					
								}
				
				
						}
			
						if (drillbacktrack) {
								drillAnimator.SetBool ("isCollect", false);
								//drillAnimator.SetBool ("isDrill", true);

								if(TrailDraw.backRoadPoint.Count!=0)
								{
								backTarget = (Vector3)TrailDraw.backRoadPoint [0];
								}
								backTarget.y = groundlevel;
								transform.position = Vector3.MoveTowards (transform.position, backTarget, BackSpeed * Time.deltaTime);
								if (transform.position == backTarget) {
									drillAnimator.speed=0.5f;
									movehorizontal = true;
									//vertical = 0;
										if(vertical>0.0f){
										drilliswaiting=false;
										drillforward = false;
											}else
											{
												//Debug.Log ("Stopped");
												drillbacktrack = false;
												drilliswaiting=true;
											}
										
										//startgame=false;
									
								}		
						}	

			if ((samplecollected == 5 && drilliswaiting && RockPopup.shelfupdated) || Input.GetKey(KeyCode.F2)|| (score.score == 0)) {
								if (Application.loadedLevelName == "Driller") {
										SoundFXCtrl.instance.PlaySound (5, 1);
										DataManager.instance.AddToScore (score.score);
										Application.LoadLevel ("PlanPath");
								}
						}  
			
			
				}
	}
	
	public void UpdateRotation(Vector3 drillProxyDirection){
		if(transform.up!=-drillProxyDirection){
			transform.up = Vector3.RotateTowards(transform.up, -drillProxyDirection, 1f*Time.deltaTime, 0);
		}
	}
	//change the linear drag, called by layers
	public void SetLinearDrag(float dragVal){
		rigidbody2D.drag = dragVal;
	}
	
	//update energy
//	void OnTriggerEnter2D(Collider2D other)
//	{
//		if (other.tag == "layer") {
//			layerProperty = other.GetComponent<LayerProperty> ();
//		}
//
//	}
	
	public void StartGameDriller()
	{	
		StartCoroutine(Wait());
	}

	IEnumerator Wait(){

		yield return new WaitForSeconds(1);
		startgame = true;
	} 

	
	void CheckforGameStart(){

		if (drilliswaiting && vertical==0.0f) {
			count+=1;
			//Debug.Log ("count: "+count);
			if(count==2){
			//Debug.Log ("Drill isstill");
			StartCoroutine(Wait());
			drilliswaiting=false;
			count=0;
			drillforward = true;
			}
		}
	}

}
