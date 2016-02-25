using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererOrder : MonoBehaviour {

	public string layerName;
	public int layerorder;

	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
		line.renderer.sortingLayerName = layerName;
		line.renderer.sortingOrder = layerorder;
	}
}
