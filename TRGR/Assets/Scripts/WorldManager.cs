using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * 各シーンの切り替えを行うスクリプトです。
 * 2015/6/27  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class WorldManager : SingletonMonoBehaviour<WorldManager> {

	public String enteredStage
	{
		set;
		get;
	}

	public String playBgm
	{
		set;
		get;
	}

	public bool isGameOver
	{
		set;
		get;
	}

	private bool[] isStageClear;
	private string[] stageName;
	ParticleSystem p;

	private Vector3[] stages = new Vector3[]{new Vector3(15.5f, 5.6f, 6.0f), new Vector3(16.7f, 7.4f, 6.0f),
		new Vector3(18.6f, 8.2f, 6.0f), new Vector3(19.8f, 8.9f, 6.0f), new Vector3(21.9f, 10.0f, 6.0f)};

	private GameObject obj;
	private Text StageLabel;
	private int selected;

	private bool onceFlag = false;

	// Use this for initialization
	void Awake ()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		// Resource/Audio/BGM階層内のサウンドファイルをすべて読み込む

		this.isStageClear = new bool[] { true, true, true, true, true, false };
		this.stageName = new string[] {"Stage 1-1", "Stage 1-Boss", "Stage 2-1", "Stage 2-Boss", "Stage 3-1", "Stage Last" };
		this.isGameOver = false;

		this.obj = GameObject.Find("Player");
		this.selected = 0;
		obj.transform.position = stages[selected];
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < this.isStageClear.Length; i++)
		{
			if (Application.loadedLevelName == "select" && isStageClear[i])
			{
				p = GameObject.Find(this.stageName[i]).GetComponent<ParticleSystem>();
				p.startColor = new Color(1f, 1f, 1f);
				// this.isStageClear[i] = false;
			}
		}

		if (Application.loadedLevelName == "select")
		{
			if (BgmManager.Instance.GetPlayingBgmName() != "Select")
			{
				BgmManager.Instance.Play("Select", true);
			}

			if (obj == null)
			{
				this.onceFlag = false;
				this.obj = GameObject.Find("Player");
				if (selected >= stages.Length)
				{
					obj.transform.position = stages[selected - 1];
				}
				else
				{
					obj.transform.position = stages[selected];
				}
			}

			if (StageLabel == null)
			{
				this.StageLabel = GameObject.Find("StageLabel").GetComponent<Text>();
				StageLabel.text = this.stageName[selected];
			}

			if (!onceFlag)
			{
				if (Input.GetButtonDown("Cancel"))
				{
					onceFlag = true;
					BgmManager.Instance.SePlay("Return");
					FadeManager.Instance.LoadLevel("title", 0.9f);
				}

				if (Input.GetButtonDown("Submit"))
				{
					onceFlag = true;
					BgmManager.Instance.SePlay("Decide");
					switch (this.selected)
					{
						case 0:
							this.enteredStage = "stage1-1";
							this.playBgm = "Stage1_1";
							break;
						case 1:
							this.enteredStage = "stage1-boss";
							this.playBgm = "Stage1_Boss";
							//FadeManager.Instance.LoadLevel("stage", 0.9f);
							break;
						case 2:
							this.enteredStage = "stage2-1";
							this.playBgm = "Stage2_1";
							//FadeManager.Instance.LoadLevel("", 0.9f);
							break;
						case 3:
							this.enteredStage = "stage2-boss";
							this.playBgm = "Stage2_Boss";
							//FadeManager.Instance.LoadLevel("", 0.9f);
							break;
						case 4:
							this.enteredStage = "stage3-1";
							this.playBgm = "Stage3_1";
							//FadeManager.Instance.LoadLevel("", 0.9f);
							break;
						case 5:
							this.enteredStage = "stage3-boss";
							this.playBgm = "LassBoss";
							//FadeManager.Instance.LoadLevel("", 0.9f);
							break;

					}
					FadeManager.Instance.LoadLevel("stagetutrial", 0.9f);
				}
			}

			if (Input.GetButtonDown("LEFT"))//Input.GetAxis("Horizontal") < 0f)
			{
				if (this.selected >= stages.Length)
				{
					GameObject.Find("Stage 3-1").GetComponent<ParticleSystem>().startColor = new Color(184 / 255f, 0, 112 / 255f);
				}

				this.selected--;
				if (this.selected < 0)
					this.selected = 0;

				obj.transform.position = stages[selected];

				StageLabel.text = this.stageName[selected];
			}
			else if (Input.GetButtonDown("RIGHT"))//(Input.GetAxis("Horizontal") > 0f)
			{
				if (WorldManager.Instance.isStageCleared(this.selected))
				{
					this.selected++;
					if (this.selected >= stages.Length)
					{
						GameObject.Find("Stage 3-1").GetComponent<ParticleSystem>().startColor = new Color(38 / 255f, 0, 23 / 255f);
						this.selected = stages.Length;// stages.Length;
						StageLabel.text = this.stageName[selected];
					}
					else
					{
						obj.transform.position = stages[selected];
						StageLabel.text = this.stageName[selected];
					}
				}

				
			}
		}
	}

	public void ChangeStageState(int stage, bool clearFlag)
	{
		this.isStageClear[stage] = clearFlag;
	}

	public bool isStageCleared(int stage)
	{
		return this.isStageClear[stage];
	}

	public void Initialize()
	{
		this.isStageClear = new bool[] { true, true, true, true, true, false };
		this.selected = 0;
		this.isGameOver = false;
	}
}
