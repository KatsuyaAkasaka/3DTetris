using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBlocks : MonoBehaviour {

	const float drop_interval = 1f;		//ブロックの落ちるスピード
	private float timer = 0f;		//タイマー
	const int STAGE_SIZE_X = 8;		//stageのサイズ(8,7,8)
	const int STAGE_SIZE_Y = 7;
	const int STAGE_SIZE_Z = 8;
	Vector3 down_amount = new Vector3(0f, 0.08f, 0f);
	private bool finished_this_obj = false;
	//private GameObject test;
	//private Text t;

	public static bool confirmed = true;		//stageが確定したらtrue。それによって新しくブロックが生成されたらfalse


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


	//ステージを全探索して、現在落下中のブロックを見つけたら一つ下に落とす
	void drop_down()
	{
		//落とせるかどうか確認
		if (StageState.CouldMoveBlock ("drop")) {
			StageState.MoveBlock("drop");
		//無理ぽならステージ確定させて、消せるrawを消して、このオブジェクトの動作を終了させる
		} else {
			StageState.confirm_stage ();
			List<int> filledlist = StageState.findFill ();
			foreach(int i in filledlist) {
				StageState.DeleteRaw (filledlist [i]);
			}
			confirmed = true;
			finished_this_obj = true;
		} 
	}

}
