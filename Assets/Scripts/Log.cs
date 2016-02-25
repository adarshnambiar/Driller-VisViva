using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// don't need this

public class Log : MonoBehaviour {
	public static Log instance;
	public List<GameObject> log;
	public GameObject shale;

	// Use this for initialization
	void Start () {
		instance = this;
		log = new List<GameObject>();
	}

}
