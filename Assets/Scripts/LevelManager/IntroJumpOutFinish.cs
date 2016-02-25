using UnityEngine;
using System.Collections;

public class IntroJumpOutFinish : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void JumpOutFinish(){
		GeologistUICtrl.instance.StartIntro ();
	}

}
