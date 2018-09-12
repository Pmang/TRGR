using UnityEngine;
using System.Collections;

/*
 * モブの共通処理を一括して行うスクリプトです。
 * 2015/6/24  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class MobBase : MonoBehaviour
{
	private TimerApi timer;

	private EnemyManager em;

	// デポップの最中かどうか
	private bool isDropping;

	private Color color;

	protected float healValue
	{
		set;
		get;
	}

	//回復アイテム
	protected GameObject heal;

	public virtual void Start()
	{
		this.timer = GameObject.Find("Canvas").GetComponent<TimerApi>();
		this.em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
		this.color = this.GetComponent<SpriteRenderer>().color;
		this.sm = GameObject.Find("StageManager").GetComponent<StageManager>();
		this.healValue = 3f;
	}

	public virtual void Update()
	{
		if (isDropping)
		{
			this.GetComponent<SpriteRenderer>().color = this.color;
		}
	}

	// Mobのデポップ
	public void Depop()
	{
		if (isDropping)
			return;

        System.Type type = this.GetType();

		// ボスでなければ新しいヒールアイテムのオブジェクトを生成する
        if (type != typeof(Boss_A) && type != typeof(Boss_B) && type != typeof(Boss_C) && type != typeof(Mob_Y))
		{
            Instantiate(heal, new Vector3(transform.position.x, heal.transform.position.y, transform.position.z), transform.rotation);
            
            // 3秒制限時間回復
            BgmManager.Instance.SePlay("Timer");
            this.timer.Heal(healValue);
            this.isDropping = true;
		}




		// フェードアウト開始
		StartCoroutine(FadeOut());
	}

	// デポップ時にフェードアウトさせるコルーチン
	private IEnumerator FadeOut()
	{
		float interval = 0.9f;
		float time = 0;

		while (time <= interval)
		{
			this.color.a = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		this.isDropping = false;
		this.em.Depop(this);
	}

	// ダメージ受けた時に赤点滅させるコルーチン
	protected IEnumerator Flush()
	{
		Color color = new Color(1f, 1f, 1f);
		Color rcolor = new Color(1f, 0f, 0f);
		SpriteRenderer r = this.GetComponent<SpriteRenderer>();

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

	// ダメージの計算
	public static int calcDamage(int minAtk, int maxAtk)
	{
		float dmg = maxAtk - minAtk;
		dmg = dmg * Random.value;

		return (int)(minAtk + dmg);
	}

	public StageManager sm { get; set; }
}
