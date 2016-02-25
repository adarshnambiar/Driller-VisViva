using UnityEngine;
using System.Collections;

public class DrillOnClient : MonoBehaviour {

	public DrillRotationFollow drillRotCtrl;

	Transform _transform;
	Vector3 direction;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
	}
	[RPC]
	public void UpdatePosition(Vector3 dir){
		///_transform.position = pos;
		//direction = dir;
		drillRotCtrl.UpdateRotation (dir);
	}

}
