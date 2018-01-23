using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBlocks : MonoBehaviour
{

	const float drop_interval = 1f;
	const float next_block_interval = 0.3f;
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

	float f = 0f;


	public static bool confirmed = true;
	//stageが確定したらtrue。それによって新しくブロックが生成されたらfalse

	DeleteBlocks db;

	void Start(){
		db = GameObject.Find("GameManager").GetComponent<DeleteBlocks> ();
	}


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

			List<Transform> destroy_blocks = new List<Transform> ();
			List<Transform> drop_blocks = new List<Transform>();
			GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
			foreach(int i in filledlist) {
				//システム的削除
				StageState.DeleteRaw (i);

				//物理的削除

				foreach (GameObject block in blocks) {
					foreach (Transform t in block.transform) {
						if (t.position.y > -0.2f + (i - 1) * 0.08f - 0.02f && t.position.y < -0.2f + (i - 1) * 0.08f + 0.02f) { 
							Destroy (t.gameObject);
						} 
						if (t.position.y > -0.2f + i * 0.08f - 0.02f) {
							drop_blocks.Add(t);
						}
					}
				}

				if (drop_blocks.Count > 0) {
					int times = 0;
					while (times < 10) {
						drop_blocks.ForEach ((b) => b.position -= new Vector3 (0f, 0.08f / (float)10, 0f));
						yield return new WaitForSeconds (0.03f);
						times++;
					}
					drop_blocks.Clear ();
				}
				yield return new WaitForSeconds (next_block_interval);
			}
			finished_this_obj = true;
			confirmed = true;
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

	IEnumerator drop_coroutin(Transform t)
	{
		int i = 0;
		while(i < 10){
			t.position -= new Vector3 (0f, 0.08f / (float)10, 0f);
			yield return new WaitForSeconds (1f);
			i++;
		}
	}

}
