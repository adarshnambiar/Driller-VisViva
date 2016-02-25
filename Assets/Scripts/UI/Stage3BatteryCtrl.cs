using UnityEngine;
using System.Collections;

public class Stage3BatteryCtrl : MonoBehaviour {

	public Transform[] smallBatteries;
	public Transform batteryShelf;
	public Transform gatherPosition;
	public Transform batteryLevel;
	public float gatherSpeed = 1;
	public float scaleSpeed = 0.5f;
	public AudioClip[] batterySound;

	int stage = 0;						//transition stage: 0- nothing, 1- small batteries gathering, 2- shelf disappearing
	Vector3 batteryLevelScale;			//used for settin the scale volume for battery fullfillment
	float currentScaleLevel;
	int curActBattery = 0;				//the index of current activated small battery
	SpriteRenderer[] batterySprites;	//reference to battery sprites to achieve transparency transition
	SpriteRenderer batteryLevelSprite;	//for changing the color of battery level
	Color transparent;					//transparent color
	float tmpDist;						
	Vector3 shelfDisappearPos;
	int curActSound;
	int batCountDown;

	// Use this for initialization
	void Start () {
		stage = 0;
		//DataManager.instance.AddToScore (1000); //for testing
		int used = 10 - (int)(DataManager.instance.GetCurrentScore()/100);
		batterySprites = new SpriteRenderer[smallBatteries.Length];
		for (int i = 0; i < smallBatteries.Length; i++) {
			batterySprites[i] = smallBatteries[i].GetComponent<SpriteRenderer>();
		}

		for(curActBattery = 0; curActBattery < used; curActBattery++){
			smallBatteries[curActBattery].gameObject.SetActive(false);
		}

		transparent = Color.white;
		transparent.a = 0;
		shelfDisappearPos = batteryShelf.position + 1.2f * batteryShelf.up;

		batteryLevelSprite = batteryLevel.GetComponentInChildren<SpriteRenderer> ();
		batteryLevelScale = batteryLevel.localScale;
		currentScaleLevel = 0;
		batteryLevelScale.x = currentScaleLevel;
		batteryLevel.localScale = batteryLevelScale;

		curActSound = 0;
		batCountDown = 2;
	}
	
	// Update is called once per frame
	void Update () {
		switch (stage) {
		case 1:
			smallBatteries[curActBattery].position = Vector3.MoveTowards(smallBatteries[curActBattery].position, gatherPosition.position, gatherSpeed * Time.deltaTime);
			batterySprites[curActBattery].color = Color.Lerp(transparent, Color.white, Vector3.Distance (smallBatteries[curActBattery].position, gatherPosition.position)/tmpDist);
			if(smallBatteries[curActBattery].position == gatherPosition.position){
				smallBatteries[curActBattery].gameObject.SetActive(false);
				curActBattery++;

				SoundFXCtrl.instance.PlaySound(batterySound[curActSound], 1);
				batCountDown--;
				if(batCountDown==0){
					batCountDown = 2;
					curActSound++;
				}

				SoundFXCtrl.instance.PlaySound(7, 1);
				currentScaleLevel += 100/PlanPathManager.instance.GetTotalLevelScore();
				if(curActBattery<smallBatteries.Length){
					tmpDist = Vector3.Distance (smallBatteries[curActBattery].position, gatherPosition.position);
				}else{
					stage = 2;
				}
			}
			break;
		case 2:
			batteryShelf.position = Vector3.MoveTowards(batteryShelf.position, shelfDisappearPos, gatherSpeed * Time.deltaTime);
			if(batteryShelf.position == shelfDisappearPos){
				batteryShelf.gameObject.SetActive(false);
				stage = 0;
				PlanPathManager.instance.StartDrillPhase();
			}
			break;
		}

		if(batteryLevelScale.x != currentScaleLevel){
			batteryLevelScale.x = Mathf.MoveTowards (batteryLevelScale.x, currentScaleLevel, scaleSpeed * Time.deltaTime);
			batteryLevel.localScale = batteryLevelScale;
			batteryLevelSprite.color = Color.Lerp(Color.red, Color.green, batteryLevelScale.x);
		}

	}

	public void StartGathering(){
		if (curActBattery < smallBatteries.Length) {
			stage = 1;
			tmpDist = Vector3.Distance (smallBatteries [curActBattery].position, gatherPosition.position);
		} else {
			stage = 2;
		}
	}

	public void UpdateCurrentLevel(float scaleFactor){
		currentScaleLevel = scaleFactor;
	}

}
