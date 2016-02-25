using UnityEngine;
using System.Collections;

public class SkylineBattery : MonoBehaviour {

	public Transform[] BatteryType;
	public Transform[] BatteryLevels;
	//public TextMesh scoreText;

	SpriteRenderer batterySprite;
	Animator batteryAnimation;
	// Use this for initialization
	void Start () {
		float scoreFactor = DataManager.instance.GetCurrentScore () / DataManager.instance.GetTotalScore ();
		scoreFactor = Mathf.Clamp01 (scoreFactor);

		int type = 0;
		float TotalScaleFactor = 1;
		if (scoreFactor < 0.25f) {
			type = 0;
			TotalScaleFactor = 0.25f;
		} else if (scoreFactor < 0.5f) {
			type = 1;
			TotalScaleFactor = 0.5f;
		} else if (scoreFactor < 0.75f) {
			type = 2;
			TotalScaleFactor = 0.75f;
		} else if (scoreFactor <= 1) {
			type = 3;
			TotalScaleFactor = 1;
		}

		Transform curBattery = BatteryType [type];
		//curBattery.gameObject.SetActive (true);
		Transform BatteryLevel = BatteryLevels [type];

		scoreFactor = scoreFactor / TotalScaleFactor;

		Vector3 levelScaler = BatteryLevel.localScale;
		levelScaler.y = scoreFactor;
		BatteryLevel.localScale = levelScaler;

		batterySprite = BatteryLevel.GetComponentInChildren<SpriteRenderer> ();
		batterySprite.color = Color.Lerp (Color.red, Color.green, scoreFactor);

		//scoreText.text = string.Format("{0:n0}", (DataManager.instance.GetCurrentScore () / DataManager.instance.GetTotalScore ())*7000000)+"kwh";

		for (int i = 0; i< BatteryType.Length; i++) {
			if(i != type){
				BatteryType[i].gameObject.SetActive(false);
			}
		}


		batteryAnimation = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BatteryJumpOut(){
		batteryAnimation.SetTrigger ("bounce");
	}

}
