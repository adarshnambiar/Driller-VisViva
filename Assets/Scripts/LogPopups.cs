using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogPopups : MonoBehaviour {
	public static LogPopups instance;
	private LayerType type;
	public GameObject shale; // shale card
	public GameObject soft; // soft card
	public GameObject hard; // hard card
	private Color layerColor; // color to change to
	private float spawnX = 189.0f;
	private float spawnY = 302.5f;
	private float openTime = 3f;
	private LayerProperty layer;
	private bool open = false; // used to detect if a popup is currently on screen
	private float spawnTime = 0f; // time popup is spawned at
	private float currentTime;
	private GameObject clone;
	DrillControlNew drilltrackback;
	private Vector3 endPosition;
	private Vector3 endScale;
	private float speed = 1.5f;
	private string name;
	private bool isRevealed;

	void Start()
	{
		instance = this;
		endPosition = new Vector3(3f, 6.5f, 0f); // approximate location of data log
		endScale = new Vector3 (0f, 0f, 0f);
		isRevealed = false;
	}

	void Update()
	{
		currentTime = Time.time;
		if (open == true) {
						if (currentTime - spawnTime >= openTime) {
								if (currentTime - spawnTime >= 5f) {
										Destroy (clone);
										open = false;
								} else {	
										MovePopup ();
										ShrinkPopup ();
								}
						}
				}
	}

	void MovePopup()
	{
		clone.transform.position = Vector3.Lerp(clone.transform.position, endPosition, Time.deltaTime*speed);
	}

	void ShrinkPopup()
	{
		clone.transform.localScale = Vector3.Lerp(clone.transform.localScale, endScale, Time.deltaTime*speed);
		DrillControlNew.instance.drillbacktrack = true;
	}


	public bool Popups(LayerProperty layerProperty) {
		layer = layerProperty;
		type = layer.type;
		name = layer.gameObject.name;
		object[] layers = DataManager.instance.GetRevealedLayers (); // can redo this just using the is revealed function
		for (int i=0; i<layers.Length; i++) {
				if (layers [i].ToString() == name.Trim()) {
						isRevealed = true;
				}
		}

		if (isRevealed) {
			//Score.score -= 100;
			// DataManager.instance.AddCollectedSamples(name); Adarsh has this already
			spawnTime = Time.time;

			// deal with sampling while popup already open
			if (open == true) {
					Destroy (clone);
			} else {
					open = true;
			}
			layerColor = DataManager.instance.GetColorOfType (type);
			if (type == LayerType.Oil) {
					clone = Instantiate (shale) as GameObject;
			} else if (type == LayerType.SoftLayer) {
					clone = Instantiate (soft) as GameObject;
			} else if (type == LayerType.HardLayer) {
					clone = Instantiate (hard) as GameObject;
			}

			if (DataManager.instance.IfLayerRevealed (name) == true) {
					clone.GetComponentInChildren<SpriteRenderer> ().color = layerColor;
			}
			isRevealed = false;
			return true;
		} else {
			Debug.Log ("Too Bad!!!!Not Revealed Layer");
			return false;
		}
		}

}
