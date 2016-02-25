using UnityEngine;
using System.Collections;

public class DrillRotationFollow : MonoBehaviour {

	public float rotSpeed = 1;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(_transform.up!=-drillProxy.GetDirection()){
			_transform.up = Vector3.RotateTowards(_transform.up, -drillProxy.GetDirection(), rotSpeed*Time.deltaTime, 0);
		}
		*/
	}

	//drill should call this to update the rotation of drill
	public void UpdateRotation(Vector3 drillProxyDirection){
		if(_transform.up!=-drillProxyDirection){
			_transform.up = Vector3.RotateTowards(_transform.up, -drillProxyDirection, rotSpeed*Time.deltaTime, 0);
		}
	}

}
