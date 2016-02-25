using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Shelf3 : MonoBehaviour {
	private object[] samples;
	private LayerProperty[] layers = new LayerProperty[5];
	private bool[] isNone = new bool[5]; // will store whether layer is None or not.
	//private Color[] colors = new Color[5];
	private int size; // number of samples collected
	private int index=0;
	private Image[] jars = new Image[5];
	private Image[] smallRocks = new Image[5];
	private Text[] labels = new Text[5];
	private GameObject orgLayer;
	private LayerProperty layer;
	private LayerType type;
	private LayerType noneType = LayerType.None;
	private Text name;
	private Text permeability;
	private Text density;
	private Text probability;
	private Image rock; // big rock in jar
	private Text number;
	public GameObject notRevealedCard;

	public Sprite normalJar;
	public Sprite missingJar;

	public Sprite rockLimestone;
	public Sprite rockSandstone;
	public Sprite rockConglomerate;
	public Sprite rockShale;
	public Sprite rockSiltstone;
	public Sprite rockClay;
	public Sprite rockX;
	public Sprite rockNone;


	void Start()
	{
		GameObject nameObject = GameObject.Find("Layer Name Info");
		name = nameObject.GetComponent<Text>();
		GameObject permeabilityObject = GameObject.Find("Permeability Info");
		permeability = permeabilityObject.GetComponent<Text>();
		GameObject densityObject = GameObject.Find("Density Info");
		density = densityObject.GetComponent<Text>();
		GameObject probObject = GameObject.Find ("Probability Info");
		probability = probObject.GetComponent<Text>();

		GameObject rockObject = GameObject.Find ("Rock"); // big rock in jar
		rock = rockObject.GetComponent<Image>(); // big rock in jar
		GameObject numberObject = GameObject.Find ("Sample Number");
		number = numberObject.GetComponent<Text>();
	}

	void Update()
	{
		if (Input.GetButtonDown("Shelf_Right"))
		{
			if (size>0)
			{
				jars[index].sprite = normalJar;
				smallRocks[index].enabled=true;
				index = (index+1)%(size);
				jars[index].sprite = missingJar;
				smallRocks[index].enabled=false;
				//rock.color = jars[index+1].color;
				LoadCardInfo(index);
			}


		}
		else if (Input.GetButtonDown ("Shelf_Left"))
		{
			if (size>0)
			{
				jars[index].sprite = normalJar;
				smallRocks[index].enabled=true;
				index = (index-1+size)%size;
				jars[index].sprite = missingJar;
				smallRocks[index].enabled=false;
				//rock.color = jars[index+1].color;
				LoadCardInfo(index);
			}
		}
	}

	// Use this for initialization
	public void InitJars () {
		samples = DataManager.instance.GetCollectedSamples();
		size = samples.Length;
		Debug.Log ("size");
		Debug.Log(size);
		if (size>0)
		{
			GameObject emptyCard = GameObject.Find ("Empty Info");
			emptyCard.SetActive(false); // cannot find inactive game objects - must handle here.
			notRevealedCard = GameObject.Find ("Not Revealed Info");
			notRevealedCard.SetActive (false);
			//jars = gameObject.GetComponentsInChildren<Image>();
			for(int i=0; i<size; i++)
			{
				// get layers
				string layerName = ((CoreSample)samples[i]).layername;
				string printName = "";
				if (layerName == "None")
				{
					printName = "Not Revealed";
					isNone[i] = true;
				}
				else
				{
					orgLayer = GameObject.Find (layerName);
					layer = orgLayer.GetComponent<LayerProperty> ();
					type = layer.type;
					printName = layer.GetPrintName ();
					isNone[i] = false;
				}
				layers[i] = layer;

				// add correct number of jars to list and enable them
				GameObject thisJar = GameObject.Find ("Jar" + (i+1).ToString ());
				Image thisJarImage = thisJar.GetComponent<Image>();
				thisJarImage.enabled = true;
				jars[i] = thisJarImage;

				// add correct number of rocks to list, enable them, and set to right texture
				GameObject thisRock = GameObject.Find ("Rock" + (i+1).ToString ());
				Image thisRockImage = thisRock.GetComponent<Image>();
				thisRockImage.sprite = GetSpriteByName(printName);
				thisRockImage.enabled = true;
				smallRocks[i] = thisRockImage;

				// add correct text to list, enable them, set to right name
				GameObject thisText = GameObject.Find ("Text" + (i+1).ToString ());
				Text thisTextText = thisText.GetComponent<Text>();
				thisTextText.text = printName;
				thisTextText.enabled = true;
				labels[i] = thisTextText;

				// old stuff
				//Color color = DataManager.instance.GetColorOfLayerName (layerName);
				//jars[i+1].enabled = true; // jars has shelf first so index is 1 up
				//jars[i+1].color = color; // might be able to just do this using layer properties. check later.
				
				//colors[i] = color; 
			}
			//jars[index+1].enabled=false; // jars[0] is the shelf
			jars[0].sprite = missingJar;
			smallRocks[0].enabled = false;
			//rock.color = jars[index+1].color; // make rock correct color
			LoadCardInfo(0);
		}

		else
		{
			rock.enabled = false; // no rock if no samples
			GameObject regCard = GameObject.Find ("Info");
			regCard.SetActive(false);
		}
	}

	private void LoadCardInfo(int i)
	{
		if (layers[i]!=null) // just to be safe
		{
			notRevealedCard.SetActive (false);
			LayerProperty current = layers[i];
			//LayerType currentType = current.type;
			String layerName = current.GetPrintName ();
			if (isNone[i])
			{
				// swap to other card
				notRevealedCard.SetActive (true);
				String printName = "Not Revealed";
				rock.sprite = GetSpriteByName(printName);
				number.text = (i+1).ToString ();
			}
			else
			{
			name.text = current.GetPrintName();
			permeability.text = current.GetPorosity().ToString () + " %";
			density.text = current.GetDensity().ToString () + " kg/m^3";
			probability.text = String.Format("{0:F0}", current.GetPossibility()) + " %"; // format to chop off extra decimals
			number.text = (i+1).ToString();
			rock.sprite = GetSpriteByName(current.GetPrintName());
			}
		}
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
