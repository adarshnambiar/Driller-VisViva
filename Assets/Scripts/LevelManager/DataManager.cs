using UnityEngine;
using System.Collections;

public enum LayerType{
	SoftLayer,				//deprecated type
	HardLayer,				//deprecated type
	Oil,					//deprecated type
	Limestone,
	Sandstone,
	Conglomerate,
	Shale,
	Siltstone,
	Clay,
	None,
};

public enum LayerShape{
	Normal,
	FaultTrap,
	AnticlineTrap,
	StratagraphicTrap,
	CapRock,
};

public class CoreSample{
	public string layername;
	public Vector3 position;

	public CoreSample(string layername, Vector3 position){
		this.layername = layername;
		this.position = position;
	}
}

public class DataManager : MonoBehaviour {

	public static DataManager instance;

	public string SelectedLevel;		//current selected level(layer map)
	public Color[] ColorsForLayers;		//color pool for different layers
	public Texture[] TextureForLayers;	//texture for different layer types: 0 - limestone, 1 - sandstone, 2 - conglomerate, 3 - shale, 4 - siltstone, 5 - clay
	public Color RevealDefaultColor;	//color for default revealed color
	public float score;					//the score of player
	public LayerMask layerOnlyMask;

	Hashtable LayerColorTable;
	ArrayList RevealedLayers;			//the revealed layers in stage 1
	ArrayList CollectedSamples;			//the collected samples from stage 2
	float totalScore;					//the total score of current level
	float avgScore;						//the average score a layer may contains

	void Awake(){
		if (DataManager.instance == null) {
			DataManager.instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy(gameObject);
		}
		score = 0;
		CollectedSamples = new ArrayList ();
		LayerColorTable = new Hashtable ();
		RevealedLayers = new ArrayList ();
		FillLayerColorTable ();
	}

	// Use this for initialization
	void Start () {

	}

	void Update () {
		//for loging debug information
		if (Input.GetKeyDown (KeyCode.F2)) {
			object[] layernames = GetRevealedLayers();
			foreach(string name in layernames){
				Debug.Log(name);
			}
		}
	}

	//return the color of a layer type
	public Color GetColorOfType(LayerType type){
		return (Color)LayerColorTable[type];
	}

	//return the color of a layer - according to name of layer
	public Color GetColorOfLayerName(string layername){
		GameObject orgLayer = GameObject.Find ("/Layers/" + layername);
		if (orgLayer != null)
			return GetColorOfType (orgLayer.GetComponent<LayerProperty> ().type);
		else
			return Color.white;
	}

	//add revealed layer to list, if layer has already been revealed, return false
	public bool AddRevealedLayer(string layername){
		if (RevealedLayers.Contains (layername)) {
			//Debug.Log("duplicated");
			return false;
		} else {
			RevealedLayers.Add (layername);
			//Debug.Log ("new revealed");
			return true;
		}
	}

	//get all reveled layer names
	public object[] GetRevealedLayers(){
		return RevealedLayers.ToArray ();
	}

	//if a layer has been revealed
	public bool IfLayerRevealed(string name){
		return RevealedLayers.Contains (name);
	}

	//reset the data manager
	public void Reset(){
		score = 0;
		CollectedSamples.Clear ();
		RevealedLayers.Clear ();
		LayerColorTable.Clear ();
		FillLayerColorTable ();
	}

	void FillLayerColorTable(){
		//randomize the colors
		for (int i = 0; i<ColorsForLayers.Length; i++) {
			int r = Random.Range(0, ColorsForLayers.Length);
			Color temp = ColorsForLayers[i];
			ColorsForLayers[i] = ColorsForLayers[r];
			ColorsForLayers[r] = temp;
		}
		LayerColorTable.Add (LayerType.HardLayer, ColorsForLayers[0]);
		LayerColorTable.Add (LayerType.SoftLayer, ColorsForLayers[1]);
		LayerColorTable.Add (LayerType.Oil, ColorsForLayers[2]);

		LayerColorTable.Add (LayerType.Limestone, ColorsForLayers[3]);
		LayerColorTable.Add (LayerType.Sandstone, ColorsForLayers[4]);
		LayerColorTable.Add (LayerType.Conglomerate, ColorsForLayers[5]);
		LayerColorTable.Add (LayerType.Shale, ColorsForLayers[6]);
		LayerColorTable.Add (LayerType.Siltstone, ColorsForLayers[7]);
		LayerColorTable.Add (LayerType.Clay, ColorsForLayers[8]);
	}

	//add to collected samples
	public void AddCollectedSamples(CoreSample sample){
		CollectedSamples.Add (sample);
	}

	public object[] GetCollectedSamples(){
		return CollectedSamples.ToArray ();
	}

	public void AddToScore(float amount){
		score += amount;
	}

	public float GetCurrentScore(){
		return score;
	}

	public void SetTotalScore (float tscore){
		totalScore = tscore;
	}

	public float GetTotalScore(){
		return totalScore;
	}

	public void SetAvgScore(float ascore){
		avgScore = ascore;
	}

	public float GetAvgScore(){
		return avgScore;
	}

	public LayerProperty GetLayerProperty(string layername){
		GameObject orgLayer = GameObject.Find ("/Layers/" + layername);
		if (orgLayer != null)
			return orgLayer.GetComponent<LayerProperty>();
		else
			return null;
	}

	public LayerShape GetLayerShape(string layername){
		GameObject orgLayer = GameObject.Find ("/Layers/" + layername);
		if (orgLayer != null)
			return orgLayer.GetComponent<LayerProperty> ().GetLayerShape ();
		else
			return LayerShape.Normal;
	}

	public Color GetRevealDefaultColor(){
		return RevealDefaultColor;
	}

	//texture for different layer types: 0 - limestone, 1 - sandstone, 2 - conglomerate, 3 - shale, 4 - siltstone, 5 - clay
	public Texture GetTextureOfType(LayerType type){
		switch (type) {
		case LayerType.Limestone:
			return TextureForLayers[0];
			break;
		case LayerType.Sandstone:
			return TextureForLayers[1];
			break;
		case LayerType.Conglomerate:
			return TextureForLayers[2];
			break;
		case LayerType.Shale:
			return TextureForLayers[3];
			break;
		case LayerType.Siltstone:
			return TextureForLayers[4];
			break;
		case LayerType.Clay:
			return TextureForLayers[5];
			break;
		default:
			return null;
			break;
		}
	}

	public Texture GetTextureOfLayerName(string layername){
		LayerProperty property = GetLayerProperty (layername);
		if (property != null)
			return GetTextureOfType (property.type);
		else
			return null;
	}

	public bool CheckLayerReveal(string name){
		return RevealedLayers.Contains (name);
	}

	public string DetectLayerAtPosition(Vector3 position){
		RaycastHit2D hit = Physics2D.Raycast (position, Vector3.up, Mathf.Infinity, layerOnlyMask);
		if (hit != null) {
			return hit.collider.name;
		} else {
			return "";
		}
	}

	public bool LayerHasBeenSampled(string name){
		foreach (CoreSample sample in CollectedSamples) {
			if(sample.layername == name){
				return true;
			}
		}
		return false;
	}

}



