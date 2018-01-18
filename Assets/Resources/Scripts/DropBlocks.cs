using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBlocks : MonoBehaviour
{

	const float drop_interval = 1f;
	//ブロックの落ちるスピード
	private float timer = 0f;
	//タイマー
	const int STAGE_SIZE_X = 8;
	//stageのサイズ(8,7,8)
	const int STAGE_SIZE_Y = 7;
	const int STAGE_SIZE_Z = 8;
	Vector3 down_amount = new Vector3 (0f, 0.08f, 0f);
	private bool finished_this_obj = false;

	bool isRunning = false;
	//private GameObject test;
	//private Text t;


	public static bool confirmed = true;
	//stageが確定したらtrue。それによって新しくブロックが生成されたらfalse


	// Update is called once per frame
	void Update ()
	{
		if (!finished_this_obj) {
			if (!confirmed) {
				timer += Time.deltaTime;
				if (timer > drop_interval) {		//stage確定してからdrop_interval秒後にdrop_down
					StartCoroutine(drop_down ());
					timer = 0f;
				}
			}
		}
	}


	//ステージを全探索して、現在落下中のブロックを見つけたら一つ下に落とす
	IEnumerator drop_down ()
	{
		//落とせるかどうか確認
		if (StageState.CouldMoveBlock ("drop")) {
			StageState.MoveBlock ("drop");
			yield return null;
			//無理ぽならステージ確定させて、消せるrawを消して、このオブジェクトの動作を終了させる
		} else {
			if (isRunning)
				yield break;
			isRunning = true;
			StageState.confirm_stage ();
			List<int> filledlist = StageState.findFill ();
			int count = 0;
			foreach(int i in filledlist) {
				Debug.Log (i);
				//システム的削除(Out of Range Error)
				StageState.DeleteRaw (i-count);
				//物理的削除(Out of Range Error)
				DeleteBlocks.delete (i-count);
				count++;
				yield return new WaitForSeconds (drop_interval);
			}

			confirmed = true;
			finished_this_obj = true;
//			for (int i = 1; i < STAGE_SIZE_X-1; i ++){
//				for (int j = 1; j < STAGE_SIZE_Y; j++){
//					for (int k = 1; k < STAGE_SIZE_Z-1; k++){
//						if(StageState.stage[i,j,k] != 0)
//							Debug.Log(i + "," + j + "," + k + ", = " + StageState.stage[i,j,k]);
//					}
//				}
//			}
//			Debug.Log ("-------------------------------");
		}

	}

}
