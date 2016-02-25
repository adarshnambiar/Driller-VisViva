using UnityEngine;
using System.Collections;

public class SoundCatcher : MonoBehaviour {

	//private bool inRange = false;
	//public GameObject sound;
	//private ScoreUI scoreUI;

	public static SoundCatcher instance;

	ArrayList respondSounds;	//use a array to handle multiple sounds

	void Start()
	{
		instance = this;

		respondSounds = new ArrayList ();
		//GameObject scoreUIText = GameObject.FindWithTag ("ScoreUI");
		//scoreUI = scoreUIText.GetComponent <ScoreUI>();
	}

	/*
	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.tag=="IncomingSound")
		{
			Debug.Log("sound in");
			//prevent a object be inserted twice
			if(!respondSounds.Contains(other.gameObject)){
				respondSounds.Add(other.gameObject);
			}
			//inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if(other.tag=="IncomingSound")
		{
			Debug.Log("sound out");
			respondSounds.Remove(other.gameObject);
			//inRange = false;
			//Destroy(other.gameObject);
		}
	}*/

	public void AddToList(GameObject sound){
		if(!respondSounds.Contains(sound)){
			respondSounds.Add(sound);
		}
	}

	public void DeleteFromList(GameObject sound){
		respondSounds.Remove (sound);
	}


	void Update()
	{
		/*
		int inputBtn = -1;
		if (Input.GetButtonDown ("CatchGreen")) {
			//Debug.Log("Green Btn");
			inputBtn = 0;
		}else if(Input.GetButtonDown ("CatchRed")) {
			//Debug.Log("Red Btn");
			inputBtn = 1;
		}else if(Input.GetButtonDown ("CatchBlue")) {
			//Debug.Log("Blue Btn");
			inputBtn = 2;
		}else if(Input.GetButtonDown ("CatchYellow")) {
			//Debug.Log("Yellow Btn");
			inputBtn = 3;
		}


		if (respondSounds.Count != 0&&inputBtn!=-1) {
			//see if any sounds are catched
			ArrayList temp = new ArrayList();
			foreach (GameObject sound in respondSounds) {
				RespondWave soundScript = sound.GetComponent<RespondWave> ();
				if (soundScript.type == inputBtn) {
					GeoGameManager.instance.RespondCatch (soundScript.layername);
					temp.Add(sound);
					soundScript.RemoveNow();
				}
			}

			//remove carched sounds
			foreach(GameObject sound in temp){
				respondSounds.Remove(sound);
			}

		}
		*/
	}

}

