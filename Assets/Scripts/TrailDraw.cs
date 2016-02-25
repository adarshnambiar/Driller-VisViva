using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailDraw : MonoBehaviour {
	
	public static List<Vector3> linePoints;
	LineRenderer linerenderer;	
	public static TrailDraw trail;
	public static ArrayList backRoadPoint;
	//public GameObject TrailObject;
	// Use this for initialization
	void Start () {
		linePoints = new List<Vector3>();
		backRoadPoint= new ArrayList();
		linerenderer=gameObject.GetComponent<LineRenderer>();
		trail =gameObject.GetComponent<TrailDraw>() ;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (DrillControlNew.instance.drillforward && !DrillControlNew.instance.movehorizontal &&
		    DrillControlNew.instance.vertical>0 && !DrillControlNew.instance.traildrawn && DrillControlNew.instance.startgame) {
			if(transform.parent.position.x!=0f && transform.parent.position.y!=0f && transform.parent.position.z!=0f){
				linePoints.Add (transform.parent.position);
				backRoadPoint.Add(transform.parent.parent.position);
				linerenderer.SetVertexCount (linePoints.Count);
				linerenderer.SetPosition (linePoints.Count - 1, transform.parent.position);
			}


		} else if (DrillControlNew.instance.drillbacktrack &&DrillControlNew.instance.movehorizontal && !DrillControlNew.instance.istrail) {
			trail.enabled=false;
		}
	}


//	IEnumerator CheckforGameStart(float waitTime){
//
//		if (DrillControlNew.instance.drilliswaiting) {
//			yield return new WaitForSeconds(waitTime);
//			DrillControlNew.instance.startgame=true;
//			DrillControlNew.instance.drilliswaiting=false;
//		}
//	}
}
