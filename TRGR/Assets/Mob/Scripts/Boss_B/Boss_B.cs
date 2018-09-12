using UnityEngine;
using System.Collections;
/*
 * ステージ2のボスを管理するスクリプトです
 * 2015/9/3 専門学校ビーマックス 2年 藤田 勇気
 */

public class Boss_B : MobBase {

    //ステータス
    public int hp = 500;
    public float gravity = 0;

    public int maxAtk = 120;
    public int minAtk = 80;

    //クールタイム
    public float coolTime = 3.0f;

    private int attack2_cool = 4;

    private int jump_cool = 2;

    private int guard_cool = 2;

    //Attack2用のカウンター
    public int a2;


    //現在のプレイヤークラス
    Player player;

    //HPを管理するスクリプト
    HpApi hpapi;

    //プレイヤーの座標
    public Vector3 playerPosithion;

    //攻撃モーション中かどうか
    private bool isAttack;

    //各攻撃の範囲判定
    public bool isAttack1;

    public bool isAttack2;

    public bool isGuard;

    //アニメーションステートに渡す値を保持
    private int stayId;
    private int attack1Id;
    private int attack2Id;
    private int guardId;
    private int jumpId;
    private int counterId;

    //攻撃時に生成するエフェクト
    private GameObject slash;
    private GameObject bomb;


    //アニメーター
    private Animator anime;



    //Bossのstate
    public enum State
    {
        Stay,
        Attack,

    }

    public State state;



	// Use this for initialization
    public override void Start()
    {

        // MobBaseクラスのStart呼び出し
        base.Start();

        //プレイヤーの現在位置を取得したいのでオブジェクトから取得
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        //HPを取得
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();

        //ヒールオブジェクトを取得
        heal = (GameObject)Resources.Load("Prefabs/heal_item");

        //最初は攻撃を行わない状態
        state = State.Stay;

        isAttack = false;

        isAttack1 = false;

        isAttack2 = false;

        //各トリガーへ渡すハッシュを取得
        anime = transform.root.gameObject.GetComponent<Animator>();
        stayId = Animator.StringToHash("stay");
        attack1Id = Animator.StringToHash("attack1");
        attack2Id = Animator.StringToHash("attack2");
        guardId = Animator.StringToHash("guard");
        jumpId = Animator.StringToHash("jump");
        counterId = Animator.StringToHash("counter");

        //生成するゲームオブジェクトをプレハブから読み込む
        slash = (GameObject)Resources.Load("Prefabs/BossB_ef1");
        bomb = (GameObject)Resources.Load("Prefabs/BossB_ef2");

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

        }

        //プレイヤー座標の更新
        playerPosithion = player.transform.position;

