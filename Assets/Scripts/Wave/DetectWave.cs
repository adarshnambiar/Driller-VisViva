using UnityEngine;
using System.Collections;

public class DetectWave : MonoBehaviour {

	public float Speed = 5;
	public float MaxDistance = 10;
	public GameObject relatedTargetMarker;

	public LayerMask mask = -1;			//layer mask for sending detect wave

	Transform _transform;
	Vector3 direction;
	Vector2 hitnormal;

	string lastDetectLayer;
	string currentDetectLayer;
	string targetLayerName;

	// Use this for initialization
	void Start () {
		_transform = transform;
		direction = -_transform.up;

		Vector3 dist = _transform.position + MaxDistance * direction;
		float length = Vector3.Distance (dist, _transform.position);
		RaycastHit2D hit = Physics2D.Raycast (dist, -direction, length, mask);
		if (hit != null) {
			targetLayerName = hit.collider.name;

			hit.transform.collider2D.enabled = false;
			RaycastHit2D hit2 = Physics2D.Raycast(dist, direction, Mathf.Infinity, mask);
			if(hit2!=null){
				hitnormal = hit2.normal;
			}
			hit.transform.collider2D.enabled = true;
		}

		lastDetectLayer = "";
		currentDetectLayer = "";

	}
	
	// Update is called once per frame
	void Update () {
		float movedelta = Speed * Time.deltaTime;
		_transform.position += movedelta * direction;
		/*
		MaxDistance -= movedelta;
		if (MaxDistance < 0) {
			RespondAndDestroy();
		}
		*/
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "layer") {
			lastDetectLayer = currentDetectLayer;
			if(lastDetectLayer == targetLayerName){
				Vector3 reflectDir = Vector3.Reflect(direction, hitnormal);
				RespondAndDestroyDir(reflectDir);

				//RespondAndDestroy();
			}
			currentDetectLayer = other.name;
		}
		if (other.tag == "boundary") {
			lastDetectLayer = currentDetectLayer;
			if(other.name=="Bottom"){
				Vector3 reflectDir = Vector3.Reflect(direction, Vector3.up);
				RespondAndDestroyDir(reflectDir);
			}else{
				StartCoroutine(RespondAndDestoryDelay(_transform.position, 0.5f));
			}
		}
	}

	void RespondAndDestroyDir(Vector3 reflectDir){
		GeoGameManager.instance.CreateRespondWaveInDir (_transform.position, lastDetectLayer, reflectDir);
		if (relatedTargetMarker != null) {
			Destroy(relatedTargetMarker);
		}

		Destroy (gameObject);
	}
	/*
	void RespondAndDestroy(){
		GeoGameManager.instance.CreateRespondWave (_transform.position, lastDetectLayer);
		Destroy (gameObject);
	}
	*/
	IEnumerator RespondAndDestoryDelay(Vector3 eposition, float delay){
		yield return new WaitForSeconds (delay);
		GeoGameManager.instance.CreateRespondWaveInDir (_transform.position, lastDetectLayer, direction);
		if (relatedTargetMarker != null) {
			Destroy(relatedTargetMarker);
		}
		Destroy (gameObject);
	}

}
