using UnityEngine;
using System.Collections;

public class SortingOrderModel : MonoBehaviour {

	public string sortingLayer;
	public int sortingOrder;

	// Use this for initialization
	void Start () {
		MeshRenderer[] meshrenderers = GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer meshrenderer in meshrenderers) {
			meshrenderer.sortingLayerName = sortingLayer;
			meshrenderer.sortingOrder = sortingOrder;
		}
	}

}
