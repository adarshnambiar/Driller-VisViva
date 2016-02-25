using UnityEngine;
using System.Collections;

public class CheckClawCollider : MonoBehaviour {
	public static LayerProperty layerProperty;


	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.tag == "layer" ) {
			layerProperty = other.gameObject.GetComponent<LayerProperty> ();
		}
		
	}
		
	
	void OnTriggerStay2D(Collider2D other)
	{
		
		if (other.tag == "layer" ) {
			layerProperty = other.gameObject.GetComponent<LayerProperty> ();
		}
		
	}


}
