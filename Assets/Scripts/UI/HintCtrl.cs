using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintCtrl : MonoBehaviour {

	public static HintCtrl instance;

	public Text hintText;
	public Image whiteMask;
	public float transitSpeed = 2;

	int transitState;	//0-normal, 1-white fade in, 2-white fade out
	Color transitCol;
	string nextText;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		nextText = "";
		hintText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		switch (transitState) {
		case 1:
			transitCol = whiteMask.color; 
			transitCol.a = Mathf.MoveTowards(transitCol.a, 1, transitSpeed * Time.deltaTime);
			whiteMask.color = transitCol;
			//whiteMask.color = Color.Lerp(whiteMask.color, Color.white, transitSpeed * Time.deltaTime);
			if(transitCol.a == 1){
				transitState = 2;
				hintText.text = nextText;
			}
			break;
		case 2:
			transitCol.a = Mathf.MoveTowards(transitCol.a, 0, transitSpeed * Time.deltaTime);
			whiteMask.color = transitCol;
			//whiteMask.color = Color.Lerp(whiteMask.color, transparent, transitSpeed * Time.deltaTime);
			if(transitCol.a == 0){
				transitState = 0;
			}
			break;
		}
	}

	public void UpdateHintText(string newtext){
		nextText = newtext;
		transitState = 1;
	}

}
