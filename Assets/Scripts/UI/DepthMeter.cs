using UnityEngine;
using System.Collections;

public class DepthMeter : MonoBehaviour {

	public float SmoothSpeed = 10;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (WaveGenerator.instance != null) {
			Vector3 scale = _transform.localScale;
			scale.y = Mathf.Clamp01(Mathf.MoveTowards(scale.y, WaveGenerator.instance.GetCurrentPower(), SmoothSpeed * Time.deltaTime));

			_transform.localScale = scale;
		}
	}
}
