using UnityEngine;
using System.Collections;

public class RockPopup : MonoBehaviour {
	public GameObject rock; // rock prefab
	private GameObject clone; // stores each instatiated rock prefabs
	private int rockCount = 0; // rocks sampled so far
	DrillControlNew drilltrackback;
	public LayerProperty layer;
	public LayerType type;
	public LayerShape shape;
	public string name;
	public bool isRevealed;

	public Sprite rockLimestone;
	public Sprite rockSandstone;
	public Sprite rockConglomerate;
	public Sprite rockShale;
	public Sprite rockSiltstone;
	public Sprite rockClay;
	public Sprite rockNone;
	public Sprite rockX;
	public static bool shelfupdated;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool Popups(LayerProperty layerProperty) {
		// get layer info
		layer = layerProperty;
		type = layer.type;
		name = layer.gameObject.name;
		shape = DataManager.instance.GetLayerShape (name);
		shelfupdated = false;//to check if the shelf is updated.
		// check if layer is revealed
		 isRevealed = checkRevealed(name);

		if (isRevealed) 
		{

			// current layer
			Invoke("Wait",4);
			rockCount ++;
			return true;
		} 

		else 
		{
			Debug.Log("print name is");
			Debug.Log (layer.GetPrintName());
			Invoke("WaitNotRevealed",4);
			rockCount ++;
			return false;
		}
	}

	// check if current layer is revealed. can only sample revealed layers.
	private bool checkRevealed(string name)
	{

		//Debug.Log ("CHECK LAYER:" + name);
		object[] layers = DataManager.instance.GetRevealedLayers ();
		for (int i=0; i<layers.Length; i++) {
			if (layers [i].ToString().Contains(name.Trim())) {
				Debug.Log ("MATCH   :" +layers [i].ToString());
				return true;
			}
		}
		return false;

	}

	// wait, move rock, destroy rock, add to shelf
	private IEnumerator MoveRock(int seconds, GameObject clone, bool revealed) {
		yield return new WaitForSeconds(seconds); // leave rock on screen before moving it to shelf
		string jarName = "Jar" + rockCount.ToString (); // name of jar we're trying to move to
		float offset = 1.05f; // need offset to make it look right
		Vector3 endPosition = GameObject.Find (jarName).transform.position * offset; // target location
		Vector3 endScale = new Vector3 (0.0f, 0.0f, 0f); // will shrink to 0
		float timeToMove = 2f;
		float timeElapsed = 0f;
		float speed = 1.5f;
		while (timeElapsed<timeToMove)
		{
			clone.transform.position = Vector3.Lerp(clone.transform.position, endPosition, Time.deltaTime*speed);
			clone.transform.localScale = Vector3.Lerp(clone.transform.localScale, endScale, Time.deltaTime*speed);
			timeElapsed+=Time.deltaTime;
			DrillControlNew.instance.drillbacktrack = true; // double check with adarsh
			yield return new WaitForEndOfFrame();
		}
		Destroy (clone);
		UpdateShelf(clone, revealed);
		shelfupdated = true;
	}

	private void UpdateShelf(GameObject clone, bool revealed)
	{
		ShelfNew shelf;
		shelf = GameObject.Find ("ShelfNew").GetComponent<ShelfNew>();
		string layerName = "blank";
		if (revealed)
		{
			layerName = layer.GetPrintName ();
		}
		else
		{
			layerName = "Not Revealed";
		}
		shelf.AddJar(layerName);
		SoundFXCtrl.instance.PlaySound (8, 1);
	}


	// called if unrevealed layer is sampled
	void WaitNotRevealed()
	{
		clone = Instantiate (rock) as GameObject;
		TextMesh text = clone.GetComponentInChildren<TextMesh>();
		text.text = "Not Revealed";
		Sprite currentSprite = rockX;
		SpriteRenderer thisSprite = clone.GetComponentInChildren<SpriteRenderer>();
		thisSprite.sprite = currentSprite;
		StartCoroutine(MoveRock(3, clone, false));

	}

	void Wait(){

		clone = Instantiate (rock) as GameObject; // instantiate prefab
		float prob=layer.GetPossibility ();

		if (prob > 0f && prob <= 30.0f) {	
			SoundFXCtrl.instance.PlaySound (14, 1);
		} else if (prob > 30.0f && prob <= 70.0f) {
			SoundFXCtrl.instance.PlaySound (10, 1);
		} else if (prob > 70.0f) {
			SoundFXCtrl.instance.PlaySound (9, 1);
		}

	
		TextMesh text = clone.GetComponentInChildren<TextMesh>(); // get textmesh component
		text.text = layer.GetPrintName (); // set text to printable layer name
		Sprite currentSprite = GetSpriteByName(layer.GetPrintName ());
		SpriteRenderer thisSprite = clone.GetComponentInChildren<SpriteRenderer>();
		thisSprite.sprite = currentSprite;
		//clone.GetComponentInChildren<SpriteRenderer> ().color = layerColor; // set color of rock
		Color layerColor = DataManager.instance.GetColorOfType (type); // color of current layer
		//LayerColorMask.instance.ChangeLayerColor (layer.name, layerColor);//Change Layer Color
		LayerColorMask.instance.ChangeLayerTexture (layer.name);//Change Layer Texture
		PlayAudioFeedback (shape);
		StartCoroutine(MoveRock(3, clone, true)); // sequence of events to move rock to shelf
		isRevealed = false; // turn off for next time
	}

	private Sprite GetSpriteByName(string name)
	{
		if (name == "Limestone")
		{
			return rockLimestone;
		}
		else if (name == "Sandstone")
		{
			return rockSandstone;
		}
		else if (name == "Conglomerate")
		{
			return rockConglomerate;
		}
		else if (name == "Shale")
		{
			return rockShale;
		}
		else if (name == "Siltstone")
		{
			return rockSiltstone;
		}
		else if (name == "Clay")
		{
			return rockClay;
		}
		else
		{
			return rockNone;
		}

	}

	private void PlayAudioFeedback(LayerShape layershape){

		switch (layershape) {
	
		case LayerShape.AnticlineTrap:
			print ("LayerShape.AnticlineTrap");
			GeologistUICtrl.instance.PlayVoice (1);
			GeologistUICtrl.instance.DisplayDialogNow(1);
			break;
		case LayerShape.CapRock:
			print ("LayerShape.CapRock");
			GeologistUICtrl.instance.PlayVoice (0);
			GeologistUICtrl.instance.DisplayDialogNow(0);
			break;
		case LayerShape.FaultTrap:
			print ("LayerShape.FaultTrap");
			GeologistUICtrl.instance.PlayVoice (2);
			GeologistUICtrl.instance.DisplayDialogNow(2);
			break;
		case LayerShape.StratagraphicTrap:
			print ("LayerShape.StratagraphicTrap");
			GeologistUICtrl.instance.PlayVoice (3);
			GeologistUICtrl.instance.DisplayDialogNow(3);
			break;
		default:
			print ("LayerShape.Normal");
			GeologistUICtrl.instance.PlayVoice (4);
			GeologistUICtrl.instance.DisplayDialogNow(4);
			break;
				}

	}

}