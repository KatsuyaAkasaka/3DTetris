using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


	private Vector3 touchStartPos;
	private Vector3 touchPos;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//ステージが確定していたらユーザは操作不能状態にする
		if (!DropBlocks.finish_put) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				move ("left");
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				move ("right");
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				move ("up");
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				move ("down");
			}
			if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
				move ("rotate_x");
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				move ("rotate_y");
			}

			Flick ();


		}
	}

	void move (string key)
	{
		if (StageState.CouldMoveBlock (key)) {
			StageState.MoveBlock (key);
		}
	}

	void Flick ()
	{
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			touchStartPos = new Vector3 (Input.mousePosition.x,
				Input.mousePosition.y,
				Input.mousePosition.z);
		}

		if (Input.GetKey (KeyCode.Mouse0)) {
			touchPos = new Vector3 (Input.mousePosition.x,
				Input.mousePosition.y,
				Input.mousePosition.z);
			move (GetDirection ());
		}
	}

	string GetDirection ()
	{
		float directionX = touchPos.x - touchStartPos.x;
		float directionY = touchPos.y - touchStartPos.y;
		string Direction = "";

		if (Mathf.Abs (directionY) < Mathf.Abs (directionX)) {
			if (30 < directionX) {
				//右向きにフリック
				Direction = "right";
				ResetPos ();
			} else if (-30 > directionX) {
				//左向きにフリック
				Direction = "left";
				ResetPos ();
			}
		} else if (Mathf.Abs (directionX) < Mathf.Abs (directionY)) {
			if (30 < directionY) {
				//上向きにフリック
				Direction = "up";
				ResetPos ();
			} else if (-30 > directionY) {
				//下向きのフリック
				Direction = "down";
				ResetPos ();
			}
		} else {
			//タッチを検出
			Direction = "touch";
		}
		return Direction;
	}

	void ResetPos(){
		touchStartPos = touchPos;
	}
}
