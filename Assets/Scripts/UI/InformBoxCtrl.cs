using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InformBoxCtrl : MonoBehaviour {

	public Text informText;

	float remainDispTime = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		remainDispTime -= Time.deltaTime;
		if (remainDispTime <= 0) {
			gameObject.SetActive(false);
		}
	}

	public void DispInform(string inform, float staytime){
		gameObject.SetActive (true);
		informText.text = inform;
		remainDispTime = staytime;
	}


}
