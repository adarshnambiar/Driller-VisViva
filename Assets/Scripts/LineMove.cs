using UnityEngine;
using System.Collections;

public class LineMove : MonoBehaviour {
	public GameObject horizontalline;
	//public GameObject verticalline;
	public GameObject Drilly;
	public float groundlevel=4.81f;
	public float bottomlevel=-6.04f;

	// Use this for initialization
	void Start () {
		//movepos = TrailObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	

		//verticalline.transform.position = new Vector3 (TrailObject.transform.position.x, -4.81f, 0);
		//horizontalline.transform.position = new Vector3 (0,TrailObject.transform.position.y, 0);
		float height = (horizontalline.transform.position.y - bottomlevel) / Mathf.Abs ((groundlevel - bottomlevel));
		DepthMeterCtrl.instance.SetScaledDepth((1f-height)-0.038f);

	}
}
