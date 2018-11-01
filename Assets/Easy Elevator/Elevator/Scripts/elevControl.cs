using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class elevControl : MonoBehaviour {

	public Material buttonOnMat, buttonOffMat, buttonSelectorMat;
	public Material ledMat;
	public Transform ledPanel;
	public float ledMatSwitchDelay = .5f;
	private Material[] ledMatsArray;

    private List<Transform> buttonLightList;
	private List<GameObject> hallFramesList;
	private List<Texture> texturesList;
	private AnimationClip openAnim, closeAnim;

	public Transform btnLightGroup;
	public string hallFrameTag = "elev01hallFrame";

	public int curFloorLevel = 0;
	public float timeBtwnFloors = 3;
	public bool doorOpen;
	public bool waitForFixedUpdate = true; //FIXES JITTER WHEN PLAYER IS ON MOVING ELEVATOR
	[HideInInspector]
	public bool useControls, useCallBtn, isElevMoving;
	[HideInInspector]
	public int prevBtn, newFloor;
	private Transform elevator;

	/// <summary>
	/// returns the floor level to move the elevator to. 
	/// A Value of 13 will return the current floor. 
	/// Anything greater than 13 will return one floor lower. (14 is actually 13)
	/// </summary>
	/// <returns>The floor for the elevator to go to.</returns>
	/// <param name="floor">Floor.</param>
	private int floorCheck( int floor ){
		if( floor == 13 )
			return curFloorLevel;
		if( floor > 12 )
			floor -= 1;
		return floor;
	}

	void Start () {
		//TRANSFORM REFERENCE
		elevator = this.transform;

		//BUTTON LIGHTS OBJECT LIST, POPULATE LIGHTS OBJECT LIST, SORT LIGHTS OBJECT LIST
		buttonLightList = new List<Transform>();
		foreach ( Transform btnLight in btnLightGroup ){ 
			buttonLightList.Add( btnLight );
		}
		buttonLightList = buttonLightList.OrderBy( Transform=>Transform.name ).ToList();


		//HALL FRAMES OBJECT LIST, POPULATE FRAMES OBJECT LIST, SORT FRAMES OBJECT LIST
		hallFramesList	= new List<GameObject>();
		foreach ( GameObject hallFrame in GameObject.FindGameObjectsWithTag( hallFrameTag ) ){ 
			hallFramesList.Add( hallFrame );
		}
		hallFramesList = hallFramesList.OrderBy( GameObject=>GameObject.GetComponent<elevHallFrameController>().floor ).ToList();

//		//LED PANEL TEXTURES LIST, POPULATE TEXTURES LIST, SORT THE LIST
//		texturesList = new List<Texture>();
//		foreach ( Texture tex in Resources.LoadAll( "LEDPanelTextures" ) ){ 
//			texturesList.Add( tex );
//		}
//		texturesList = texturesList.OrderBy( Texture=>Texture.name ).ToList();

		//LED PANEL TEXTURES LIST, POPULATE TEXTURES LIST, SORT THE LIST
		texturesList = new List<Texture>();
		foreach ( Texture tex in Resources.LoadAll( "LEDPanelTexturesV2" ) ){ 
			texturesList.Add( tex );
		}
		texturesList = texturesList.OrderBy( Texture=>Texture.name ).ToList();


		//SET ANIMATION CLIPS
		openAnim = transform.GetComponent<Animation>().GetClip( "OpenDoorsV2" );
		closeAnim = transform.GetComponent<Animation>().GetClip( "CloseDoorsV2" );	

		//ASSIGN LED MATERIALS TO ELEVATOR AND HALL FRAMES, THEN SET LED FLOOR DISPLAY & ELEVATOR TO CURRENT FLOOR
		ledMatsArray = new Material[2];
		ledMatsArray[0] = ledPanel.GetComponent<Renderer>().material;
		ledMatsArray[1] = ledMat;
		ledPanel.GetComponent<Renderer>().materials = ledMatsArray;
		foreach ( var hallFrame in hallFramesList ) {
			hallFrame.GetComponent<elevHallFrameController>().HallLedPanel.GetComponent<Renderer>().materials = ledMatsArray;
		}
		LEDPanel( curFloorLevel );
		elevator.position = hallFramesList[ curFloorLevel ].transform.position;

		//SET DOOR OPEN/CLOSE
		if ( doorOpen ) {
			elevator.GetComponent<Animation>().clip = openAnim;
			elevator.GetComponent<Animation>()[openAnim.name].time =openAnim.length;
			elevator.GetComponent<Animation>().Play();
			if( hallFramesList[curFloorLevel].GetComponent<elevHallFrameController>() != null ){
				hallFramesList[curFloorLevel].GetComponent<elevHallFrameController>().GetComponent<Animation>().clip = openAnim;
				hallFramesList[curFloorLevel].GetComponent<elevHallFrameController>().GetComponent<Animation>()[openAnim.name].time = openAnim.length;
				hallFramesList[curFloorLevel].GetComponent<elevHallFrameController>().GetComponent<Animation>().Play();
			}
		}
	}

	/// <summary>
	/// Buttons the select.
	/// </summary>
	/// <param name="buttonNum">Button number.</param>
	public void ButtonSelect( int buttonNum ){
		if( buttonNum == 13 && prevBtn == 12 ){
			buttonNum = 14;
			newFloor = 14;
		}
		if( buttonNum == 13 && prevBtn == 14 ){
			buttonNum = 12;
			newFloor = 12;
		}
		if(buttonNum > 21 ){
			buttonNum = 0;
			newFloor	=0;
		}
		if(buttonNum < 0 ){
			buttonNum = 21;
			newFloor	=21;
		}
		if( buttonNum > 12 )
			buttonNum -= 1;
		if (newFloor == 13)
			newFloor = buttonNum;

		var selectedBtn = buttonLightList[ buttonNum ];
		var oldMat = selectedBtn.GetComponent<Renderer>().material;

		buttonLightList[ prevBtn  ].GetComponent<Renderer>().material = oldMat;
		selectedBtn.GetComponent<Renderer>().material = buttonSelectorMat;
		prevBtn = buttonNum ;
	}

	/// <summary>
	/// Finds the button to to highlight when player enters the trigger for the elevator control panel.
	/// </summary>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	public void SelectButtonOnTrigger( bool turnOn ){
		if( curFloorLevel <= 12 )
			newFloor = curFloorLevel ;
		else
			newFloor = curFloorLevel + 1;

		if( turnOn )
			buttonLightList[ curFloorLevel ].GetComponent<Renderer>().material = buttonSelectorMat;
		else
			buttonLightList[ prevBtn ].GetComponent<Renderer>().material = buttonOffMat;
	}

	/// <summary>
	/// Presses highlighted button on the elevator panel.
	/// </summary>
	/// <param name="buttonNum">Button number.</param>
	public void PressButton( int buttonNum ){
		switch ( buttonNum ) {
		case 20:
			ButtonHelpLight( true );
			break;
		case 21:
			ButtonOpenLight( true );
			//OPEN THE DOOR, IF IT IS CLOSED
			if( !doorOpen )
				OpenDoor( curFloorLevel );
			break;
			
		default:
			if (buttonNum > hallFramesList.Count)
				break;
			else
				MoveElevator( buttonNum, false );
			break;
		}
	}

	/// <summary>
	/// Turn Button light ON/OFF for Floor Buttons.
	/// NOTE: USE FLOOR NUMBERS, 0 = Basement!
	/// </summary>
	/// <param name="buttonNum">Button number (THE FLOOR NUMBER).</param>
	/// <param name="turnOn">If set to <c>true</c> turn on.</param>
	void ButtonFloorLight(  int buttonNum, bool turnOn  ){
		//SAFETY CHECK!
		if( buttonNum < 0 || buttonNum > hallFramesList.Count )
			return;

		//CHANGE BUTTON OBJECT MATERIAL
		if( turnOn )
			buttonLightList[buttonNum].GetComponent<Renderer>().material = buttonOnMat;
		else
			buttonLightList[buttonNum].GetComponent<Renderer>().material = buttonOffMat;

	}

	/// <summary>
	/// Help Button light ON/OFF.
	/// </summary>
	void ButtonHelpLight( bool turnOn ){
		if( turnOn )
			buttonLightList[19].GetComponent<Renderer>().material = buttonOnMat;
		else
			buttonLightList[19].GetComponent<Renderer>().material = buttonOffMat;;
	}


	/// <summary>
	/// Open Button light ON/OFF.
	/// </summary>
	void ButtonOpenLight( bool turnOn ){
		if( turnOn )
			buttonLightList[20].GetComponent<Renderer>().material = buttonOnMat;
		else
			buttonLightList[20].GetComponent<Renderer>().material = buttonOffMat;
	}


	/// <summary>
	/// Switch LED display to this number
	/// NOTE: USE FLOOR NUMBER
	/// </summary>
	/// <param name="newFlooNum">New Floor number.</param>
	void LEDPanel( int newFloorNum ){
		//SAFETY CHECK!
		if( newFloorNum > hallFramesList.Count || newFloorNum < 0  )
			return;

		StartCoroutine( LEDPanelSwitch(  newFloorNum ) );
	}
	/// <summary>
	/// Switch LED display from current floor to new floor incrementally
	/// </summary>
	/// <param name="newFloorNum">New floor number.</param>
	/// <param name="floorTime">Time between floors.</param>
	void LEDPanel( int newFloorNum, float floorTime ){
		//SAFETY CHECK!
		if( newFloorNum > hallFramesList.Count || newFloorNum < 0 )
			return;
		
		StartCoroutine( LEDPanelSwitch(  newFloorNum, floorTime ) );
	}

	/// <summary>
	/// Switch LED display to this number.
	/// </summary>
	/// <param name="newFloorNum">Floor number.</param>
	IEnumerator LEDPanelSwitch( int newFloorNum ){
		//SWITCH TO BLANK LED TEXTRES
//		ledMat.SetTexture( "_MainTex", texturesList[ texturesList.Count-2 ] );
		ledMat.SetTexture( "_EmissionMap", texturesList[ texturesList.Count-1 ] );
		yield return new WaitForSeconds( ledMatSwitchDelay );

		//CONVERT FLOOR NUMBER TO LIST INDEXES
		int illume = newFloorNum ;
//		int illume = ( newFloorNum * 2 ) +1;
//		int diff = illume - 1;
		
		//CHANGE LED MATERIAL TEXTURES
//		ledMat.SetTexture( "_MainTex", texturesList[diff] );
		ledMat.SetTexture( "_EmissionMap", texturesList[illume] );

		//TURN BUTTON LIGHT OFF
		ButtonFloorLight( newFloorNum, false );

		curFloorLevel = newFloorNum;
	}

	/// <summary>
	/// Switch LED display from current floor to new floor incrementally 
	/// </summary>
	/// <param name="newfloorNum">New Floor number.</param>
	/// <param name="floorTime">Time between Floors.</param>
	IEnumerator LEDPanelSwitch( int newFloorNum, float floorTime ){
		//UP OR DOWN
		int floorIncrement = ( newFloorNum < curFloorLevel ) ? -1 : 1;

		while ( curFloorLevel != newFloorNum ) {
			//SWITCH TO BLANK LED TEXTRES
//			ledMat.SetTexture( "_MainTex", texturesList[ texturesList.Count-2 ] );
			ledMat.SetTexture( "_EmissionMap", texturesList[ texturesList.Count-1 ] );
			yield return new WaitForSeconds( ledMatSwitchDelay );

			//CONVERT FLOOR NUMBER TO LIST INDEXES
//			int illume = ( (curFloorLevel + floorIncrement ) * 2 ) + floorIncrement;
			int illume = curFloorLevel + floorIncrement;
//			if( floorIncrement < 0 )
//				illume = illume + 2;
//			int diff = illume - 1;

			//CHANGE LED MATERIAL TEXTURES
//			ledMat.SetTexture( "_MainTex", texturesList[diff] );
			ledMat.SetTexture( "_EmissionMap", texturesList[illume] );
			yield return new WaitForSeconds( floorTime - ledMatSwitchDelay );

			curFloorLevel += floorIncrement;
		}
		//TURN BUTTON LIGHT OFF
		ButtonFloorLight( newFloorNum, false );
	}

	/// <summary>
	/// Opens the elevator door only.
	/// </summary>
	void OpenDoor(){
		transform.GetComponent<Animation>().clip = openAnim;
		transform.GetComponent<Animation>().Play();
		doorOpen = true;
	}
	/// <summary>
	/// Opens the elevator & hall door.
	/// </summary>
	/// <param name="floor">Floor.</param>
	void OpenDoor( int floor ){
		transform.GetComponent<Animation>().clip = openAnim;
		transform.GetComponent<Animation>().Play();
		hallFramesList[floor].GetComponent<elevHallFrameController>().OpenDoor();
		doorOpen = true;
	}

	/// <summary>
	/// Closes the elevator door only.
	/// </summary>
	void CloseDoor(){
		transform.GetComponent<Animation>().clip = closeAnim;
		transform.GetComponent<Animation>().Play();
		doorOpen = false;
	}
	/// <summary>
	/// Closes the elevator & hall door.
	/// </summary>
	/// <param name="floor">Floor.</param>
	void CloseDoor( int floor ){
		transform.GetComponent<Animation>().clip = closeAnim;
		transform.GetComponent<Animation>().Play();
		hallFramesList[floor].GetComponent<elevHallFrameController>().CloseDoor();
		doorOpen = false;
	}

	/// <summary>
	/// Moves the elevator to the specified floor.
	/// </summary>
	/// <param name="moveToFloor">Move to floor.</param>
	public void MoveElevator( int moveToFloor , bool callElevator){
		//SAFETY CHECK
		if( moveToFloor > hallFramesList.Count || moveToFloor < 0 )
			return;

		StartCoroutine ( MoveElevatorTo( floorCheck( moveToFloor ), callElevator ) );
	}

	/// <summary>
	/// Moves the elevator.
	/// </summary>
	/// <param name="startPos">Start position.</param>
	/// <param name="endPos">End position.</param>
	IEnumerator MoveElevatorTo(  int floor, bool callElevator ){
		isElevMoving = true;
		useControls = false;
		//ADJUST BUTTON LIGHTS
		ButtonFloorLight( curFloorLevel, false );
		ButtonFloorLight( floor, true );

		//CLOSE THE DOORS
		if( doorOpen ){
			CloseDoor( curFloorLevel );
			yield return new WaitForSeconds(elevator.GetComponent<Animation>().clip.length) ;
		}else{
			yield return new WaitForSeconds( .5f ) ;
		}

		//START LED PANEL CHANGE
		LEDPanel( floor, timeBtwnFloors );

		//MOVE THE ELEVATOR
		Vector3 startPos = elevator.position;
		Vector3 endPos = hallFramesList[floor].transform.position;
		float time = Mathf.Abs( newFloor - curFloorLevel) * timeBtwnFloors;
		if( time == 0 )
			time = timeBtwnFloors;
		float i = 0.0f;
		float rate	= 1.0f/time;

		while ( i < 1.0f ) {
			i += Time.deltaTime * rate;
			elevator.position = Vector3.Lerp( startPos, endPos,  i );
			if( waitForFixedUpdate )
				yield return new WaitForFixedUpdate();
			else
				yield return null; 
		}

		//OPEN THE DOORS
		OpenDoor( floor );

		//ADJUST BUTTON LIGHTS
		ButtonFloorLight( floor, false );
		hallFramesList[ floor ].GetComponent<elevHallFrameController>().CallButtonLight( false );
		isElevMoving = false;

		if( callElevator )
			useCallBtn = false;
		else
			useControls = true;

		newFloor = curFloorLevel;
	}
	
	void Update () {
		//USE THE HALL FRAME CALL BUTTON > TRIGGERED FROM callBtnTrigger.cs
		if( useCallBtn ){
			if(Input.GetKeyDown(KeyCode.E))
				hallFramesList[ floorCheck( newFloor )].GetComponent<elevHallFrameController>().CallElevator();
		}

		//USE THE ELEVATOR CONTROL PANEL > TRIGGERED FROM  controlTrigger.cs
		if( useControls ){

			//HIGHLIGHT SELECTION UP/DOWN
			if( Input.GetKeyDown( KeyCode.R ) ){
				newFloor ++;
				ButtonSelect( newFloor );
			}
			if( Input.GetKeyDown( KeyCode.F ) ){
				newFloor --;
				ButtonSelect( newFloor );
			}

			//SELECT THE HIGHLIGHTED BUTTON
			if(Input.GetKeyDown(KeyCode.E)){
				PressButton( newFloor );
			}	
		}
	}
}

