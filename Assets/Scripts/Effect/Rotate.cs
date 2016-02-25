using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float rotateSpeed = 90;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		_transform.Rotate (Vector3.forward * rotateSpeed*Time.deltaTime, Space.World);
	}
}
