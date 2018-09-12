using System;
using UnityEngine;
using System.Collections;

/*
 * プレイヤーの動作を管理するスクリプトです。
 * 2015/5/20 ITスペシャリスト学科 情報処理コース 2年 大矢野 克哉(キャラクターの移動、移動できる範囲の管理を担当)
 *           ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気(攻撃の実装を担当)
 *           ITスペシャリスト学科 情報処理コース 2年 大福 祐輔(ゲームシステムの埋め込みを担当)
 */

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(BoxCollider))]

public class Player : MonoBehaviour
{
	//ステータス
    public Vector3 position;
	public float speed = 0.25f;
	public float jump = 100;
	public float gravity = 0;
	public float walkForce = 15f;
	public float maxWalkSpeed = 3f;
    public int attack = 25;

	private StageManager sm;
	
	[NonSerialized]
	public Animator anime;
	
	[NonSerialized]
	public Rigidbody body;

	private bool is_ground = false;
	private float face = 1;

	private bool canimeEnd;

    //残撃を飛ばしていいかの判定
    public bool isUp;

    //現在ダメージを受けるかどうか
    public bool isDamage;

    //残撃オブジェクト
    GameObject w1; //遠距離
    GameObject w2; //通常

	private enum State
	{
		Normal,
		Damaged,
		Attack,
	}
	private State state = State.Normal;

	private const float RAY_LENGTH = 1.0f;
	private const string TERRAIN_NAME = "Terrain";

	void Start()
	{
		this.anime = this.GetComponent<Animator>();
		this.body = this.GetComponent<Rigidbody>();
		this.sm = GameObject.Find("StageManager").GetComponent<StageManager>();
		Physics.gravity = new Vector3(0, this.gravity, 0);

        w1 = (GameObject)Resources.Load("Prefabs/Player_ef1");
        w2 = (GameObject)Resources.Load("Prefabs/Player_ef2");
        isUp = false;

        isDamage = true;
	}

	void FixedUpdate()
	{
		position = this.transform.position;

		if (!this.sm.isFreeze && this.state != State.Damaged)
		{
			this.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetButtonDown("Jump"), false);
		}
	}

	public void Move(float x, float z, bool a, bool cpu)
	{
		//向きを設定
		this.face = x > 0 ? -1 : (x < 0 ? 1 : face);
		this.transform.rotation = Quaternion.Euler(0, (this.face + 1) * 90, this.transform.rotation.z);
		this.anime.SetFloat("Horizontal", x != 0 ? x : (z != 0 ? this.face : 0));

		if (!this.sm.isFreeze || cpu)
		{
			//プレイヤーを移動
			Vector3 p = this.transform.position;

			// 自動移動時は壁無視
			if (!cpu)
			{
				if (this.sm.isBossMode)
				{
					p.z = p.z > 18 ? 18 : (p.z < 2 ? 2 : p.z);	//Z=0以上かつ6未満の範囲でZ軸移動できる
					p.x = p.x > 72 ? 72 : (p.x < 22 ? 22 : p.x);	//X=11以上かつ60未満の範囲でZ軸移動できる
				}
				else
				{
					p.z = p.z > 6 ? 6 : (p.z < 0 ? 0 : p.z);	//Z=0以上かつ6未満の範囲でZ軸移動できる
					p.x = p.x > 144 ? 144 : (p.x < 11 ? 11 : p.x);	//X=11以上かつ60未満の範囲でZ軸移動できる
				}
				this.transform.position = p;
			}

			// 横方向に力を加える
			//if (this.body.velocity.x < maxWalkSpeed)
			{
				this.body.AddForce(Vector3.right * x * walkForce);
			}

			// 縦方向に力を加える
			//if (this.body.velocity.y < maxWalkSpeed)
			{
				this.body.AddForce(Vector3.forward * z * walkForce);
			}

			// 最高速度を上回った場合速度を制限する
			if (Mathf.Abs(this.body.velocity.x) > maxWalkSpeed)
			{
				this.body.velocity = new Vector3(Mathf.Sign(this.body.velocity.x) * maxWalkSpeed,
					this.body.velocity.y, this.body.velocity.z);
			}

			//最高速度を上回った場合速度を制限する
			if (Mathf.Abs(this.body.velocity.z) > maxWalkSpeed)
			{
				this.body.velocity = new Vector3(this.body.velocity.x,
					this.body.velocity.y, Mathf.Sign(this.body.velocity.z) * maxWalkSpeed);
			}
		}

		//アニメーション設定
		this.anime.SetFloat("Vertical", this.body.velocity.y);
		this.anime.SetBool("isGround", this.is_ground);

		if (Input.GetButtonDown("Attack"))
		{
			StartCoroutine(DelaySePlay());
            this.anime.SetTrigger("Attack");
		}
	}

	IEnumerator DelaySePlay()
	{
		yield return new WaitForSeconds(0.5f);
		BgmManager.Instance.SePlay("Swing");
	}

	void Attack()
	{
        if (isUp)
        {
            //速度2倍の場合は通常の攻撃に加え飛ぶ斬撃を生成する
            Instantiate(w1, this.transform.FindChild("PopManager").transform.position, this.transform.rotation);
            Instantiate(w2, this.transform.FindChild("PopManager").transform.position, this.transform.rotation);
        }
        else
        {
            //普通の攻撃のみを生成
            Instantiate(w1, this.transform.FindChild("PopManager").transform.position, this.transform.rotation);
        }
	}

	void OnFinishedInvincibleMode()
	{
		this.state = State.Normal;
	}

	public void OnClearAnimationEnd()
	{
		this.canimeEnd = true;
	}

	public bool IsAnimationEnd()
	{
		return this.canimeEnd;
	}
}
