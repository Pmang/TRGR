using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

/*
 * ゲーム画面上のHPバーの処理を管理するスクリプトです。
 * 2015/5/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

/// <summary>
/// HPゲージのAPI
/// </summary>
public class HpApi : MonoBehaviour
{
	public const int MAXHP = 1000;

	/// <summary>
	/// 赤い方のゲージ
	/// </summary>
	private Slider slider;

	/// <summary>
	/// オレンジ色のゲージ
	/// </summary>
	private Slider slider_back;

	/// <summary>
	/// DecreaseHp() が動いているかどうか
	/// </summary>
	private bool isRunning;

	/// <summary>
	/// キャラクターのHP　キャラクターのHPとリンクさせてください
	/// </summary>
	public float Hp
	{
		set
		{
			slider.value = value;
		}

		get
		{
			return slider.value;
		}
	}

	// Use this for initialization
	void Start()
	{
		// スタート時に各オブジェクトを取得
		this.slider = GameObject.Find("Slider Front").GetComponent<Slider>();
		this.slider_back = GameObject.Find("Slider Back").GetComponent<Slider>();

		// 各ゲージをキャラクターのHP値で初期化
		this.slider.maxValue = MAXHP;
		this.slider.value = MAXHP;
		this.slider_back.maxValue = MAXHP;
		this.slider_back.value = MAXHP;

		this.isRunning = false;

	}

	// Update is called once per frame
	void Update()
	{
		if (Hp <= 0 && !WorldManager.Instance.isGameOver)
		{
			WorldManager.Instance.isGameOver = true;
			FadeManager.Instance.LoadLevel("gameover", 0.9f);
		}
	}

	/// <summary>
	/// ゲージをダメージ分減らします
	/// </summary>
	/// <param name="damage">減らしたいダメージ量　マイナスの値を指定すると回復する</param>
	public void Damage(float damage)
	{
		// ヒールでHP最大値超えないように
		if (damage < 0 && this.Hp + damage > MAXHP)
		{
			this.Hp = MAXHP;
		}

		this.Hp -= damage;
		StartCoroutine(Flush());

		// コルーチンが動いてなくてかつ赤ゲージが橙ゲージを下回った時
		if (!this.isRunning && this.Hp < this.slider_back.value)
		{
			// ディクリースコルーチンを開始する
			this.isRunning = true;
			StartCoroutine(DecreaseHp());
		} // ヒール用 赤ゲージと橙ゲージをあわせる
		else if (this.Hp > this.slider_back.value)
		{
			this.slider_back.value = this.Hp;
		}
	}

	/// <summary>
	/// ゲージを少しづつ増やす
	/// </summary>
	/// <returns></returns>
	private IEnumerator IncreaseHp()
	{
		yield return new WaitForSeconds(1f + Time.deltaTime);

		while (this.Hp < this.slider_back.value)
		{
			this.slider_back.value += MAXHP / 128f;
			yield return new WaitForSeconds(0f);
		}

		this.isRunning = false;
	}

	/// <summary>
	/// 橙ゲージを少しづつ減らす
	/// </summary>
	/// <returns></returns>
	private IEnumerator DecreaseHp()
	{
		yield return new WaitForSeconds(1f + Time.deltaTime);

		while (this.Hp < this.slider_back.value)
		{
			this.slider_back.value-=MAXHP/128f;
			yield return new WaitForSeconds(0f);
		}

		this.isRunning = false;
	}

	// ダメージ受けた時に赤点滅させるコルーチン
	protected IEnumerator Flush()
	{
		Color color = new Color(1f, 1f, 1f);
		Color rcolor = new Color(1f, 0f, 0f);
		SpriteRenderer r = GameObject.Find("Player").GetComponent<SpriteRenderer>();

		for (int i = 0; i < 3; i++)
		{
			r.color = rcolor;
			yield return new WaitForSeconds(0.08f);
			//yield return 0;
			r.color = color;
			yield return new WaitForSeconds(0.08f);
		}

		yield return 0;
	}
}
