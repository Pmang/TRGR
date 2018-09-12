using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * 
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class Pulser : MonoBehaviour {

	SpriteRenderer image;
	Color color;
	bool isRunning;


	// Use this for initialization
	void Start () {
		this.image = GameObject.Find("PressSpace").GetComponent<SpriteRenderer>();
		this.color = this.image.color;
		this.isRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isRunning)
		{
			StartCoroutine(Off());
		}
	}

	IEnumerator Off()
	{
		this.isRunning = true;
		yield return new WaitForSeconds(0.8f);

		this.color.a = 0f;
		this.image.color = this.color;

		StartCoroutine(On());
	}

	IEnumerator On()
	{
		yield return new WaitForSeconds(0.8f);

		this.color.a = 1f;
		this.image.color = this.color;
		this.isRunning = false;
	}
}
