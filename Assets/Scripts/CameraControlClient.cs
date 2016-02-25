using UnityEngine;
using System.Collections;

public class CameraControlClient : MonoBehaviour {

	public float speed = 1;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.P)) {
			_transform.position -= Vector3.up * speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.O)){
			_transform.position += Vector3.up * speed * Time.deltaTime;
		}
	}
}
