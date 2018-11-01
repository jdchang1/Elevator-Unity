using UnityEngine;
using System.Collections;

public class controlTrigger : MonoBehaviour {

	private	elevControl		elevContrl;

	void Start(){
		elevContrl	= this.transform.parent.GetComponent<elevControl>();
	}

	void OnTriggerEnter( Collider other ){
		if( !elevContrl.isElevMoving ){
			elevContrl.useControls 	= true;
			elevContrl.prevBtn 		= elevContrl.curFloorLevel;
			elevContrl.SelectButtonOnTrigger( true );
		}
	}

	void OnTriggerExit( Collider other ){
		if( !elevContrl.isElevMoving ){
			elevContrl.useControls 	= false;
			elevContrl.SelectButtonOnTrigger( false );
		}
	}

}
