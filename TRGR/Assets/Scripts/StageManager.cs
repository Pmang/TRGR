using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * ステージ開始時とクリア時の演出の処理を行うスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class StageManager : MonoBehaviour {

	public bool isFreeze;

	public bool isPause;

	public Vector3 startPosition;

	public Vector3 endPosition;

	public Vector3 initialPosition;

	public bool isBossMode;

	private Player p;

	private Image startTelop;

	private Image clearTelop;

	private Color color;

	private bool onceFlag;

	private bool isClear;

	// Use this for initialization
	void Start () {
		this.isFreeze = true;
		this.p = GameObject.Find("Player").GetComponent<Player>();
		this.startTelop = GameObject.Find("StartTelop").GetComponent<Image>();
		this.clearTelop = GameObject.Find("ClearTelop").GetComponent<Image>();
		
		this.initialPosition = this.p.transform.position;
		this.color = this.startTelop.color;
		this.color.a = 0f;
		this.startTelop.color = this.color;
		this.onceFlag = false;
		this.isPause = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Submit"))
		{
			if (!isPause)
			{
				Pauser.Pause();
				FadeManager.Instance.Pause();
				isPause = true;
				isFreeze = true;
			}
			else
			{
				Pauser.Resume();
				FadeManager.Instance.Resume();
				isPause = false;
				isFreeze = false;
			}
		}

		if (!onceFlag)
		{
			StartCoroutine(Starting());
			onceFlag = true;
		}

		if (isClear)
		{
			StartCoroutine(Clearing());
		}
	}

	public void Clear()
	{
		this.isClear = true;
		
		if (Application.loadedLevelName == "stage1-1")
		{
			WorldManager.Instance.ChangeStageState(0, true);
		}
		else if (Application.loadedLevelName == "stage1-boss")
		{
			WorldManager.Instance.ChangeStageState(1, true);
		}
        else if (Application.loadedLevelName == "stage3-boss")
        {
            FadeManager.Instance.LoadLevel("movie_ed", 0.9f);
        }
	}

	private IEnumerator Clearing()
	{
		this.isClear = false;
		this.isFreeze = true;

		yield return new WaitForSeconds(0.5f);
		this.p.anime.SetTrigger(Animator.StringToHash("clear0"));

		this.p.body.velocity = new Vector3(0, this.p.body.velocity.y, Mathf.Sign(this.p.body.velocity.z));
		this.endPosition = new Vector3(this.p.transform.position.x + 10f, this.p.transform.position.y, this.p.transform.position.z);

		BgmManager.Instance.Play("Clear", false);
		
		this.color.a = 1f;
		this.clearTelop.color = this.color;

		while (BgmManager.Instance.IsPlaying() || !this.p.IsAnimationEnd())
		{
			yield return 0;
		}

		this.p.anime.SetTrigger(Animator.StringToHash("clanimeend"));
		this.color.a = 0f;
		this.clearTelop.color = this.color;

		while (this.p.transform.position.x < endPosition.x)
		{
			this.p.Move(0.5f, 0f, false, true);
			yield return 0;
		}

		FadeManager.Instance.LoadLevel("select", 0.9f);
	}

	private IEnumerator Starting()
	{
		yield return new WaitForSeconds(0.1f);
		this.startPosition = new Vector3(this.p.transform.position.x + 10f, this.p.transform.position.y, this.p.transform.position.z);

		while (this.p.transform.position.x < startPosition.x)
		{
			this.p.Move(0.5f, 0f, false, true);
			yield return 0;
		}

		this.p.Move(0f, 0f, false, true);
		this.p.body.velocity = new Vector3(0, this.p.body.velocity.y, Mathf.Sign(this.p.body.velocity.z));

		yield return new WaitForSeconds(0.5f);

		this.color.a = 1f;
		this.startTelop.color = this.color;

		yield return new WaitForSeconds(1f);

		this.color.a = 0f;
		this.startTelop.color = this.color;

		this.p.collider.enabled = true;
		this.isFreeze = false;
	}
}
