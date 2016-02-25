using UnityEngine;
using System.Collections;

public class TransmitDrillPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

 		//transform.parent.networkView.RPC("UpdatePosition",RPCMode.Others,transform.parent.position,transform.localScale);
	}

	[RPC]
	public void UpdatePosition(Transform pos,Transform scale){
		//empty function to enable RPC call 
	}
}
