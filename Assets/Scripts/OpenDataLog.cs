using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OpenDataLog : MonoBehaviour {
	public GameObject openDataLog;
	public static OpenDataLog instance;
	public Text layerNum;
	//public SpriteRenderer oil;
	//public SpriteRenderer hard;
	//public SpriteRenderer soft;
	public Image hard;
	public Image soft;
	public Image oil;
	private bool visible = false;
	public int entryNumber = 0;
	private Image current;
	public Image right;
	public Image left;
	public Image logCover;

	// Use this for initialization
	void Awake () {
		instance=this;
	}

	void Start(){

	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void Open(){
		object[] samples = DataManager.instance.GetCollectedSamples();
		int size = samples.Length;
		if (size>0)
		{
			visible = !visible;
			layerNum.enabled=!layerNum.enabled;
			//logCover.enabled=!logCover.enabled;
			right.enabled = !right.enabled;
			left.enabled = !left.enabled;
			if (visible)
			{
				// show current one
				Show();
				//PlanPathManager.instance.InViewLog();
			}
			else
			{
				// hide current one
				Hide();
				//PlanPathManager.instance.CloseViewLog();
			}
		}

	}

	public void Show()
	{
		object[] samples = DataManager.instance.GetCollectedSamples();
		string layerName = ((CoreSample)samples[entryNumber]).layername;
		Color layerColor = DataManager.instance.GetColorOfLayerName(layerName);
		GameObject orgLayer = GameObject.Find ("/Layers/" + layerName);
		LayerProperty property = orgLayer.GetComponent<LayerProperty> ();
		LayerType type = property.type;
		if (type==LayerType.Oil)
		{
			current=oil;
		}
		
		else if (type==LayerType.HardLayer)
		{
			current=hard;
		}
		
		else if (type==LayerType.SoftLayer)
		{
			current=soft;
		}

		layerNum.text = "Sample: " + string.Format("{0:0}", (entryNumber+1));
		current.enabled=true;
		if (DataManager.instance.IfLayerRevealed(layerName))
		{
			current.color = layerColor;
		}

	}

	public void Hide()
	{
		current.color = Color.white;
		oil.enabled = false;
		hard.enabled = false;
		soft.enabled = false;
	}
}
