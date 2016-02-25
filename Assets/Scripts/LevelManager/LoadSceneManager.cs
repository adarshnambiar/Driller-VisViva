using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class LoadSceneManager : MonoBehaviour {

	public string LevelFolder = "Level";	//the folder containing all the layer files
	public GameObject LayerPrefab;

	Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
		if (DataManager.instance != null) {
			LoadLevel (DataManager.instance.SelectedLevel);
		} else {
			Debug.Log("Data Manager not founded!");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadLevel(string levelName){
		string path = Application.dataPath + "/" + LevelFolder + "/" + levelName;

		/********Load level data from level document***************/
		XmlDocument layerdoc = new XmlDocument ();
		layerdoc.Load(path+"/leveldef.xml");
		int i = 0;
		foreach (XmlNode singlelayer in layerdoc.SelectNodes("layers/layer"))
		{
			string imgpath = path + "/" + singlelayer.SelectSingleNode("imgfile").InnerText + ".png";
			GameObject newLayer = Instantiate(LayerPrefab, _transform.position, Quaternion.identity) as GameObject;
			newLayer.transform.parent = _transform;
			//load image
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(File.ReadAllBytes(imgpath));
			//turn image into sprite
			Rect texRect = new Rect(0,0,tex.width, tex.height);
			Sprite texSprite = Sprite.Create(tex, texRect, new Vector2(0.5f, 0.5f), 100);
			SpriteRenderer spRenderer = newLayer.GetComponent<SpriteRenderer>();
			spRenderer.sprite = texSprite;
			//add collider
			newLayer.AddComponent<PolygonCollider2D>();
			newLayer.collider2D.isTrigger = true;
			//change name and apply properties
			string imgName = imgpath.Substring(path.Length+1, imgpath.Length-path.Length-1-4);
			newLayer.name = imgName;
			spRenderer.sortingOrder = i;
			LayerProperty layerProp = newLayer.GetComponent<LayerProperty>();
			//set layer type
			layerProp.SetType(System.Convert.ToInt32(singlelayer.SelectSingleNode("type").InnerText));
			//set layer shape marker
			if(singlelayer.SelectSingleNode("shapemarker")!=null){
				layerProp.SetLayerShape(System.Convert.ToInt32(singlelayer.SelectSingleNode("shapemarker").InnerText));
			}
			//set damage factor
			layerProp.SetDamageFactor(System.Convert.ToSingle(singlelayer.SelectSingleNode("damagefactor").InnerText));
			//set speed factor
			layerProp.SetSpeedFactor(System.Convert.ToSingle(singlelayer.SelectSingleNode("speedfactor").InnerText));
			//set fossil fuel possibility
			float maxPossibility = System.Convert.ToSingle(singlelayer.SelectSingleNode("maxoilpossibility").InnerText);
			float minPossibility = System.Convert.ToSingle(singlelayer.SelectSingleNode("minoilpossibility").InnerText);
			layerProp.SetPossibility(Random.Range(minPossibility, maxPossibility));
			//set fossil fuel amount
			float maxAmount = System.Convert.ToSingle(singlelayer.SelectSingleNode("maxoilamount").InnerText);
			float minAmount = System.Convert.ToSingle(singlelayer.SelectSingleNode("minoilamount").InnerText);
			layerProp.SetFossilFuelAmount(Random.Range(minAmount, maxAmount));
			//set porosity
			layerProp.SetPorosity(System.Convert.ToSingle(singlelayer.SelectSingleNode("prosity").InnerText));
			//set density
			layerProp.SetDensity(System.Convert.ToSingle(singlelayer.SelectSingleNode("density").InnerText));

			i++;
		}


		/*
		string[] layerFileImg = Directory.GetFiles(path, "*.png");
		foreach (string imgpath in layerFileImg) {
			GameObject newLayer = Instantiate(LayerPrefab, _transform.position, Quaternion.identity) as GameObject;
			newLayer.transform.parent = _transform;
			//load image
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(File.ReadAllBytes(imgpath));
			//turn image into sprite
			Rect texRect = new Rect(0,0,tex.width, tex.height);
			Sprite texSprite = Sprite.Create(tex, texRect, new Vector2(0.5f, 0.5f), 100);
			SpriteRenderer spRenderer = newLayer.GetComponent<SpriteRenderer>();
			spRenderer.sprite = texSprite;
			//add collider
			newLayer.AddComponent<PolygonCollider2D>();
			newLayer.collider2D.isTrigger = true;
			//change name and apply properties
			string imgName = imgpath.Substring(path.Length+1, imgpath.Length-path.Length-1-4);
			string[] segments = imgName.Split(new char[]{'_'});
			newLayer.name = segments[0];
			spRenderer.sortingOrder = IntParse(segments[0]);
			LayerProperty layerProp = newLayer.GetComponent<LayerProperty>();
			layerProp.SetType(segments[1]);
		}*/

		SendMessage("LoadLevelFinish");

	}

	void LoadLevelFinish(){

	}

	int IntParse(string value){
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}

}
