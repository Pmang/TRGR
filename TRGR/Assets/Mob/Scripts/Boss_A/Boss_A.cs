using UnityEngine;
using System.Collections;

/*
 * ボスの基本的な動作を行うスクリプトです。
 * 2015/5/22 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Boss_A : MobBase
{

    //ステータス
    public int hp = 300;
    public float speed = 0.009f;
    public float gravity = 0;


    //現在のプレイヤークラス
    Player player;

     //武器オブジェクトを取得
    GameObject wepon;

    //突進時のエフェクトオブジェクト
    GameObject tackle;

    //プレイヤーの座標
    public Vector3 endPosithion;

    //各攻撃後のクールタイム
    public float coolTime;

    public float rimit = 3f;

    //Attack2用のカウンター
    public int a2;

    //各攻撃範囲に入っているかどうか
    public bool isAttack_A;
    public bool isAttack_B;
    public bool isAttack_C;

    //アニメーション
    private Animator anime;

    //アニメーションステートに渡す値を保持
    public int stayId;
    public int attack1Id;
    public int attack2Id;
    public int attack3Id;
    public int attack4Id;

    //現在攻撃中かどうか
    public bool isAttack_now;

    //Bossのstate
    public enum State
    {
        Normal,
        Attack,

    }

    public State state;


	// 初期化、最初に行う処理
	public override void Start()
	{
		// MobBaseクラスのStart呼び出し
		base.Start();

        //プレイヤーの現在位置を取得したいのでオブジェクトから取得
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        //生成するゲームオブジェクトをプレハブから読み込む
        wepon = (GameObject)Resources.Load("Prefabs/Attack1_ef");
        tackle = (GameObject)Resources.Load("Prefabs/Attack4_ef");

		//ヒールオブジェクトを取得
		heal = (GameObject)Resources.Load("Prefabs/heal_item");

        anime = transform.root.gameObject.GetComponent<Animator>();
        stayId = Animator.StringToHash("stay");
        attack1Id = Animator.StringToHash("attack1");
        attack2Id = Animator.StringToHash("attack2");
        attack3Id = Animator.StringToHash("attack3");
        attack4Id = Animator.StringToHash("attack4");

        //開幕から攻撃はしませんね
        isAttack_A = false;
        isAttack_B = false;
        isAttack_C = false;

        //ボスは基本攻撃
        state = State.Normal;

        //Attack2用のカウンターを初期化
        a2 = 1;

    }

	// Update is called once per frame
	public override void Update()
	{
		if (!GameObject.Find("StageManager").GetComponent<StageManager>().isFreeze)
		{

			// MobBaseクラスのUpdate呼び出し
			base.Update();

			if (hp < 0)
			{
				// MobBaseクラスのDepop呼び出し
				base.Depop();
			}


			//攻撃中は向きを変更しない
			if (isAttack_now)
			{
				//何もしない
			}
			else
			{
				//移動先に応じて画像の向きを変える
				if (this.transform.position.x - player.transform.position.x < 0)
				{
					//画像が反転しているかどうか
					if (transform.localScale.x < 0)
					{
						//そのまま
					}
					else
					{
						//反転する
						transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z - 1);
					}

				}
				else
				{
					//画像が反転しているかどうか
					if (transform.localScale.x < 0)
					{
						//反転する
						transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z - 1);
					}
					else
					{
						//そのまま
					}
				}
			}

			if (isAttack_now)
			{
				//クールタイム中は攻撃をしない
				Cool();
			}
			else
			{
				//ステートを攻撃に変更
				state = State.Attack;

				switch (state)
				{

					case State.Normal:
						break;


					case State.Attack:
						Attacking();
						break;


				}
			}
		}

    }


    void Attacking()
    {
        if (isAttack_A)
        {
            //攻撃Aのアニメーションを再生
            anime.SetTrigger(attack1Id);
            

        }
        else if (isAttack_B)
        {
            //攻撃Bのアニメーションを再生
            anime.SetTrigger(attack2Id);
            

        }
        else if (isAttack_C)
        {
            //攻撃Cのアニメーションを再生
            anime.SetTrigger(attack3Id);
            
        }
        else
        {
            
            
            //攻撃Dのアニメーションを再生
            anime.SetTrigger(attack4Id);
            
        }


    }

    void Attack1()
    {
        //新しい遠距離武器のオブジェクトを生成
        Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z), wepon.transform.rotation);
    }


    void Attack2()
    {
        switch (a2)
        {
            case 1:
                //新しい遠距離武器のオブジェクトを生成
                Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z + 3), wepon.transform.rotation);
                break;

            case 2:
                Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z + 1), wepon.transform.rotation);
                break;

            case 3:
                Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z), wepon.transform.rotation);
                break;

            case 4:
                Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z - 1), wepon.transform.rotation);
                break;

            case 5:
                //最後まで来たらカウンターを0に戻す
                Instantiate(wepon, new Vector3(transform.position.x, player.transform.position.y, transform.position.z - 3), wepon.transform.rotation);
                a2 = 0;
                break;
        }
        //カウンターを1増やす
        a2++;

    }


    void Attack3()
    {
        //自身を基点に遠距離モブを2匹召喚
		GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Pop(Enemy.S, 2, this.transform.position);
    }

    void Attack4()
    {
        //突進先をエフェクトに渡す為プレイヤーの現在地を取得
        endPosithion = player.transform.position;

        //突進エフェクトを生成(その後の動作はエフェクトが持つスクリプトで行う)
        Instantiate(tackle, this.transform.FindChild("AttackManager").position, transform.rotation);



    }

    //一定時間以上経ったら攻撃に移れる(つまりクールタイム)
    void Cool()
    {
        coolTime += Time.deltaTime;

        if (coolTime > rimit)
        {
            isAttack_now = false;
			anime.SetBool(stayId, false);
            coolTime = 0f;
        }   
    }

    //クールタイムを発生させる
    public void CoolOn()
    {
        coolTime = 0f;
		anime.SetBool(stayId, true);
        isAttack_now = true;
    }


    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //Weponタグの場合はダメージを受ける
        if (other.tag == "Wepon")
        {
            int damage = new System.Random().Next(player.attack - 5, player.attack + 5);
            hp -= damage;
            Debug.Log(damage + "のダメージ   " + "残りHP=   " + hp);
			StartCoroutine(Flush());
        }
    }
}
