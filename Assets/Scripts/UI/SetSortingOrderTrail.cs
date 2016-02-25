using UnityEngine;
using System.Collections;

public class SetSortingOrderTrail : MonoBehaviour {

	public string sortingOrderName;
	public int sortingOrder;

	// Use this for initialization
	void Start () {
		TrailRenderer trail = GetComponent<TrailRenderer> ();
		trail.renderer.sortingLayerName = sortingOrderName;
		trail.renderer.sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
