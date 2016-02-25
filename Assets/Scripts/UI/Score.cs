using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	Text scoreText;
	public int score = 1000;

	// Use this for initialization
	void Start () {
		scoreText = gameObject.GetComponent<Text>();
		scoreText.text = "Score : " + string.Format ("{0}",score);
	}

	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score : " + string.Format ("{0}",score);
		
	}

}
