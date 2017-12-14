using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			move ("left");
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			move ("right");
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			move ("up");
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			move ("down");
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			move ("rotate");
		}
	}

	void move(string key)
	{
		if (StageState.CouldMoveBlock (key)) {
			Debug.Log ("could move to the direction you pushed");
		}
	}
}
