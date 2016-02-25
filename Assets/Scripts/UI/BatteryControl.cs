using UnityEngine;
using System.Collections;

public class BatteryControl : MonoBehaviour {

	public SmallBatteryNew[] Batteries;
	public SpriteRenderer[] BatteryGlows;
	public float LevelupSpeed = 0.5f;
	public float TransitSpeed = 1f;

	float curBatteryLevel;	//current battery fill level
	float nextBatteryLevel;	//next battery fill level

	int curActiveBattery;	//current battery state
	int nextActiveBattery;	//next battery state
	float TotalScaleFactor;	//factor used for battery sprite level scaling

	int status = 0;					//current game status, 0-normal, 1-transit effect fade in, 2-transit effect fade out	
	float transitFactor = 0;		//indicating transition progress

	Color transparent;

	// Use this for initialization
	void Start () {
		curBatteryLevel = 0;
		nextBatteryLevel = 0;

		curActiveBattery = 0;
		nextActiveBattery = 0;

		TotalScaleFactor = 0.25f;
		DisableAllBatteries ();
		Batteries [curActiveBattery].gameObject.SetActive (true);

		status = 0;
		transitFactor = 0;

		transparent = Color.white;
		transparent.a = 0;

	}
	
	// Update is called once per frame
	void Update () {
		switch(status){
		case 0:
			if (nextBatteryLevel != curBatteryLevel) {
				curBatteryLevel = Mathf.MoveTowards(curBatteryLevel, nextBatteryLevel, LevelupSpeed * Time.deltaTime);
				if(curBatteryLevel>0&&curBatteryLevel<0.25f){		//activate battery 0
					nextActiveBattery = 0;
				}else if(curBatteryLevel>=0.25f&&curBatteryLevel<0.5f){
					nextActiveBattery = 1;
				}else if(curBatteryLevel>=0.5f&&curBatteryLevel<0.75f){
					nextActiveBattery = 2;
				}else if(curBatteryLevel>=0.75f&&curBatteryLevel<=1){
					nextActiveBattery = 3;
				}

				if(nextActiveBattery != curActiveBattery){
					status = 1;
				}else{
					UpdateFillLevelDisplay();
				}
			}
			break;
		case 1:
			transitFactor = Mathf.MoveTowards(transitFactor, 1, TransitSpeed * Time.deltaTime);
			BatteryGlows[nextActiveBattery].color = Color.Lerp(transparent, Color.white, transitFactor);
			if(transitFactor == 1){
				status = 2;
				Batteries[curActiveBattery].gameObject.SetActive(false);
				curActiveBattery = nextActiveBattery;
				Batteries[curActiveBattery].gameObject.SetActive(true);
				switch(curActiveBattery){
				case 0:
					TotalScaleFactor = 0.25f;
					break;
				case 1:
					TotalScaleFactor = 0.5f;
					break;
				case 2:
					TotalScaleFactor = 0.75f;
					break;
				case 3:
					TotalScaleFactor = 1;
					break;
				}
				UpdateFillLevelDisplay();
			}
			break;
		case 2:
			transitFactor = Mathf.MoveTowards(transitFactor, 0, TransitSpeed * Time.deltaTime);
			BatteryGlows[curActiveBattery].color = Color.Lerp(transparent, Color.white, transitFactor);
			if(transitFactor == 0){
				status = 0;
			}
			break;
		}
	}

	public void UpdateCurrentLevel(float scaleFactor){
		nextBatteryLevel = scaleFactor;
	}

	void DisableAllBatteries(){
		foreach (SmallBatteryNew bat in Batteries) {
			bat.gameObject.SetActive(false);
		}
	}

	void UpdateFillLevelDisplay(){
		Batteries [curActiveBattery].SetFillLevel (curBatteryLevel/TotalScaleFactor);
	}

}
