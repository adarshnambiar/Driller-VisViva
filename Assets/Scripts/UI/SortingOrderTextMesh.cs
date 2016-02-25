using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class SortingOrderTextMesh : MonoBehaviour {
	public string sortingLayer;
	public int sortingOrder;

	//TextMesh text;

	// Use this for initialization
	void Start () {
		//text = GetComponent<TextMesh> ();
		//text.renderer.sortingLayerName = sortingLayer;
		//text.renderer.sortingOrder = sortingOrder;
		GetComponent<MeshRenderer> ().sortingLayerName = sortingLayer;
		GetComponent<MeshRenderer> ().sortingOrder = sortingOrder;
	}

}
