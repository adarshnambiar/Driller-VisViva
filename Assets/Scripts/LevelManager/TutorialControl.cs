using UnityEngine;
using System.Collections;

public class TutorialControl : MonoBehaviour {

	public static TutorialControl instance;

	public GameObject[] tutorials;
	public float transitSpeed = 1;

	CanvasGroup canvasGroup;
	int transitState;		//0 - normal, 1 - fade out, 2 - fade in
	int currentIndex;
	int nextIndex;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		HideAllTutorial ();
		currentIndex = 0;
		nextIndex = 0;
		tutorials [currentIndex].SetActive (true);
		canvasGroup = GetComponent<CanvasGroup> ();
		transitState = 0;
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (transitState == 1) {
			canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, 0, transitSpeed * Time.deltaTime);
			if (canvasGroup.alpha == 0) {
				transitState = 2;
				tutorials [currentIndex].SetActive (false);
				tutorials [nextIndex].SetActive (true);
				currentIndex = nextIndex;
			}
		} else if (transitState == 2) {
			canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, 1, transitSpeed * Time.deltaTime);
			if (canvasGroup.alpha == 1) {
				transitState = 0;
			}
		} else if (transitState == 3) {
			canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, 0, transitSpeed * Time.deltaTime);
			if (canvasGroup.alpha == 0) {
				gameObject.SetActive(false);
			}
		}
	}

	void HideAllTutorial(){
		foreach (GameObject tut in tutorials) {
			tut.SetActive(false);
		}
	}

	//display last tutorial page and return true if at the beginning of tutorial
	public bool DisplayLast(out int current){
		if (currentIndex <= 0) {
			current = currentIndex;
			return true;
		} else {
			if(transitState==0){
				transitState = 1;
				nextIndex = currentIndex-1;
			}
			current = nextIndex;
			return false;
		}
	}

	public bool DisplayLast(){
		int placeHolder;
		return DisplayLast (out placeHolder);
	}

	//display next tutorial page and return true if reach the end of tutorials
	public bool DisplayNext(out int current){
		if (currentIndex >= tutorials.Length - 1) {
			current = currentIndex;
			return true;
		} else {
			if(transitState==0){
				transitState = 1;
				nextIndex = currentIndex+1;
			}
			current = nextIndex;
			return false;
		}
	}

	public bool DisplayNext(){
		int placeHolder;
		return DisplayNext (out placeHolder);
	}

	public void TotalFadeOut(){
		transitState = 3;
	}

	public int GetCurrentIndex(){
		return currentIndex;
	}

}
