using UnityEngine;
using System.Collections;

/*
 * ゲームオーバー時の処理を行うスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class GameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BgmManager.Instance.Play("GameOver", false);
		WorldManager.Instance.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit"))
		{
			FadeManager.Instance.LoadLevel("title", 0.9f);
		}
	}
}
