using UnityEngine;
using System.Collections;

public class DepthMeterCtrl : MonoBehaviour {

	public static DepthMeterCtrl instance;

	public Transform FillingBar;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//set depth value (amount 0 ~ 1)
	public void SetScaledDepth(float depthValue){
		Vector3 fillingBarScale = FillingBar.localScale;
		fillingBarScale.y = Mathf.Clamp01 (depthValue);
		FillingBar.localScale = fillingBarScale;
	}

}
