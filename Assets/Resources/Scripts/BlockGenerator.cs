using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour {

	const float interval = 1f;
	private float timer = 0f;
	public GameObject T, O, S, I, L;
	private GameObject stage;
	private Vector3 generateVec;	//生成されるブロックの座標(transform.position)
	private Vector3 generatePos = new Vector3 (4, 6, 4);	//生成されるブロックの位置

	Vector3[] blockpos;


	// Use this for initialization
	void Start () 
	{
		stage = (GameObject)Resources.Load ("Prefabs/Stage");
		generateVec = stage.transform.Find ("generatePos").gameObject.transform.position;
	}



	// Update is called once per frame
	void Update () 
	{
		if (GameController.isGameStarted && DropBlocks.confirmed) {		//ブロックがintervalごとに生成される
			timer += Time.deltaTime;
			if (interval < timer) {		//時間になったら新たなブロック生成
				generate_block (randomG());
				timer = 0;
				DropBlocks.confirmed = false;
			}
		}
	}

	GameObject randomG()
	{
		int r = Random.Range (1, 6);
		switch (r) {
		case 1:
			return T;
		case 2:
			return O;
		case 3:
			return S;
		case 4:
			return I;
		case 5:
			return L;
		default:
			return T;
		}
	}


	//blockがgeneratePosに生成される
	void generate_block(GameObject block)
	{
		GameController.nowBlock = Instantiate (block, generateVec, Quaternion.identity) as GameObject;
		data_maker (block.name);
	}



	//それぞれのブロックに応じた形のベクトルを作成する
	//回転中心はこの作成した配列の[0]になる
	void data_maker(string name)
	{
		switch (name) {
		case "Block-T":
			blockpos = new Vector3[] {
				generatePos, 
				generatePos + create_vec (0, -1, 0),
				generatePos + create_vec (-1, -1, 0),
				generatePos + create_vec (1, -1, 0)
			};
			break;

		case "Block-O":
			blockpos = new Vector3[] {
				generatePos,
				generatePos + create_vec (1, 0, 0),
				generatePos + create_vec (0, -1, 0),
				generatePos + create_vec (1, -1, 0)
			};
			break;

		case "Block-S":
			blockpos = new Vector3[] {
				generatePos,
				generatePos + create_vec (0, -1, 0),
				generatePos + create_vec (-1, -1, 0),
				generatePos + create_vec (-1, -2, 0)
			};
			break;
		case "Block-I":
			blockpos = new Vector3[] {
				generatePos,
				generatePos + create_vec (0, -1, 0),
				generatePos + create_vec (0, -2, 0),
				generatePos + create_vec (0, -3, 0)
			};
			break;
		case "Block-L":
			blockpos = new Vector3[] {
				generatePos,
				generatePos + create_vec (0, -1, 0),
				generatePos + create_vec (0, -2, 0),
				generatePos + create_vec (1, -2, 0)
			};
			break;
		}

		//static変数に代入して参照できるように
		GameController.nowBlockPos = blockpos;
	}


	//ベクトルを作成
	Vector3 create_vec(int x, int y, int z)
	{
		return new Vector3 (x, y, z);
	}


	//渡された配列に入っているベクトルの位置を2に書き換える
//	void stage_update(Vector3[] ary)
//	{
//		for (int i = 0; i < ary.Length; i++) {
//			StageState.stage [(int)(ary[i].x), (int)(ary[i].y), (int)(ary[i].z)] = 2;
//		}
//	}
}
