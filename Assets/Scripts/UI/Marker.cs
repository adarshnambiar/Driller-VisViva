using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {

	ArrayList layerScripts;

	// Use this for initialization
	void Start () {
		layerScripts = new ArrayList ();
		string layername = DataManager.instance.DetectLayerAtPosition (transform.position);
		layerScripts.Add (DataManager.instance.GetLayerProperty (layername));
		Debug.Log("add:"+layername);
	}

	void OnTriggerEnter2D(Collider2D other){
		/*
		if (other.tag == "layer") {
			if(DataManager.instance.CheckLayerReveal(other.name)){
				layerScripts.Add(other.GetComponent<LayerProperty>());
				Debug.Log("add:"+other.name);
			}
		}*/
		if (other.name == "drawPointer") {
			PlanPathManager.instance.MarkerNum++;
			Destroy(gameObject);
		}
		if (other.name == "Drill") {
			Destroy(gameObject);
		}
	}

	public float GetFossilFuelHere(){
		float result = 0;
		foreach (LayerProperty layerScript in layerScripts) {
			result += layerScript.ObtainFossilFuel();
		}
		return result;
	}

	public float GetAvgPossibilityHere(){
		float result = 0;
		foreach (LayerProperty layerScript in layerScripts) {
			result += layerScript.GetPossibility();
		}
		result /= layerScripts.Count;
		return result;
	}

	public bool HasBeenSampled(){
		LayerProperty layerS = layerScripts [0] as LayerProperty;
		return DataManager.instance.LayerHasBeenSampled(layerS.name);
	}

	public string HoldLayerName(){
		LayerProperty layerS = layerScripts [0] as LayerProperty;
		return layerS.name;
	}

}
