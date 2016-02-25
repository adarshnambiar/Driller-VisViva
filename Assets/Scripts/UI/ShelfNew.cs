using UnityEngine;
using System.Collections;

public class ShelfNew : MonoBehaviour {

	private static int numRocks=5;
	private GameObject[] rocks = new GameObject[numRocks];
	private GameObject[] labels = new GameObject[numRocks];
	private int rockCount=0;

	public Sprite rockLimestone;
	public Sprite rockSandstone;
	public Sprite rockConglomerate;
	public Sprite rockShale;
	public Sprite rockSiltstone;
	public Sprite rockClay;
	public Sprite rockNone;
	public Sprite rockX;
	
	// Use this for initialization
	void Start () {
		// init rocks and names
		for (int i=0; i<numRocks; i++)
		{
			string rockName = "Rock" + (i+1).ToString ();
			Debug.Log (rockName);
			GameObject currentRock = GameObject.Find (rockName);
			rocks[i] = currentRock;

			string textName = "Text" + (i+1).ToString ();
			GameObject currentLabel = GameObject.Find (textName);
			currentLabel.SetActive (false);
			labels[i] = currentLabel;
		}
	}
	
	public void AddJar(string layerName){
		Debug.Log ("ranshelf");
		Debug.Log (rocks.Length);
		if (rockCount<rocks.Length)
		{
			RevealRock(layerName);
			RevealText(layerName);
			rockCount++;

		}
		
	}

	private void RevealRock(string layerName)
	{
		GameObject currentRock = rocks[rockCount];
		Sprite currentSprite = GetSpriteByName(layerName);
		SpriteRenderer currentRenderer = currentRock.GetComponent<SpriteRenderer>();
		currentRenderer.sprite = currentSprite;
		currentRenderer.enabled = true;
		
	}

	private void RevealText(string layerName)
	{
		GameObject currentLabel = labels[rockCount];
		TextMesh currentText = currentLabel.GetComponent<TextMesh>();
		currentText.text = layerName;
		currentLabel.SetActive (true);

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
		else if (name == "Not Revealed")
		{
			return rockX;
		}
		else
		{
			return rockNone;
		}
		
	}
}
