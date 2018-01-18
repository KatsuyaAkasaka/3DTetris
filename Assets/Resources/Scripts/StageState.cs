using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState : MonoBehaviour {

	public static int[,,] stage;	//それぞれx, y, zを表している
	const int STAGE_SIZE_X = 8;		//stageのサイズ(8,7,8)
	const int STAGE_SIZE_Y = 7;
	const int STAGE_SIZE_Z = 8;

	const float moveAmount = 0.08f;

	//(0,0,0)はステージの手前左下



	//stage init (all status are 0)
	void Start () {
		stage = new int[STAGE_SIZE_X, STAGE_SIZE_Y, STAGE_SIZE_Z];
		for (int i = 0; i < STAGE_SIZE_X; i++) {
			for (int j = 0; j < STAGE_SIZE_Y; j++) {
				for (int k = 0; k < STAGE_SIZE_Z; k++) {
					if(i == 0 || i == STAGE_SIZE_X-1 || j == 0 || k == 0 || k == STAGE_SIZE_Z-1){
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
	//動かせないならnowBlockPosを初期に戻してfalseを返す
	//動ける場合は、centerposも更新
	public static bool CouldMoveBlock(string str)
	{
		//一度tmpに保存
		Vector3[] tmpBlockPos = new Vector3[GameController.nowBlockPos.Length];
		for (int i = 0; i < tmpBlockPos.Length; i++) {
			tmpBlockPos [i] = GameController.nowBlockPos [i];
		}
		for (int i = 0; i < tmpBlockPos.Length; i++) {
		
			switch (str) {
			case "drop":
				GameController.nowBlockPos [i] += new Vector3 (0, -1, 0);
				break;
			case "left":
				GameController.nowBlockPos[i] += new Vector3 (-1, 0, 0);
				break;
			case "right":
				GameController.nowBlockPos[i] += new Vector3 (1, 0, 0);
				break;
			case "up":
				GameController.nowBlockPos[i] += new Vector3 (0, 0, 1);
				break;
			case "down":
				GameController.nowBlockPos[i] += new Vector3 (0, 0, -1);
				break;
			case "rotate_y":
				int centerx = (int)GameController.nowBlockPos [0].x;
				int centery = (int)GameController.nowBlockPos [0].y;
				int centerz = (int)GameController.nowBlockPos [0].z;
				int rerativex = (int)GameController.nowBlockPos [i].x - centerx;
				int rerativey = (int)GameController.nowBlockPos [i].y - centery;
				int rerativez = (int)GameController.nowBlockPos [i].z - centerz;
				int ansx, ansz;
				if (rerativex == 1) {
					ansz = 1;
				} else if (rerativex == -1) {
					ansz = -1;
				} else {
					ansz = 0;
				}
				if (rerativez == 1) {
					ansx = -1;
				} else if ( rerativez == -1) {
					ansx = 1;
				} else {
					ansx = 0;
				}
				GameController.nowBlockPos [i] = GameController.nowBlockPos [0] + new Vector3 (ansx, rerativey, ansz);		
				break;

			default:
				break;
			}
			int posx = (int)GameController.nowBlockPos[i].x;
			int posy = (int)GameController.nowBlockPos[i].y;
			int posz = (int)GameController.nowBlockPos[i].z;

			//もし動かせなかったらもどす
			if (stage[posx,posy,posz] == 1) {
				//ダメなら保存したtmpを入れて終わり
				for (int j = 0; j < tmpBlockPos.Length; j++) {
					GameController.nowBlockPos [j] = tmpBlockPos [j];
				}
				return false;
			}
		}
		//動かせたらnowBlockPosはそのままにしてtrue
		return true;
	}

	//システムの座標の移動
	//ブロックの座標の移動はmoveBlock
	public static void confirm_stage() 
	{
		for (int i = 0; i < GameController.nowBlockPos.Length; i++) {
			int posx = (int)GameController.nowBlockPos [i].x;
			int posy = (int)GameController.nowBlockPos [i].y;
			int posz = (int)GameController.nowBlockPos [i].z;
			StageState.stage [posx, posy, posz] = 1;
		}
	}

	public static List<int> findFill()
	{
		//ブロックの置けるエリア内が全て1の時はlistにappend
		List<int> filledList = new List<int>();
		for (int i = 1; i < STAGE_SIZE_Y; i++){
			int count = 0;
			for (int j = 0; j < STAGE_SIZE_X; j++){
				for (int k = 0; k < STAGE_SIZE_Z; k++){
					if (stage [j, i, k] == 1)
						count++;
				}
			}
			if (count > 30/*== STAGE_SIZE_X * STAGE_SIZE_Z*/)
				filledList.Add(i);
		}
		return filledList;
	}

	//ブロックの座標移動
	//システムの座標は移動しない
	//システムの座標の移動はconfirm_stage
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
		case "rotate_y":
			GameController.nowBlock.transform.Rotate (new Vector3 (0, 1, 0), 90);
			break;
		default:
			break;
		}
	}

	public static void DeleteRaw(int raw)
	{

				for (int i = 1; i < STAGE_SIZE_X - 1; i++) {
					for (int j = 1; j < STAGE_SIZE_Z - 1; j++) {
				stage [i, raw, j] = 0;
					}
				}
//		for (int i = 1; i < STAGE_SIZE_X - 1; i++) {
//			for (int j = 1; j < STAGE_SIZE_Z - 1; j++) {
//				for (int k = raw; k < STAGE_SIZE_Y-1; k++) {
//					stage [i, k, j] = stage [i, k+1, j];
//				}
//			}
//		}
//		for (int i = 1; i < STAGE_SIZE_X - 1; i++) {
//			for (int j = 1; j < STAGE_SIZE_Z - 1; j++) {
//				stage [i, STAGE_SIZE_Y - 1, j] = 0;
//			}
//		}
	}
		
}
