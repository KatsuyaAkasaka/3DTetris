using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBlocks : MonoBehaviour {

	const float drop_interval = 2f;		//ブロックの落ちるスピード
	private float timer = 0f;		//タイマー
	const int STAGE_SIZE_X = 8;		//stageのサイズ(8,7,8)
	const int STAGE_SIZE_Y = 7;
	const int STAGE_SIZE_Z = 8;
	Vector3 down_amount = new Vector3(0f, 0.08f, 0f);
	private bool finished_this_obj = false;
	//private GameObject test;
	//private Text t;

	public static bool confirmed = true;		//stageが確定したらtrue。それによって新しくブロックが生成されたらfalse



	void Start()
	{
		//test = GameObject.Find ("abletodrop");
//		t = test.GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!finished_this_obj) {
			if (!confirmed) {
				timer += Time.deltaTime;
				if (timer > drop_interval) {		//stage確定してからdrop_interval秒後にdrop_down
					drop_down ();
					timer = 0f;
				}
			}
		}
	}


	//drop_down()に関数が入らないのでUpdateが読み込まれていない？
	//ステージを全探索して、現在落下中のブロックを見つけたら一つ下に落とす
	void drop_down()
	{
		//落とせるかどうか確認
		//何かに引っかかったら、2を全て1にして確定。落とせたら、座標移動させる
		if (StageState.CouldMoveBlock ("drop")) {
			//落とせたら、次のdrop_intervalまで暫定の2のままにさせておく
			GameController.nowBlock.transform.position -= down_amount;
		} else {
			//落とせなかったらstageの2を全て1にしてfinish
			StageState.confirm_stage ();
			confirmed = true;
			finished_this_obj = true;
			Debug.Log ("確定");
		} 
	//	t.text = able_to_drop.ToString();
	}

}
