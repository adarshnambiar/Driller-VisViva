using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour {

	public float speed = 1;
	public GameObject callbackGO;

	CanvasGroup target;
	int status = 0;	//0 - empty; 1 - fade in; 2 - fade out

	// Use this for initialization
	void Awake () {

		target = GetComponent<CanvasGroup> ();

	}
	
	// Update is called once per frame
	void Update () {
		switch(status){
		case 1:
			if(target.alpha < 1){
				target.alpha = Mathf.MoveTowards(target.alpha, 1, speed * Time.unscaledDeltaTime);
				if(target.alpha >= 1){
					target.alpha = 1;
					status = 0;
					if(callbackGO!=null){
						callbackGO.SendMessage("FadeInFinish", gameObject);
					}
				}
			}
			break;
		case 2:
			if(target.alpha > 0){
				target.alpha = Mathf.MoveTowards(target.alpha, 0, speed * Time.unscaledDeltaTime);
				if(target.alpha <= 0){
					target.alpha = 0;
					status = 0;
					if(callbackGO!=null){
						callbackGO.SendMessage("FadeOutFinish", gameObject);
					}
				}
			}
			break;
		}
	}

	public void FadeIn(){
		gameObject.SetActive (true);
		target.interactable = true;
		target.alpha = 0;
		status = 1;
	}

	public void FadeOut(){
		target.interactable = false;
		target.alpha = 1;
		status = 2;
		//gameObject.SetActive (false);
	}

}
