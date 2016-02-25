using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnPointerDown(PointerEventData pointer){
		//Debug.Log (pointer.position);
		//PlanPathManager.instance.OnPointerDown (pointer);
	}

	public void OnPointerUp(PointerEventData pointer){
		//Debug.Log (pointer.position);
		//PlanPathManager.instance.OnPointerUp (pointer);
	}

}
