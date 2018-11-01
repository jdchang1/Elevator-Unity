using UnityEngine;
using System.Collections;

public class elevHallFrameController : MonoBehaviour {


	public int floor;
	public Transform callButtonLight;		
	public Transform HallLedPanel;
	public string elevTag = "elev01";

	private	elevControl elevator;
	private	AnimationClip openAnim, closeAnim;

	void Start(){
		//GRAB ELEVATOR REF
		elevator = GameObject.FindGameObjectWithTag( elevTag ).transform.GetComponent<elevControl>();
		//SET ANIMATION CLIPS
		openAnim = transform.GetComponent<Animation>().GetClip( "OpenDoorsV2" );
		closeAnim = transform.GetComponent<Animation>().GetClip( "CloseDoorsV2" );	
	}

	/// <summary>
	/// Opens the Hall Frame Doors.
	/// </summary>
	public void OpenDoor(){
		transform.GetComponent<Animation>().clip = openAnim;
		transform.GetComponent<Animation>().Play();
	}

	/// <summary>
	/// Closes the Hall Frame Door.
	/// </summary>
	public void CloseDoor(){
		transform.GetComponent<Animation>().clip = closeAnim;
		transform.GetComponent<Animation>().Play();
	}

	/// <summary>
	/// Turn Call Button light ON/OFF .
	public void CallButtonLight( bool turnOn ){
		//CHANGE BUTTON OBJECT MATERIAL
		if( turnOn )	
			callButtonLight.GetComponent<Renderer>().material = elevator.buttonOnMat;
		else
			callButtonLight.GetComponent<Renderer>().material  = elevator.buttonOffMat;
	}

	public void CallElevator(){
		CallButtonLight( true );
		elevator.MoveElevator( floor, true );
	}
}
