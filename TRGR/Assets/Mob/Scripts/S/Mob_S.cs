using UnityEngine;
using System.Collections;

/*
 * モブの基本的な動作を行うスクリプトです。
 * 2015/6/2 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Mob_S : MobBase
{
	//ステータス
	public int hp = 100;
	public float speed = 0.003f;
	public float gravity = 0;

	//初期位置
	private Vector3 startPosithon;

	//現在地
	private Vector3 nowPosithion;

	//目的地
	private Vector3 endPosithion;

	//目的地までの距離
	private Vector3 directhion;

	//現在地と目的地の差
	private float distance;

	//移動速度
	public Vector3 moveSpeed;

	//目標地点に到着したとする距離（プレイヤーと全く同じ位置まで動くと衝突し合う為）
	public const float endPoint = 0.4f;

	//現在のプレイヤークラス
	private Player m;

	//遠距離武器のオブジェクト
	GameObject wepon;

	//経過時間
	float Interval = 1.0f;

	private Animator anime;


	//mobのstate
	public enum State
	{
		Normal,
		Moveing,
		Damaged,
		Attack,
		Returning
	}

	public State state = State.Normal;

	// 初期化、最初に行う処理
	public override void Start()
	{
		// MobBaseクラスのStart呼び出し
		base.Start();

		//プレイヤーの現在位置を取得したいのでフィールドを取得
		m = GameObject.FindWithTag("Player").GetComponent<Player>();

		//初期位置を保管（元の位置に戻る事があるので）
		startPosithon = this.transform.position;

		//移動先をとりあえず現在地に
		endPosithion = this.transform.position;

		//武器オブジェクトを取得
		wepon = (GameObject)Resources.Load("Prefabs/WeponObject_A");
		
		//ヒールオブジェクトを取得
		heal = (GameObject)Resources.Load("Prefabs/heal_item");
	}

	// Update is called once per frame
	public override void Update()
	{
		// MobBaseクラスのUpdate呼び出し
		base.Update();

		//経過時間を更新
		Interval -= Time.deltaTime;

		if (hp < 0)
		{
			// MobBaseクラスのDepop呼び出し
			base.Depop();
		}

		switch (state)
		{

			case State.Normal:
				break;

			case State.Moveing:
				Moveing();
				break;

			case State.Damaged:
				Damage();
				break;

			case State.Attack:

				//経過時間が一定の感覚であれば攻撃
				if (Interval <= 0.0f)
				{
					Interval = 1.0f;
					Attacking();
				}
				break;

			case State.Returning:
				Returning();
				break;
		}
	}

	void Returning()    //元の位置に戻る
	{
		//自分の位置を取得
		nowPosithion = this.transform.position;

		//目的地と現在地の距離と方角の差を求める
		directhion = startPosithon - nowPosithion;

		//移動量を求める
		moveSpeed = directhion * speed;

		//描画する位置を調整する(つまり移動する)
		Vector3 p = this.transform.position;
		p = this.transform.position = new Vector3(p.x + moveSpeed.x, p.y, p.z + moveSpeed.z);
	}

	void Moveing()      //プレイヤーに忍び寄る
	{
		//自分の位置を取得
		nowPosithion = this.transform.position;

		//プレイヤーの位置を取得して目的地とする
		endPosithion = m.position;

		//目的地と現在地の距離と方角の差を求める
		directhion = endPosithion - nowPosithion;

		//移動量を求める
		moveSpeed = directhion * speed;

		//描画する位置を調整する(Sは縦移動のみ行う)
		Vector3 p = this.transform.position;
		p = this.transform.position = new Vector3(p.x, p.y, p.z + moveSpeed.z);
	}

	void Damage()
	{

	}

	void Attacking()
	{
		//新しい遠距離武器のオブジェクトを生成
		Instantiate(wepon, transform.position, transform.rotation);
	}

	//範囲内に何かが入ると動く
	private void OnTriggerEnter(Collider other)
	{
		//プレイヤータグの場合はダメージを与えて消す
		if (other.tag == "Wepon")
		{
			int damage = new System.Random().Next(m.attack - 5, m.attack + 5);
			hp -= damage;
			StartCoroutine(Flush());
		}
	}
}
