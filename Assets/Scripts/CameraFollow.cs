using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform followTarget;
	public float yOffset = -0.3f;
	public float followSpeed = 3;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
		_transform.position = new Vector3 (followTarget.position.x, followTarget.position.y+yOffset, _transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		_transform.position = Vector3.MoveTowards (_transform.position, new Vector3 (followTarget.position.x, followTarget.position.y + yOffset, _transform.position.z), followSpeed * Time.deltaTime);
	}
}
