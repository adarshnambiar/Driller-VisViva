using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {

	public Transform tailOffset;
	public float MaxMoveStep = 8;
	public Color activeCol;
	public Color inactCol;

	LineRenderer line;
	Transform _transform;
	Transform origin;
	
	float minLength;

	Material lineMaterial;
	float offset=0;

	SpriteRenderer crosshairSprite;

	// Use this for initialization
	void Start () {
		_transform = transform;
		origin = _transform.parent;

		minLength = (_transform.position - origin.position).magnitude;

		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (2);
		line.SetPosition (0, origin.position);
		line.sortingLayerName = "UI";
		line.sortingOrder = 1;

		lineMaterial = line.renderer.material;

		crosshairSprite = GetComponent<SpriteRenderer> ();
		crosshairSprite.color = inactCol;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void UpdatePosition(float Depth){
		Vector3 iconPos;

		if (Depth < minLength) {
			iconPos = origin.position - origin.up*minLength;
		} else {
			iconPos = origin.position - origin.up * Depth;
		}

		iconPos = Vector3.MoveTowards (_transform.position, iconPos, MaxMoveStep * Time.deltaTime);

		line.SetPosition (1, tailOffset.position);
		_transform.position = iconPos;

		float tileScale = Vector3.Distance (tailOffset.position, origin.position) / 0.5f;
		lineMaterial.SetTextureScale("_MainTex",new Vector2(tileScale,1));
		offset -= Time.deltaTime*2;
		lineMaterial.SetTextureOffset("_MainTex", new Vector2(offset,1));
		/*
		Vector3 posDelta = newPos - orgPos;
		Vector3 normalPosDelta = posDelta.normalized;
		if (posDelta.magnitude > 1.7f) {
			line.SetPosition (0, orgPos + normalPosDelta * 1.2f);
			line.SetPosition (1, newPos - normalPosDelta * 0.5f);
		} else {
			line.SetPosition(0, orgPos);
			line.SetPosition(1, orgPos);
		}
		_transform.position = newPos;*/
	}

	public void ActiveState(){
		crosshairSprite.color = activeCol;
	}

	public void NegativeState(){
		crosshairSprite.color = inactCol;
	}

}
