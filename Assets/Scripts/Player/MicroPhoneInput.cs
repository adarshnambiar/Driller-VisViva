using UnityEngine;
using System.Collections;

public class MicroPhoneInput : MonoBehaviour {

	public static MicroPhoneInput instance;

	public float sensitivity = 100;
	public float threahold = 10;
	public float maxScaledVolume = 40;
	public int samplerate = 11024;		//44100 when taking amplitude
	public int frequencySampleRate = 8192;

	[Header("Debug Options")]
	public bool debuging = true;
	public float debugRetuan = 0.5f;

	AudioClip clipRecord;
	int dec = 256;
	bool recording = false;
	float avgloudness;

	// Use this for initialization
	void Awake () {
		instance = this;
		avgloudness = 1;
	}

	void Update(){
		Debug.Log ("frequency:" + GetFundamentalFrequency());
	}

	public void StartRecording(){
		if (!debuging) {
			recording = true;
			clipRecord = Microphone.Start (null, true, 10, samplerate);
			audio.clip = clipRecord;
			audio.mute = true;
			audio.loop = true;
			audio.Play();
		}
	}

	public void StopRecording(){
		if (!debuging) {
			recording = false;
			Microphone.End (null);
		}
	}

	float GetAveragedVolume(){
		float[] waveData = new float[dec];
		int micPos = Microphone.GetPosition (null) - (dec + 1);
		if (micPos > 0) {
			clipRecord.GetData (waveData, micPos);
			float avg = 0;
			foreach (float s in waveData) {
				avg += Mathf.Abs (s);
			}
			return avg / dec;
		} else {
			return 0;
		}
	}

	public float GetScaledLoudness(){
		if (debuging) {
			return debugRetuan;
		} else {
			float loudness = (GetAveragedVolume () * sensitivity);
			//avgloudness = (3*avgloudness+loudness)/4;
			float scaledSound = Mathf.Clamp01((loudness-threahold)/maxScaledVolume);
			//float scaledSound = Mathf.Clamp01(loudness/(avgloudness*2));
			//float scaledSound = GetScaledFFT();
			return scaledSound;
		}
	}

	public float GetFundamentalFrequency(){
		float fundamentalFrequency = 0.0f;
		float[] data = new float[frequencySampleRate];
		audio.GetSpectrumData (data, 0, FFTWindow.Hanning);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j< frequencySampleRate; j++) {
			if(s < data[j]){
				s = data[j];
				i = j;
			}
		}
		fundamentalFrequency = i * samplerate / frequencySampleRate;
		return fundamentalFrequency;
	}

	public float GetScaledFFT(){
		return Mathf.Lerp(0.95f, 0f, Mathf.Clamp01 ((GetFundamentalFrequency () - 20) / 350));
	}

}
