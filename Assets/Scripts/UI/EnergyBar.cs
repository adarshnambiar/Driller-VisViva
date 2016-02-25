using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {
	public static EnergyBar instance;
	public Slider slider;
	private float sliderValue = 100f; // initial sliderValue
	private float lastTime = 0f; // last time energy was updated due to time loss
	private float thisTime = 0f; // time since game started
	private float energyLossPerSecond = -0.1f; // points deducted every second

	public static bool gameover;
	
	// Use this for initialization
	void Start () {
		instance = this;
		lastTime = Time.time;
		thisTime = Time.time;
		gameover = false;
	}
	
	public void ChangeEnergy(float change) 
	{
		sliderValue += change;
	}
	
	// Update is called once per frame
	void Update () {
		thisTime = Time.time;
		if ((thisTime - lastTime) >=1)
		{
			ChangeEnergy(energyLossPerSecond);
			lastTime = thisTime;
		}

		slider.value = sliderValue;
		if (sliderValue <= 0)
		{
			gameover =true;
		

		}
	}


}
