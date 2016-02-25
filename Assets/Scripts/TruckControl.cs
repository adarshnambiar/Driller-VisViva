using UnityEngine;
using System.Collections;

public class TruckControl : MonoBehaviour {

	// Use this for initialization
	public float truckSpeed;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		MoveTruck();
	}

	void MoveTruck()
	{
		float movement = Input.GetAxis("Horizontal") * truckSpeed * Time.deltaTime;
		transform.Translate(movement, 0, 0);
	}

}
