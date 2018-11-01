using UnityEngine;
using System.Collections;

public class callBtnTrigger : MonoBehaviour {

	private	elevControl						elevContrl;
	private	elevHallFrameController		hallFrameContrl;

	void Start(){
		hallFrameContrl	= this.transform.parent.GetComponent<elevHallFrameController>();
		elevContrl		= GameObject.FindGameObjectWithTag( hallFrameContrl.elevTag ).transform.GetComponent<elevControl>(); 
	}

	void OnTriggerEnter( Collider other ){
		if( !elevContrl.isElevMoving ){//&& elevContrl.curFloorLevel !=  hallFrameContrl.floor ){
			elevContrl.newFloor = hallFrameContrl.floor;
			elevContrl.useCallBtn = true;
			hallFrameContrl.callButtonLight.GetComponent<Renderer>().material = elevContrl.buttonSelectorMat;
		}
	}

	void OnTriggerExit( Collider other ){
		elevContrl.useCallBtn = false;
		if( !elevContrl.isElevMoving )
			hallFrameContrl.callButtonLight.GetComponent<Renderer>().material = elevContrl.buttonOffMat;
	}
}