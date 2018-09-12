using UnityEngine;
using System.Collections;

/*
 * ゲーム画面上のタイマーの処理を管理するスクリプトです。
 * 2015/5/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class Speed : MonoBehaviour {

	private TimerApi api;

	// Use this for initialization
	void Start () {
		this.api = GameObject.Find("Canvas").GetComponent<TimerApi>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSlow()
	{
		this.api.Mode = TimerMode.SLOW;
	}

	public void OnNormal()
	{
		this.api.Mode = TimerMode.NORMAL;
	}

	public void OnFast()
	{
		this.api.Mode = TimerMode.FAST;
	}
}
