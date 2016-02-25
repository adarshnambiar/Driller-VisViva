using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	public float flashLength = 3;
	public float transitSpeed = 1;
	public Color orgColor;

	bool lerpDir = false;
	SpriteRenderer sprenderer;

	// Use this for initialization
	void Start () {

		sprenderer = GetComponent<SpriteRenderer> ();
		sprenderer.color = orgColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (flashLength > 0) {
			Color c = sprenderer.color;
			if (lerpDir) {
				c.a = Mathf.MoveTowards (c.a, 0, transitSpeed * Time.deltaTime);
				if (c.a == 0) {
					lerpDir = !lerpDir;
					flashLength--;
				}
			} else {
				c.a = Mathf.MoveTowards (c.a, orgColor.a, transitSpeed * Time.deltaTime);
				if (c.a == orgColor.a) {
					lerpDir = !lerpDir;
					flashLength--;
				}
			}
			sprenderer.color = c;
		} else {
			Destroy(gameObject);
		}
	}


}
