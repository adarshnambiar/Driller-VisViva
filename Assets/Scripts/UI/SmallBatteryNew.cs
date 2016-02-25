using UnityEngine;
using System.Collections;

public class SmallBatteryNew : MonoBehaviour {

	public Transform FillContainer;

	SpriteRenderer fillSprite;

	// Use this for initialization
	void Awake () {
		fillSprite = FillContainer.GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetFillLevel(float scale){
		Vector3 fillScale = FillContainer.localScale;
		fillScale.y = Mathf.Clamp01 (scale);
		FillContainer.localScale = fillScale;
		fillSprite.color = Color.Lerp (Color.red, Color.green, fillScale.y);
	}
}
