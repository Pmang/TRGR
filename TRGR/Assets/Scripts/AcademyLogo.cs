using UnityEngine;
using System.Collections;

/*
 * ゲーム開始時に学校のロゴを表示するスクリプトです。
 * 2015/6/27  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class AcademyLogo : MonoBehaviour {

	float time;
	bool timeArrived;

	// Use this for initialization
	void Start () {
		time = Time.time;
		timeArrived = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - time > 2f && !timeArrived)
		{
			timeArrived = true;
			FadeManager.Instance.LoadLevel("teamlogo", 0.9f);
		}
	}
}
