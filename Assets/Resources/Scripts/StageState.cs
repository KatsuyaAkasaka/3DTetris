using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState : MonoBehaviour {

	public static int[,,] stage;	//それぞれx, y, zを表している
	const int MAXIMUM_LENGTH = 6;	//ブロックが入れる部分
	const int ARRAY_SIZE = 8;		//配列のサイズ
	const int STAGE_SIZE_X = 8;		//stageのサイズ(8,7,8)
	const int STAGE_SIZE_Y = 7;
	const int STAGE_SIZE_Z = 8;

	const float moveAmount = 0.08f;

	//(0,0,0)はステージの手前左下



	//stage init (all status are 0)
	void Start () {
		stage = new int[ARRAY_SIZE, ARRAY_SIZE-1, ARRAY_SIZE];
		for (int i = 0; i < ARRAY_SIZE; i++) {
			for (int j = 0; j < ARRAY_SIZE-1; j++) {
				for (int k = 0; k < ARRAY_SIZE; k++) {
					if(i == 0 || i == ARRAY_SIZE-1 || j == 0 || k == 0 || k == ARRAY_SIZE-1){
						stage[i, j, k] = 1;		//外壁はalways1
					}
					else {
						stage[i, j, k] = 0;		//内側はまだ何も入っていないので0
					}
				}
			}
		}
	}

	//指定した方向に動かせるならtrueを返す
	//動かせないならstageを初期に戻してfalseを返す
	public static bool CouldMoveBlock(string str)
	{
		int[,,] tmp_stage = stage; 
		switch (str) {
		case "drop":
		//全探索
			for (int i = 0; i < STAGE_SIZE_X; i++) {
				for (int j = 0; j < STAGE_SIZE_Y; j++) {
					for (int k = 0; k < STAGE_SIZE_Z; k++) {
						if (StageState.stage [i, j, k] == 2) {
							if (StageState.stage [i, j - 1, k] == 1) {		//動かしてるブロックの下にすでにobjectがあった場合false
								stage = tmp_stage;			//stageを変更前に戻す
								return false;
							} else {
								StageState.stage [i, j - 1, k] = 2;			//2をずらす
								StageState.stage [i, j, k] = 0;
							}
						}
					}
				}
			}
			return true;
		
		case "left":
			for (int i = 0; i < STAGE_SIZE_X; i++) {
				for (int j = 0; j < STAGE_SIZE_Z; j++) {
					for (int k = 0; k < STAGE_SIZE_Y; k++) {
						if (StageState.stage [i, k, j] == 2) {
							if (StageState.stage [i - 1, k, j] == 1) {		//動かしてるブロックの左にすでにobjectがあった場合false
								stage = tmp_stage;			//stageを変更前に戻す
								return false;
							} else {
								StageState.stage [i - 1, k, j] = 2;			//2をずらす
								StageState.stage [i, k, j] = 0;
							}
						}
					}
				}
			}
			return true;

		case "right":
			for (int i = STAGE_SIZE_X-1; i >= 0; i--) {
				for (int j = 0; j < STAGE_SIZE_Z; j++) {
					for (int k = 0; k < STAGE_SIZE_Y; k++) {
						if (StageState.stage [i, k, j] == 2) {
							if (StageState.stage [i + 1, k, j] == 1) {		//動かしてるブロックの右にすでにobjectがあった場合false
								stage = tmp_stage;			//stageを変更前に戻す
								return false;
							} else {
								StageState.stage [i + 1, k, j] = 2;			//2をずらす
								StageState.stage [i, k, j] = 0;
							}
						}
					}
				}
			}
			return true;

		case "up":
			for (int i = STAGE_SIZE_Z-1; i >= 0; i--) {
				for (int j = 0; j < STAGE_SIZE_X; j++) {
					for (int k = 0; k < STAGE_SIZE_Y; k++) {
						if (StageState.stage [j, k, i] == 2) {
							if (StageState.stage [j, k, i+1] == 1) {		//動かしてるブロックの下にすでにobjectがあった場合false
								stage = tmp_stage;			//stageを変更前に戻す
								return false;
							} else {
								StageState.stage [j, k, i+1] = 2;			//2をずらす
								StageState.stage [j, k, i] = 0;
							}
						}
					}
				}
			}
			return true;

		case "down":
			for (int i = 0; i < STAGE_SIZE_Z; i++) {
				for (int j = 0; j < STAGE_SIZE_X; j++) {
					for (int k = 0; k < STAGE_SIZE_Y; k++) {
						if (StageState.stage [j, k, i] == 2) {
							if (StageState.stage [j, k, i-1] == 1) {		//動かしてるブロックの下にすでにobjectがあった場合false
								stage = tmp_stage;			//stageを変更前に戻す
								return false;
							} else {
								StageState.stage [j, k, i-1] = 2;			//2をずらす
								StageState.stage [j, k, i] = 0;
							}
						}
					}
				}
			}
			return true;

		default:
			return false;
		}
	}

	//stageに現在書かれている2を全て1にしてステージ情報を確定させる
	public static void confirm_stage() 
	{
		for (int i = 0; i < STAGE_SIZE_X; i++) {
			for (int j = 0; j < STAGE_SIZE_Y; j++) {
				for (int k = 0; k < STAGE_SIZE_Z; k++) {
					if (StageState.stage [i, j, k] == 2) {
						StageState.stage [i, j, k] = 1;
					}
				}
			}
		}
	}

	//ブロックの座標移動
	//システムの座標は移動しない
	public static void MoveBlock(string direction)
	{
		switch (direction) {
		case "drop":
			GameController.nowBlock.transform.position -= new Vector3 (0f, moveAmount, 0f);
			break;
		case "left":
			GameController.nowBlock.transform.position -= new Vector3 (moveAmount, 0f, 0f);
			break;
		case "right":
			GameController.nowBlock.transform.position += new Vector3 (moveAmount, 0f, 0f);
			break;
		case "up":
			GameController.nowBlock.transform.position += new Vector3 (0f, 0f, moveAmount);
			break;
		case "down":
			GameController.nowBlock.transform.position -= new Vector3 (0f, 0f, moveAmount);
			break;
		default:
			break;
		}
	}
		
}