        //攻撃モーション中でなければプレイヤーの位置に応じて画像の向きを変更
        if (!isAttack)
        {
            //自分のx座標がプレイヤーのx座標よりも小さい場合
            if (transform.position.x < playerPosithion.x)
            {
                //反転済みで無ければ反転
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                }
            }
            else 
            {
                //反転済みで無ければ反転
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                } 
            }
        }

        //stateに応じた分岐
        switch (state)
        {
            //Stayの間はクールタイムの扱いとして動かない
            case State.Stay:

                coolTime -= Time.deltaTime;

                //クールタイムが0以下になった時点で攻撃に戻る
                if (coolTime <= 0)
                {
                    anime.SetBool(stayId, false);
                    coolTime = 3.0f;
                    state = State.Attack;
                }

                break;

            case State.Attack:
                Attacking();
                break;
        }


    }

    void Attacking()
    {
        //攻撃モーション中で無ければいずれかのアニメーションを再生
        if (!isAttack)
        {

            if (isAttack1 && isAttack2)
            {
                //attack2がクールタイム内かどうか
                if (attack2_cool <= 0)
                {
                    //attack2のアニメーションを再生し、クールを戻す
                    anime.SetTrigger(attack2Id);
                    attack2_cool = 4;
                    isAttack = true;
                }
                else
                {
                    //attack1のアニメーションを再生後、各クールを減少
                    anime.SetTrigger(attack1Id);
                    attack2_cool--;
                    jump_cool--;
                    guard_cool--;
                    isAttack = true;
                }
            }
            else if (isAttack1)
            {
                //guardがクールタイム内かどうか
                if (guard_cool <= 0)
                {
                    //guardのアニメーションを再生し、クールを戻す
                    anime.SetTrigger(guardId);
                    guard_cool = 2;
                    isAttack = true;
                }
                else
                {
                    //attack1のアニメーションを再生後、各クールを減少
                    anime.SetTrigger(attack1Id);
                    attack2_cool--;
                    jump_cool--;
                    guard_cool--;
                    isAttack = true;
                }
            }
            else
            {
                //ジャンプのクールが回復済みであればジャンプし、クールを戻す
                if (jump_cool <= 0)
                {
                    anime.SetTrigger(jumpId);
                    jump_cool = 2;
                    isAttack = true;
                }
                else
                {
                    //ガードアニメーションを再生後、各クールを減少
                    anime.SetTrigger(guardId);
                    guard_cool = 2;
                    attack2_cool--;
                    jump_cool--;
                    isAttack = true;
                }
            }
        }
    }


    //プレイヤーの位置へ通常斬撃
    void Attack1()
    {
        //これから攻撃する箇所に攻撃エフェクトを生成
        Instantiate(slash, playerPosithion, player.transform.rotation);
    }

    //広範囲への連続爆発攻撃
    void Attack2()
    {
        switch (a2)
        {
            case 1:
                
                //画像の向きに応じて攻撃方向を変更する
                if (transform.localScale.x < 0)
                {
                    //連続して爆発を起こす(爆発用のオブジェクトを生成する)
                    Instantiate(bomb, new Vector3(transform.position.x + 1.5f, bomb.transform.position.y, transform.position.z + 0.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 1.5f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 1.5f, bomb.transform.position.y, transform.position.z - 0.5f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 1.5f, bomb.transform.position.y, transform.position.z + 0.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 1.5f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 1.5f, bomb.transform.position.y, transform.position.z - 0.5f), bomb.transform.rotation);
                }
                
                break;

            case 2:
                
                if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 3.0f, bomb.transform.position.y, transform.position.z + 1.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 3.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 3.0f, bomb.transform.position.y, transform.position.z - 1.0f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 3.0f, bomb.transform.position.y, transform.position.z + 1.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 3.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 3.0f, bomb.transform.position.y, transform.position.z - 1.0f), bomb.transform.rotation);
                }

                break;
            case 3:
               if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 6.0f, bomb.transform.position.y, transform.position.z + 1.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 6.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 6.0f, bomb.transform.position.y, transform.position.z - 1.5f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 6.0f, bomb.transform.position.y, transform.position.z + 1.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 6.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 6.0f, bomb.transform.position.y, transform.position.z - 1.5f), bomb.transform.rotation);
                }

                break;

            case 4:
                 if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 9.0f, bomb.transform.position.y, transform.position.z + 2.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 9.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 9.0f, bomb.transform.position.y, transform.position.z - 2.0f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 9.0f, bomb.transform.position.y, transform.position.z + 2.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 9.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 9.0f, bomb.transform.position.y, transform.position.z - 2.0f), bomb.transform.rotation);
                }

                break;

            case 5:
                if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 12.0f, bomb.transform.position.y, transform.position.z + 2.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 12.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 12.0f, bomb.transform.position.y, transform.position.z - 2.5f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 12.0f, bomb.transform.position.y, transform.position.z + 2.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 12.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 12.0f, bomb.transform.position.y, transform.position.z - 2.5f), bomb.transform.rotation);
                }

                break;

            case 6:
                if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 15.0f, bomb.transform.position.y, transform.position.z + 3.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 15.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 15.0f, bomb.transform.position.y, transform.position.z - 3.0f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 15.0f, bomb.transform.position.y, transform.position.z + 3.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 15.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 15.0f, bomb.transform.position.y, transform.position.z - 3.0f), bomb.transform.rotation);
                }

                break;

            case 7:
                 if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 18.0f, bomb.transform.position.y, transform.position.z + 3.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 18.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 18.0f, bomb.transform.position.y, transform.position.z - 3.5f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 18.0f, bomb.transform.position.y, transform.position.z + 3.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 18.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 18.0f, bomb.transform.position.y, transform.position.z - 3.5f), bomb.transform.rotation);
                }

                break;

            case 8:
                 if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 21.0f, bomb.transform.position.y, transform.position.z + 4.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 21.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 21.0f, bomb.transform.position.y, transform.position.z - 4.0f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 21.0f, bomb.transform.position.y, transform.position.z + 4.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 21.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 21.0f, bomb.transform.position.y, transform.position.z - 4.0f), bomb.transform.rotation);
                }

                break;


            case 9:
                if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 24.0f, bomb.transform.position.y, transform.position.z + 4.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 24.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 24.0f, bomb.transform.position.y, transform.position.z - 4.5f), bomb.transform.rotation);
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 24.0f, bomb.transform.position.y, transform.position.z + 4.5f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 24.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 24.0f, bomb.transform.position.y, transform.position.z - 4.5f), bomb.transform.rotation);
                }

                break;

            //最後まで来たらカウンターを0に戻す
            case 10:
                 if (transform.localScale.x < 0)
                {
                    Instantiate(bomb, new Vector3(transform.position.x + 27.0f, bomb.transform.position.y, transform.position.z + 5.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 27.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x + 27.0f, bomb.transform.position.y, transform.position.z - 5.0f), bomb.transform.rotation);
                    a2 = 0;
                }
                else
                {
                    Instantiate(bomb, new Vector3(transform.position.x - 27.0f, bomb.transform.position.y, transform.position.z + 5.0f), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 27.0f, bomb.transform.position.y, transform.position.z), bomb.transform.rotation);
                    Instantiate(bomb, new Vector3(transform.position.x - 27.0f, bomb.transform.position.y, transform.position.z - 5.0f), bomb.transform.rotation);
                    a2 = 0;
                }

                break;
        }
        //カウンターを1増やす
        a2++;
    }



    void GuardStart()
    {
        //ガードを有効にする
        isGuard = true;

        //子オブジェクトのシールドアニメーションを可視化する
        transform.FindChild("BossB_sield").renderer.enabled = true;

    }

    void GuardEnd()
    {
        //ガードを無効にする
        isGuard = false;

        //子オブジェクトのシールドアニメーションを非可視化する
        transform.FindChild("BossB_sield").renderer.enabled = false;


    }

    void JumpStart()
    {
        //本体を非可視化する
        transform.root.gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }

    void JumpEnd()
    {
        //プレイヤーとの位置関係に応じてワープする
        if (playerPosithion.x < transform.position.x)
        {
            //プレイヤーの右側に位置を移動
            transform.position = new Vector3(playerPosithion.x + 5.0f, playerPosithion.y, playerPosithion.z);

            //本体を可視化
            transform.root.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            //カウンターアニメーションを再生(確定ダメージを与える為)
            anime.SetTrigger(counterId);
        }
        else 
        {
            //プレイヤーの左側に位置を移動
            transform.position = new Vector3(playerPosithion.x - 5.0f, playerPosithion.y, playerPosithion.z);

            //本体を可視化
            transform.root.gameObject.GetComponent<SpriteRenderer>().enabled = true;

            //カウンターアニメーションを再生(確定ダメージを与える為)
            anime.SetTrigger(counterId);
        }
    }

    void Counter()
    {
        //プレイヤーに若干のダメージを与える
        int damage = MobBase.calcDamage(minAtk, maxAtk);
        hpapi.Damage(damage);

    }

    //クールタイムの発生
    void CoolOn()
    {
        state = State.Stay;
        coolTime = 3.0f;
        isAttack = false;
        anime.SetBool(stayId, true);
    }


    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {

            //Weponタグの場合はダメージを受ける
            if (other.tag == "Wepon")
            {
                //ガード中の場合はカウンター
                if (isGuard)
                {
                    //ガードを終了してカウンターアニメーションを再生
                    GuardEnd();
                    anime.SetTrigger(counterId);
                    attack2_cool--;
                    jump_cool--;
                    isAttack = true;
                }
                else
                {
                    int damage = new System.Random().Next(player.attack - 5, player.attack + 5);

                //攻撃中のダメージは半減
                if (isAttack)
                {
                    damage -= (int)(damage / 2);
                }
                hp -= damage;
                Debug.Log(damage + "のダメージ   " + "残りHP=   " + hp);
                StartCoroutine(Flush());
                }
        }   
    }
}
