using UnityEngine;
using System.Collections;

public class TextFeedback : MonoBehaviour {
	public float tranSpeed = 1;
	public float distance = 1;
	
	public string SortingLayerName = "UI";
	public int SortingLayerOrder = 0;

	public Color badColor = Color.white;
	public Color goodColor = Color.yellow;
	public Color excellentColor = Color.yellow;

	TextMesh textMesh;
	Transform _transform;
	
	Color fromColor;
	Color toColor;
	Vector3 fromPosition;
	Vector3 toPosition;
	
	float tranProcess;
	
	// Use this for initialization
	void Start () {
		_transform = transform;


		fromPosition = _transform.position;
		toPosition = new Vector3 (_transform.position.x, _transform.position.y + distance, 0);
		tranProcess = 0;
		
		renderer.sortingLayerName = SortingLayerName;
		renderer.sortingOrder = SortingLayerOrder;
	}
	
	// Update is called once per frame
	void Update () {

		if (PlanPathManager.instance.GetStage () == PlanStage.Drilling) {
			if (tranProcess <= 1) {
				tranProcess += Time.deltaTime * tranSpeed;
				if (tranProcess > 1) {
					Destroy (gameObject);
				}
			}
			textMesh.color = Color.Lerp (fromColor, toColor, tranProcess);
			_transform.position = Vector3.Lerp (fromPosition, toPosition, tranProcess);
		}

	}

	public void SetText(int type, float energyIncrement){
		textMesh = GetComponent<TextMesh> ();

		switch (type) {
		case 0:
			textMesh.text = "+0 kWh";
			textMesh.color = badColor;
			break;
		case 1:
			textMesh.text = "+"+(int)energyIncrement+"kWh";
			textMesh.color = goodColor;
			break;
		case 2:
			textMesh.text = "+"+(int)energyIncrement+"kWh";
			textMesh.color = excellentColor;
			break;
		}

		fromColor = textMesh.color;
		toColor = new Color (fromColor.r, fromColor.g, fromColor.b, 0);
	}



}
