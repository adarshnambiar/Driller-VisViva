using UnityEngine;
using System.Collections;

public class RespondWave: MonoBehaviour {

	// It's jittery and I don't know why.

	public float soundSpeed;
	public Color wavecolor;
	public float fadeOutSpeed = 1;
	public string layername;	//for detecting and revealing
	//public int type; //the type of the sound, 0-green, 1- red, 2-blue, 3-yellow

	bool waitForDestroy;
	SpriteRenderer spRenderer;
	Color spColor;

	// Use this for initialization
	void Start () {
		MoveSound();
		waitForDestroy = false;
		spRenderer = GetComponentInChildren<SpriteRenderer> ();
		spRenderer.color = wavecolor;
		spColor = spRenderer.color;
		FadeOut ();
	}

	void Update(){
		if (waitForDestroy) {
			spColor.a = Mathf.MoveTowards(spColor.a, 0, fadeOutSpeed*Time.deltaTime);
			if(spColor.a==0){
				//SoundCatcher.instance.DeleteFromList(gameObject);
				GeoGameManager.instance.RespondCatch (layername);
				Destroy(gameObject);
			}
			spRenderer.color = spColor;
		}
	}

	void MoveSound()
	{
		rigidbody2D.velocity = transform.up*soundSpeed;
	}

	void OnTriggerEnter2D(Collider2D other){
		/*
		if (other.tag == "boundaryGround") {
			FadeOut ();
		} else */ 
		if (other.tag == "SoundCatcher") {
			//SoundCatcher.instance.AddToList(gameObject);
			GeoGameManager.instance.RespondCatch (layername);
			RemoveNow();
		}
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.tag == "SoundCatcher") {
			//SoundCatcher.instance.DeleteFromList(gameObject);
		}
	}

	public void FadeOut(){
		waitForDestroy = true;
	}

	public void RemoveNow(){
		waitForDestroy = true;
		fadeOutSpeed *= 2;
	}

}
