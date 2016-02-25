using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SamplesCollected : MonoBehaviour {

	Text sampleText;
	public int collected = 0;

	// Use this for initialization
	void Start () {
		sampleText = gameObject.GetComponent<Text>();
		sampleText.text = "Samples Collected : " + string.Format("{0:0}", collected);
		}

	// Update is called once per frame
	void Update () {
		sampleText.text = "Samples Collected : " + string.Format("{0:0}", collected);
	
	}
}
