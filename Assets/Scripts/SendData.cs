using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class SendData : MonoBehaviour {
	//public static SendData instance;
	public static SerialPort Serial = new SerialPort ("COM4",9600);
	public static string arduino_data;
	public static bool indebug = false;

	// Use this for initialization
	void Start () {
		if (!indebug) {
			if (Serial.IsOpen == false) {
				Serial.Open ();
			}
			Serial.ReadTimeout = 16;
		}
	}
	
	public static void SendToArduino(int value)
	{
		if (!indebug) {
			if (Serial.IsOpen == false) {
				Serial.Open ();
			}
			arduino_data = value.ToString ();
			Serial.Write (arduino_data);		
		}
	}

	public static void ClearLEDStrip()
	{
		if (!indebug) {
			if (Serial.IsOpen == false) {
				Serial.Open ();
			}
			Serial.Write ("0");
		}
	}
	
	public static void ClearLEDButtons()
	{
		if (!indebug) {
			if (Serial.IsOpen == false) {
				Serial.Open ();
			}
			Serial.Write ("z");
		}
	}
	
	void Update(){
		//Testing purpose
			if (Input.GetKeyDown (KeyCode.F10)) {
				SendToArduino(8);	
			}
			if (Input.GetKeyDown (KeyCode.F9)) {
				SendToArduinoTutorial('a');	
			}
			if (Input.GetKeyDown (KeyCode.F11)) {
				ClearLEDStrip ();	
				ClearLEDButtons ();
			}
			if (Application.loadedLevelName == "GameMenu") {
					if (Serial.IsOpen == false) {
						Serial.Open ();
						ClearLEDStrip ();
						ClearLEDButtons ();
			}else
				ClearLEDButtons ();
				ClearLEDStrip ();
				}
		}

	
	void OnApplicationQuit() {
		ClearLEDStrip ();
		ClearLEDButtons ();
		Serial.Close();
	}

	void OnDestroy(){
		ClearLEDStrip ();
		ClearLEDButtons ();
		Serial.Close();
	}

	public static void SendToArduinoTutorial(char val)
	{
		if (!indebug) {
			arduino_data = val.ToString ();
			if (Serial.IsOpen == false) {
				Serial.Open ();
			}
			Serial.Write (arduino_data);		
		}
	}

}
