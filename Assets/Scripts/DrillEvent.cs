using UnityEngine;
using System.Collections;

public class DrillEvent : MonoBehaviour {
	public static DrillEvent instance;	//singleton reference
	public GameObject CoreSamplePoints;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InstantiateSamplePoint () {

		if (DrillControlNew.instance.startgame) {
						if (TrailDraw.backRoadPoint.Count > 0 && DrillControlNew.instance.check_layer) {
								Vector3 pointPos = new Vector3 (transform.position.x, transform.position.y - 0.14f, transform.position.z - 0.5f);
								GameObject samplepoint = Instantiate (CoreSamplePoints, pointPos, Quaternion.identity) as GameObject;
								samplepoint.GetComponentInChildren<TextMesh> ().text = DrillControlNew.instance.samplecollected + "";
								CoreSample sample = new CoreSample (CheckClawCollider.layerProperty.name, pointPos);
								DataManager.instance.AddCollectedSamples (sample);
						}
						else
						if(!DrillControlNew.instance.check_layer)
						{
							Vector3 pointPos = new Vector3 (transform.position.x, transform.position.y , transform.position.z);
							GameObject samplepoint = Instantiate (CoreSamplePoints, pointPos, Quaternion.identity) as GameObject;
							samplepoint.GetComponentInChildren<TextMesh> ().text = DrillControlNew.instance.samplecollected + "";
							//LayerProperty property = DataManager.instance.GetLayerProperty (CheckClawCollider.layerProperty.name);
							//property.type=LayerType.None;
							CoreSample sample = new CoreSample ("None", pointPos);
							DataManager.instance.AddCollectedSamples (sample);
						}
				}

	
	}

}
