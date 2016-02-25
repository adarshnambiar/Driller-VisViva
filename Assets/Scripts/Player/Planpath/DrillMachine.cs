using UnityEngine;
using System.Collections;

public class DrillMachine : MonoBehaviour {

	public static DrillMachine instance;

	public Transform HPLeft;
	public float DrillSpeed = 1;		//normal drill moving speed
	public float TotalHP = 100;			//total HP of the drill
	public float damageDelay = 0.25f;	//the delay time between two damage take place
	public GameObject layerIndicate;
	public GameObject feedbackText;

	float remainHP;						//remaining HP
	LayerType currentType;				//the layer type of the drill's current position
	float speedFactor;					//the factor affecting speed in different layer
	float damageFactor;					//the factor of damage in different layer
	float nextDamage = 0;

	Vector3 HPBarScale;
	SpriteRenderer HPBarSprite;

	void Awake(){
		instance = this;
		speedFactor = 1;
		damageFactor = 0;

		remainHP = TotalHP;
		currentType = LayerType.None;
		nextDamage = damageDelay;

		HPBarScale = HPLeft.localScale;
		HPBarSprite = HPLeft.GetComponentInChildren<SpriteRenderer> ();
	}

	void Update(){
		if (PlanPathManager.instance.GetStage () == PlanStage.Drilling) {
			if (nextDamage > 0) {
				nextDamage -= Time.deltaTime;
				if (nextDamage <= 0) {
					if (remainHP > 0) {
						remainHP -= damageFactor;
						float hpFactor = remainHP / TotalHP;
						HPBarScale.x = Mathf.Clamp01(hpFactor);
						HPLeft.localScale = HPBarScale;
						HPBarSprite.color = Color.Lerp(Color.red, Color.green, hpFactor);
						if (remainHP <= 0) {
							PlanPathManager.instance.GameOver ();
						}
					}
					nextDamage = damageDelay;
				}
			}
		}
		/*
		switch(currentType){
		case LayerType.HardLayer:
			speedFactor = 0.6f;
			break;
		case LayerType.SoftLayer:
			speedFactor = 1;
			break;
		}*/
	}

	void OnTriggerEnter2D(Collider2D other){

		switch (other.tag) {
		case "layer":
			LayerProperty layerProp = other.GetComponent<LayerProperty>();

			speedFactor = layerProp.GetSpeedFactor();
			damageFactor = layerProp.GetDamageFactor();

			CameraShaking.instance.SetShakingPower((layerProp.GetDensity()/70f)*0.05f+0.07f);

			layerIndicate.renderer.material.color = DataManager.instance.GetColorOfType(layerProp.type);

			break;
		case "PlanMarker":

			Marker markerCtrl = other.GetComponent<Marker>();

			float fossilfuel = markerCtrl.GetFossilFuelHere();
			GameObject feedback = Instantiate(feedbackText, transform.position+Vector3.up, Quaternion.identity) as GameObject;
			TextFeedback feedbackScript = feedback.GetComponent<TextFeedback>();
			if(fossilfuel>0){
				if(fossilfuel>DataManager.instance.GetAvgScore()){
					feedbackScript.SetText(2, (fossilfuel/DataManager.instance.GetTotalScore()) * 7000000);
				}else{
					feedbackScript.SetText(1, (fossilfuel/DataManager.instance.GetTotalScore()) * 7000000);
				}

				DataManager.instance.AddToScore(fossilfuel);
				SoundFXCtrl.instance.PlaySound(7, 1);
			}else{
				feedbackScript.SetText(0, 0);
			}

			if(markerCtrl.HasBeenSampled()){
				float fossilPossibility = markerCtrl.GetAvgPossibilityHere();
				if(fossilPossibility<30){
					if(fossilfuel>0){
						PlanPathManager.instance.PauseDrill(0);
					}else{
						PlanPathManager.instance.PauseDrill(1);
					}
				}else if(fossilPossibility<70){
					if(fossilfuel>0){
						PlanPathManager.instance.PauseDrill(2);
					}else{
						PlanPathManager.instance.PauseDrill(3);
					}
				}else{
					if(fossilfuel>0){
						PlanPathManager.instance.PauseDrill(4);
					}else{
						PlanPathManager.instance.PauseDrill(5);
					}
				}
			}else{
				if(DataManager.instance.GetLayerShape(markerCtrl.HoldLayerName())==LayerShape.Normal){
					if(fossilfuel>0){
						PlanPathManager.instance.PauseDrill(9);
					}else{
						PlanPathManager.instance.PauseDrill(8);
					}
				}else{
					if(fossilfuel>0){
						PlanPathManager.instance.PauseDrill(6);
					}else{
						PlanPathManager.instance.PauseDrill(7);
					}
				}
			}
			break;
		}
	}

	public float GetMoveSpeed(){
		return DrillSpeed * speedFactor;
	}

	public float GetRemainHP(){
		return remainHP;
	}

}
