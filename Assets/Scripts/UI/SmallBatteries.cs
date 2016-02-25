using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SmallBatteries : MonoBehaviour {

	public Image[] batteries;
	private int batteryCount;
	
	// Use this for initialization
	void Start () {
		batteries = gameObject.GetComponentsInChildren<Image>(); // this includes the shelf images. jars start at index 1
		batteryCount = batteries.Length-1;
	}
	
	public void RemoveBattery(){
		if (batteryCount>0)
		{
			batteries[batteryCount].enabled=false;
			batteryCount--;
		}
	
	}

}
