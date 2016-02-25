using UnityEngine;
using System.Collections;

public class GetLayerName : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GetLayerProperty(){
		object[] samples = DataManager.instance.GetCollectedSamples();

		foreach(string layername in samples){
			GameObject orgLayer = GameObject.Find ("/Layers/" + layername);
			if (orgLayer != null){
				LayerProperty propertyScript = orgLayer.GetComponent<LayerProperty> ();
			}
		}
	}

}
