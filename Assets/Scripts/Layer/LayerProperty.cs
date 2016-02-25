using UnityEngine;
using System.Collections;

public class LayerProperty : MonoBehaviour {

	public LayerType type;		//the type of the layer

	float fossilfuel;			//the amount of fossil fuel(score) the layer holds
	float speedFactor;			//parameter affecting the drill movement in stage 3
	float damageFactor;			//parameter affecting the drill duration in stage 3
	float oilPossibility;		//the possibility of getting the fossilfuel in the layer
	LayerShape layerShape = LayerShape.Normal;		//the shape marker of a layer

	float porosity = 0;			
	float density = 0;			

	bool cangetOil;				//if oil can be obtained in the third stage

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetLayerShape(int shapeMarker){
		switch (shapeMarker) {
		case 0:
			layerShape = LayerShape.Normal;
			break;
		case 1:
			layerShape = LayerShape.FaultTrap;
			break;
		case 2:
			layerShape = LayerShape.AnticlineTrap;
			break;
		case 3:
			layerShape = LayerShape.StratagraphicTrap;
			break;
		case 4:
			layerShape = LayerShape.CapRock;
			break;
		}
	}

	public LayerShape GetLayerShape(){
		return layerShape;
	}

	public void SetType(int ltype){
		switch (ltype) {
		case 0:
			type = LayerType.SoftLayer;
			break;
		case 1:
			type = LayerType.HardLayer;
			break;
		case 2:
			type = LayerType.Oil;
			break;

		case 3:
			type = LayerType.Limestone;
			break;
		case 4:
			type = LayerType.Sandstone;
			break;
		case 5:
			type = LayerType.Conglomerate;
			break;
		case 6:
			type = LayerType.Shale;
			break;
		case 7:
			type = LayerType.Siltstone;
			break;
		case 8:
			type = LayerType.Clay;
			break;

		default:
			type = LayerType.None;
			break;
		}
	}

	public float ObtainFossilFuel(){
		if (cangetOil) {
			float amount = fossilfuel;
			fossilfuel -= amount;
			return amount;
		} else {
			return 0;
		}
	}

	public void SetSpeedFactor(float factor){
		speedFactor = factor;
	}

	public float GetSpeedFactor(){
		return speedFactor;
	}

	public void SetDamageFactor(float factor){
		damageFactor = factor;
	}

	public float GetDamageFactor(){
		return damageFactor;
	}

	public void SetPossibility(float possibility){
		oilPossibility = possibility;
		float chance = Random.Range (0f, 1f);
		if (chance < oilPossibility) {
			cangetOil = true;
		} else {
			cangetOil = false;
		}
	}

	public float GetPossibility(){
		return oilPossibility * 100;
	}

	public void SetFossilFuelAmount(float amount){
		fossilfuel = amount;
	}

	public float GetFossilFuelAmount(){
		return fossilfuel;
	}

	public void SetPorosity(float value){
		porosity = value;
	}

	public float GetPorosity(){
		return porosity;
	}

	public void SetDensity(float value){
		density = value;
	}

	public float GetDensity(){
		return density;
	}

	public float GetScoreFactor(){
		if (cangetOil) {
			return fossilfuel;
		} else {
			return 0;
		}
	}

	public string GetPrintName(){
		switch (type) {
		case LayerType.SoftLayer:
			return "Soft Layer";
		case LayerType.HardLayer:
			return "Hard Layer";
		case LayerType.Oil:
			return "Oil";
		case LayerType.Limestone:
			return "Limestone";
		case LayerType.Sandstone:
			return "Sandstone";
		case LayerType.Conglomerate:
			return "Conglomerate";
		case LayerType.Shale:
			return "Shale";
		case LayerType.Siltstone:
			return "Siltstone";
		case LayerType.Clay:
			return "Clay";
		default:
			return "Unknown";
		}
	}

}
