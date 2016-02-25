using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeUI : MonoBehaviour {
	
	Text  timeText;

	private float remainingTime = 0.0f;
	private int remainingMins = 0;
	private int remainingSeconds = 0;
	/*
	private float elapsedTime=0f;
	public int totalTimeSeconds;
	NetworkView eventsystem;
	public static bool gameend;
	*/
	// Use this for initialization
	void Start () {
		timeText = gameObject.GetComponent<Text>();
		/*
		elapsedTime = Time.time;
		remainingTime = totalTimeSeconds-elapsedTime;
		remainingMins = (int)remainingTime/60;
		remainingSeconds = (int)remainingTime%60;
		timeText.text="TIME REMAINING : " + string.Format("{0:0#}:{1:0#}", remainingMins, remainingSeconds);
		
		gameend = false;
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
		remainingTime = GeoGameManager.instance.GetRemainingTime ();
		remainingMins = (int)remainingTime/60;
		remainingSeconds = (int)remainingTime%60;
		//timeText.text="TIME REMAINING : " + string.Format("{0:0}:{1:0#}", remainingMins, remainingSeconds);
		timeText.text=string.Format("{0:0}:{1:0#}", remainingMins, remainingSeconds);
	}



}