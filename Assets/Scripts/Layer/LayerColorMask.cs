using UnityEngine;
using System.Collections;

public class LayerColorMask : MonoBehaviour {

	public static LayerColorMask instance;
	public GameObject topLayerMask;	//the prefab for overlaping the layer

	Transform layerOnTop;
	int layerCount;
	// Use this for initialization
	void Awake () {
		layerOnTop = new GameObject ("LayerMask").transform;
		instance = this;
		layerCount = 0;
	}
	
	void LoadLevelFinish(){
		//generate a layer mask for each of the original layers
		SpriteRenderer[] spRenders = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprender in spRenders) {
			layerCount++;
			GameObject duplicate = Instantiate(topLayerMask, sprender.transform.position, Quaternion.identity) as GameObject;
			duplicate.transform.parent = layerOnTop;
			duplicate.name = sprender.gameObject.name;
			SpriteRenderer dupSprite = duplicate.GetComponent<SpriteRenderer>();
			dupSprite.sprite = sprender.sprite;
			dupSprite.sortingOrder = sprender.sortingOrder * 2 + transform.childCount;
		}
		/*
		SpriteRenderer[] spRenders = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprender in spRenders) {
			Color c = sprender.color;
			c.a = 0;
			sprender.color = c;
		}
		SpriteRenderer spRenderer = GameObject.Find ("/Layers/1").GetComponent<SpriteRenderer> ();
		if (spRenderer != null) {
			spRenderer.color = new Color(spRenderer.color.r, spRenderer.color.g, spRenderer.color.b, 0.4f);
		}*/

		//call function according to current level
		switch (Application.loadedLevelName) {
		case "Geologist":
			GeoGameManager.instance.StartIntro();
			break;
		case "Driller":
			GetLayerColor();
			DrillManager.instance.StartIntro();
			break;
		case "PlanPath":
			GetLayerColor();
			PlanPathManager.instance.StartIntro();
			Shelf3 shelf = GameObject.Find("Shelf").GetComponent<Shelf3> ();
			shelf.InitJars();

			break;
		}

	}

	//function for changing layer colors to reveal layer
	public void ChangeLayerColor(string layerName, Color newColor){
		GameObject maskLayer = GameObject.Find ("/LayerMask/"+layerName);
		if (maskLayer!=null) {
			SpriteRenderer srenderer = maskLayer.GetComponent<SpriteRenderer>();
			srenderer.material.SetColor("_Color", newColor);
			srenderer.material.SetFloat("_RevealMark", 1);
		}
	}

	public void ChangeLayerTexture(string layerName){
		GameObject maskLayer = GameObject.Find ("/LayerMask/"+layerName);
		if (maskLayer!=null) {
			SpriteRenderer srenderer = maskLayer.GetComponent<SpriteRenderer>();
			srenderer.material.SetTexture("_OverrideTex", DataManager.instance.GetTextureOfLayerName(layerName));
			srenderer.material.SetColor("_Color", Color.white);
			srenderer.material.SetFloat("_RevealMark", 2);
		}
	}

	public void GetLayerColor(){
		object[] layers = DataManager.instance.GetRevealedLayers ();
		foreach(string name in layers){
			Color n_color=DataManager.instance.GetRevealDefaultColor();
			ChangeLayerColor (name, n_color);
		}

		object[] collectedSamples = DataManager.instance.GetCollectedSamples ();
		foreach (CoreSample sample in collectedSamples) {
			//Color samplelayerCol = DataManager.instance.GetColorOfLayerName(sample.layername);
			//ChangeLayerColor(sample.layername, samplelayerCol);

			ChangeLayerTexture(sample.layername);
		}

	}

	public int GetLayerCount(){
		return layerCount;
	}
	

}
