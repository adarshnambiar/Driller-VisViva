using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {
	public static Buttons instance;
	private int size; // size of Array containing log cards

	// Use this for initialization
	void Start () {
		instance = this;
		object[] samples = DataManager.instance.GetCollectedSamples();
		size = samples.Length;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RightButton()
	{
		if (size>0)
		{
			OpenDataLog.instance.Hide();
			OpenDataLog.instance.entryNumber = (OpenDataLog.instance.entryNumber + 1) % size;
			OpenDataLog.instance.Show();
		}
	}

	public void LeftButton()
	{
		if (size>0)
		{
			OpenDataLog.instance.Hide ();
			OpenDataLog.instance.entryNumber = ((OpenDataLog.instance.entryNumber - 1)+size) % size;
			OpenDataLog.instance.Show ();
		}

	}
}
