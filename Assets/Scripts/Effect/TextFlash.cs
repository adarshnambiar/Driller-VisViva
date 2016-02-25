using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFlash : MonoBehaviour {

	public float speed = 1;

	Text textCom;
	Color textCol;
	// Use this for initialization
	void Start () {
		textCom = GetComponent<Text> ();
		textCol = textCom.color;
	}
	
	// Update is called once per frame
	void Update () {
		textCol.a = Mathf.PingPong (Time.time * speed, 1);
		textCom.color = textCol;
	}
}
