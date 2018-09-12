using UnityEngine;
using System.Collections;

/*
 * 制作開始時にHPバーのテストを行っていたスクリプトです。
 * 2015/5/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class Heal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		int damage = (int)(Random.value * 100 + 1) * -1;
		HpApi api = GameObject.Find("Canvas").GetComponent<HpApi>();

		api.Damage(damage);
		Debug.Log("hit! " + damage + " healing!");
	}

	public void HealTime()
	{
		int heal = (int)(Random.value * 30 + 1);
		TimerApi api = GameObject.Find("Canvas").GetComponent<TimerApi>();

		api.Heal(heal);
		Debug.Log("yeah! " + heal + " time healing!");
	}
}
