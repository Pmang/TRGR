using UnityEngine;
using System.Collections;

/*
 * チュートリアル時の処理を行うスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class Loading : MonoBehaviour {

	private float span;

	public Sprite selecttut;
	public Sprite stagetut;
	public Sprite nowloading;
	public Sprite pressanykey;

	SpriteRenderer image;
	SpriteRenderer titleImage;
	Color color;
	bool isRunning;

	// Use this for initialization
	void Start () {
		this.span = Time.time;
		this.image = GameObject.Find("PressSpace").GetComponent<SpriteRenderer>();
		this.titleImage = GameObject.Find("TitleImage").GetComponent<SpriteRenderer>();
		this.color = this.image.color;
		this.isRunning = false;
		

		if (Application.loadedLevelName == "selecttutrial")
		{
			BgmManager.Instance.Play("Title", true);
			titleImage.sprite = selecttut;
		}
		else if (Application.loadedLevelName == "stagetutrial")
		{
			BgmManager.Instance.Play("Select", true);
			titleImage.sprite = stagetut;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - this.span > 3)
		{
			this.image.sprite = pressanykey;

			if (!isRunning)
			{
				StartCoroutine(Off());
			}

			if (Input.GetButtonDown("Submit"))
			{
				BgmManager.Instance.SePlay("Decide");

				if (Application.loadedLevelName == "stagetutrial")
				{
					FadeManager.Instance.LoadLevel(WorldManager.Instance.enteredStage, 0.9f);
				}
				else
				{
					FadeManager.Instance.LoadLevel("select", 0.9f);
				}
			}
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
