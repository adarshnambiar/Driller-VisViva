using UnityEngine;
using System.Collections;

public enum CameraEffect{
	None,
	Shake,
};

public class CameraShaking : MonoBehaviour {

	public static CameraShaking instance;
	public float delay = 0.05f;
	public float shakingPower = 0.1f;

	CameraEffect curEffect;
	float leftdelay;
	float orgSize;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		curEffect = CameraEffect.None;
		leftdelay = delay;
		orgSize = Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		switch (curEffect) {
		case CameraEffect.Shake:
			if(leftdelay > 0){
				leftdelay -= Time.deltaTime;
				if(leftdelay <= 0){
					leftdelay = delay;
					Camera.main.orthographicSize = Random.Range(orgSize-shakingPower, orgSize);
				}
			}
			break;
		}
	}

	public void SwitchEffect(CameraEffect cameraEffect){
		curEffect = cameraEffect;
		switch (curEffect) {
		case CameraEffect.None:
			Camera.main.orthographicSize = orgSize;
			break;
		case CameraEffect.Shake:
			break;
		}
	}

	public void SetShakingPower(float newpower){
		shakingPower = newpower;
	}

}
