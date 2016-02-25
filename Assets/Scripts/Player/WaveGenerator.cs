using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	public static WaveGenerator instance;

	public float chargeSpeed = 2f;			//speed of catch up with the volumn
	public float chargeLoseSpeed = 0.5f;	//speed of volumn fade out
	public float fallBackSpeed = 0.2f;		//speed of crosshair moving back after sending a wave
	//public Transform energyBar;

	public float spinAngle = 60;			//left and right spinning angle
	public float spinSpeed = 2;				//left and right spinning speed
	public Transform sendDir;				//the transform indicating sending the wave from wave to which direction
	public bool canSendWave = false;		//enable after some point in tutorial

	public GameObject CrossHairGO;			//the crosshair game object
	public float MaxDepth = 10;				//the maximum the cross hair can get(the wave can reach)

	float currentPower;	//a float value between 0 to 1
	Vector3 leftSpinAngle;
	Vector3 rightSpinAngle;
	bool spinLeft;
	CrossHair crossHairCtrl;
	float scaleFactor;
	bool waveSent;



	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		currentPower = 0;
		scaleFactor = 0;
		//energyBar.localScale = new Vector3(currentPower*0.4f, 0.4f, 1);

		leftSpinAngle = Quaternion.Euler (0, 0, -spinAngle) * Vector3.up;
		rightSpinAngle = Quaternion.Euler (0, 0, spinAngle) * Vector3.up;
		spinLeft = true;

		waveSent = false;
		canSendWave = false;

		crossHairCtrl = CrossHairGO.GetComponent<CrossHair> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GeoGameManager.instance.GetCurrentGameStage () == GeoStage.InGame||GeoGameManager.instance.GetCurrentGameStage () == GeoStage.Tutorial) {

			if (!waveSent) {
				//scaleFactor = MicroPhoneInput.instance.GetScaledLoudness ();
				scaleFactor = PitchInput.instance.GetScaledPitch();
				Debug.Log("scaledFactor: "+scaleFactor);
				float delta = Mathf.Abs (scaleFactor - currentPower);
				if (currentPower > scaleFactor) {
					currentPower = Mathf.MoveTowards (currentPower, scaleFactor, chargeLoseSpeed * Mathf.Clamp01 (delta / 0.2f) * Time.deltaTime);
				} else {
					//Debug.Log ("recording:"+scaleFactor);
					currentPower = Mathf.MoveTowards (currentPower, scaleFactor, chargeSpeed * Mathf.Clamp01 (delta / 0.2f) * Time.deltaTime);
				}
			} else {
				/********After sending a wave out, player need to wait for the wave to move back to the original place******/
				currentPower = Mathf.MoveTowards (currentPower, 0, fallBackSpeed * Time.deltaTime);
				if (currentPower == 0) {
					waveSent = false;
				}
			}
			/**********Send Wave with one click**************************/
			if (Input.GetButtonDown ("SendWave")&&canSendWave) {
				if (currentPower > 0.05f && waveSent == false) {
					GeoGameManager.instance.CreateDetectWave (currentPower * MaxDepth, sendDir.up);
					waveSent = true;
					SoundFXCtrl.instance.PlaySound(3, 0.5f);
				}
			}

		} else {
			scaleFactor = 0;
		}

		crossHairCtrl.UpdatePosition(currentPower * MaxDepth);

		float crossHairMovement = Input.GetAxis("Horizontal_Stage1");

		if (crossHairMovement<0) {
			sendDir.up = Vector3.RotateTowards (sendDir.up, leftSpinAngle, spinSpeed * Time.deltaTime, 0.0f);
			if(Vector3.Angle(sendDir.up, leftSpinAngle)<3){
				spinLeft = false;
			}
		} else if(crossHairMovement>0) {
			sendDir.up = Vector3.RotateTowards (sendDir.up, rightSpinAngle, spinSpeed * Time.deltaTime, 0.0f);
			if(Vector3.Angle(sendDir.up, rightSpinAngle)<3){
				spinLeft = true;
			}
		}

		if (waveSent == false && currentPower > 0.05f) {
			crossHairCtrl.ActiveState ();
		} else {
			crossHairCtrl.NegativeState();
		}

		//energyBar.localScale = new Vector3(currentPower*0.4f, 0.4f, 1);
	}

	public float GetSoundScaledLevel(){
		return scaleFactor;
	}

	public float GetCurrentPower(){
		return currentPower * Vector3.Dot(Vector3.up, sendDir.up);
	}

	public float GetUnscaledSoundPower(){
		return currentPower;
	}

}
