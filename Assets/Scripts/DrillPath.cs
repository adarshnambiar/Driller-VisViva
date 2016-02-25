using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrillPath : MonoBehaviour
{
	List<Vector3> linePoints = new List<Vector3>();
	LineRenderer lineRenderer;
	public float startWidth = 1.0f;
	public float endWidth = 1.0f;
	public float threshold = 0.001f;
	Camera thisCamera;
	int lineCount = 0;
	Vector3 pathTarget;
	Vector3 direction;	

	public Transform character;
	public float speed = 1;

	int currentIndex = 0;
	Vector3 currentTarget;

	Vector3 lastPos = Vector3.one * float.MaxValue;
	
	
	void Awake()
	{
		thisCamera = Camera.main;
		lineRenderer = GetComponent<LineRenderer>();
		direction = Vector3.up;

		currentIndex = 0;
		currentTarget = Vector3.zero;
	}
	
	void Update()
	{
		if (Input.GetKey (KeyCode.LeftShift)) {
						if (Input.GetMouseButton (0)) {
								Vector3 mousePos = Input.mousePosition;
								mousePos.z = thisCamera.nearClipPlane + 0.1f;
								Vector3 mouseWorld = thisCamera.ScreenToWorldPoint (mousePos);
								float dist = Vector3.Distance (lastPos, mouseWorld);
								if (dist <= threshold)
										return;
								lastPos = mouseWorld;
								if (linePoints == null)
								linePoints = new List<Vector3> ();
								mouseWorld.z = -0.5f;
								linePoints.Add (mouseWorld);
								UpdateLine ();
						}
				}


		if(Input.GetButton("Vertical")){

			float step=0.5f*Time.deltaTime;
			Debug.Log(linePoints.Count);
			Debug.Log (linePoints[0]);
			for(int i = 0; i < linePoints.Count; i++)
			{

				pathTarget=linePoints[i];
				direction = transform.position - pathTarget;
				transform.position=Vector3.MoveTowards (transform.position, pathTarget,step);
			}
				direction = direction.normalized;
		}
	
		if (Input.GetButton ("Jump")) {
			currentTarget = linePoints[currentIndex];
			currentTarget.z = -3;
			character.position = Vector3.MoveTowards(character.position, currentTarget, speed * Time.deltaTime);
			Vector3 dir = currentTarget - character.position;
			//dir.z = 0;
			//character.up = -dir;
			character.up = Vector3.RotateTowards(character.up, -dir, 1f*Time.deltaTime, 0);
			Debug.Log(dir);
			if(character.position == currentTarget&&currentIndex<linePoints.Count){
				currentIndex++;
			}
		}

	}

	void UpdateLine()
	{
		lineRenderer.SetWidth(startWidth, endWidth);
		lineRenderer.SetVertexCount(linePoints.Count);
		
		for(int i = lineCount; i < linePoints.Count; i++)
		{
			lineRenderer.SetPosition(i, linePoints[i]);
		
		}
		lineCount = linePoints.Count;
	}
}