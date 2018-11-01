using UnityEngine;
using System.Collections;

public class secondCamTrigger : MonoBehaviour {

	public	GameObject	elev1Cam;

	void OnTriggerEnter( Collider other ){
		elev1Cam.transform.GetComponent<Camera>().enabled = true;
	}
	void OnTriggerExit( Collider other ){
		elev1Cam.transform.GetComponent<Camera>().enabled = false;
	}
}
