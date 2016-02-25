using UnityEngine;
using System.Collections;

public class SortingLayer : MonoBehaviour {

	public GameObject Drill;
	TrailRenderer trail;
	// Use this for initialization
	void Start () {
		trail = Drill.GetComponent<TrailRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}
}
