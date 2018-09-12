using UnityEngine;
using System.Collections;

/*
 * ゲームを映しているメインカメラを管理するスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class CameraController : MonoBehaviour {

	/// <summary>
	///  追従するオブジェクト
	/// </summary>
	public GameObject player;

	/// <summary>
	/// 追従させるカメラ
	/// </summary>
	public Camera mainCamera;

	private StageManager sm;

	// Use this for initialization
	void Start () {
		this.sm = GameObject.Find("StageManager").GetComponent<StageManager>();
	}
	
	// Update is called once per frame
	void Update()
	{

		if (!this.sm.isFreeze)
		{
			// ステージセレクトに戻る
			if (Input.GetButtonDown("Cancel"))
			{
				FadeManager.Instance.LoadLevel("select", 0.9f);
			}

			//カメラの位置を設定
			float errorRange = this.player.transform.position.x - this.mainCamera.transform.position.x;

			if (this.sm.isBossMode)
			{
				if (errorRange > 3.0f)
				{
					errorRange = 3.0f;
				}
				else if (errorRange < -3.0f)
				{
					errorRange = -3.0f;
				}
			}
			else
			{
				if (errorRange > 2.0f)
				{
					errorRange = 2.0f;
				}
				else if (errorRange < -4.0f)
				{
					errorRange = -4.0f;
				}
			}

			this.mainCamera.transform.position = new Vector3(this.player.transform.position.x - errorRange, this.mainCamera.transform.position.y, this.mainCamera.transform.position.z);
		}
	}
}