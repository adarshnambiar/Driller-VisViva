using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionBoxCtrl : MonoBehaviour {

	public float LerpSpeed = 2;
	public Text DispText;
	public Image ConfirmBtn;
	public Image DeclineBtn;

	int curSelection = 1;	//0 - confirm button, 1 - decline button
	float transitState = 0;	//the state of transition

	// Use this for initialization
	void Start () {
		transitState = 0;
	}
	
	// Update is called once per frame
	void Update () {
		switch (curSelection) {
		case 0:
			ConfirmBtn.color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(transitState, 1));
			break;
		case 1:
			DeclineBtn.color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(transitState, 1));
			break;
		}
		transitState += Time.deltaTime*LerpSpeed;
	}

	public void DispOptionBox(string content){
		gameObject.SetActive (true);
		DispText.text = content;
		curSelection = 0;
		ChangeSelection (1);
	}

	public void CloseOptionBox(){
		gameObject.SetActive (false);
	}

	public bool ChangeSelection(int select){
		if (curSelection != select) {
			ConfirmBtn.color = Color.white;
			DeclineBtn.color = Color.white;
			curSelection = select;
			transitState = 0;
			return true;
		} else {
			return false;
		}
	}

	public int GetCurrentSelection(){
		return curSelection;
	}

}
