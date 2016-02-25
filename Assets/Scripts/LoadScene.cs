using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	/*
		if (TimeUI.gameend) 
		{	
			Application.LoadLevel("Game Over");
			networkView.RPC ("GameEnd", RPCMode.All);
		}
		*/
	}

	[RPC]
	public void GameEnd(){
		Application.LoadLevel ("Game Over");
	}

	[RPC]
	public void UpdatePosition(Vector3 pos){
		transform.position = pos;
	}
}
