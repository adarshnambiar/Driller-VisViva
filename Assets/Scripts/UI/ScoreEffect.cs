using UnityEngine;
using System.Collections;

public class ScoreEffect : MonoBehaviour {
	public TextMesh scoreText;

	int finalScore = 0;
	int digitCount = 0;

	// Use this for initialization
	void Start () {
		//scoreText.text = string.Format("{0:n0}", (DataManager.instance.GetCurrentScore () / DataManager.instance.GetTotalScore ())*7000000)+"kwh";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float DisplScore(){
		gameObject.SetActive (true);
		finalScore = (int)((DataManager.instance.GetCurrentScore () / DataManager.instance.GetTotalScore ()) * 7000000);
		int temp = finalScore;
		digitCount = 0;
		while (temp>0) {
			temp /= 10;
			digitCount++;
		}
		StartCoroutine (UpdateScoreDisplay ());
		return digitCount;
	}

	IEnumerator UpdateScoreDisplay(){
		int curDigit = 0;
		while (curDigit<digitCount) {
			for(int k = 0; k < 15; k++){
				string dispNum = "";
				int scoreCopy = finalScore;
				for(int i = 0; i < curDigit; i++){
					dispNum = scoreCopy%10 + dispNum;
					scoreCopy /= 10;
				}
				for(int i = 0; i < digitCount - curDigit; i++){
					dispNum = Random.Range(1, 9) + dispNum;
				}
				Debug.Log(dispNum);
				scoreText.text = string.Format("{0:n0}", int.Parse(dispNum))+" kWh";
				yield return new WaitForSeconds(0.025f);
			}
			curDigit++;
		}
		scoreText.text = string.Format("{0:n0}", finalScore)+" kWh";
	}

}
