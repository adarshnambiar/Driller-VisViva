using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MenuGameManager : MonoBehaviour {

	public static MenuGameManager instance;
	public string LevelFolder = "Level";

	public GameObject NoLevelFound;
	public GameObject LevelSelection;
	public Text selectedLevelName;

	ArrayList levels;
	int curSelectedLevel;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		levels = new ArrayList ();
		LoadLevelMenu ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Shelf_Right")) {
			NextLevel();
		}else if (Input.GetButtonDown ("Shelf_Left")) {
			LastLevel();
		}
		if (Input.GetButtonDown ("Sample")) {
			ConfirmLevel();
		}
	}

	void LoadLevelMenu(){
		/*****Load all level folder information and store in array list********/
		string path = Application.dataPath + "/" + LevelFolder;
		string[] levelEntries = Directory.GetDirectories (path);
		foreach (string levelentry in levelEntries) {
			levels.Add(levelentry.Substring(path.Length+1, levelentry.Length - path.Length - 1));
		}

		/*****No level folder found, display no level information***********/
		if (levels.Count == 0) {
			LevelSelection.SetActive(false);
			NoLevelFound.SetActive(true);
			this.enabled = false;
		} else {
		/********Display option for selecting level************************/
			NoLevelFound.SetActive(false);
			LevelSelection.SetActive(true);
			curSelectedLevel = 0;
			selectedLevelName.text = (string)levels[0];
		}
	}

	public void NextLevel(){
		curSelectedLevel++;
		if (curSelectedLevel == levels.Count) {
			curSelectedLevel = 0;
		}
		selectedLevelName.text = (string)levels [curSelectedLevel];
	}

	public void LastLevel(){
		curSelectedLevel--;
		if (curSelectedLevel < 0) {
			curSelectedLevel = levels.Count-1;
		}
		selectedLevelName.text = (string)levels [curSelectedLevel];
	}

	public void ConfirmLevel(){
		DataManager.instance.SelectedLevel = (string)levels [curSelectedLevel];
		Application.LoadLevel ("IntroScene");
	}

}
