using UnityEngine;
using System.Collections;

public class StarBlink : MonoBehaviour {

	public float BlinkDelay = 2;
	public float FadeSpeed = 2;

	SpriteRenderer starSprite;
	float nextBlink;
	int state;			//0-normal, 1-fade out, 2-fade in
	Color sprColor;

	// Use this for initialization
	void Start () {
		nextBlink = Random.Range (0f, BlinkDelay);
		starSprite = GetComponent<SpriteRenderer> ();
		state = 0;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0:
			nextBlink -= Time.deltaTime;
			if(nextBlink<0){
				state = 1;
				nextBlink = BlinkDelay;
			}
			break;
		case 1:
			sprColor = starSprite.color;
			sprColor.a = Mathf.MoveTowards(sprColor.a, 0, FadeSpeed * Time.deltaTime);
			starSprite.color = sprColor;
			if(sprColor.a == 0){
				state = 2;
			}
			break;
		case 2:
			sprColor = starSprite.color;
			sprColor.a = Mathf.MoveTowards(sprColor.a, 1, FadeSpeed * Time.deltaTime);
			starSprite.color = sprColor;
			if(sprColor.a == 1){
				state = 0;
			}
			break;
		}
	}
}
