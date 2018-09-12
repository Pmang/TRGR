using UnityEngine;
using System.Collections;
/*
 * ステージ3のボスを管理するスクリプトです
 * 2015/9/13 専門学校ビーマックス 2年 藤田 勇気
 */

public class Boss_C : MobBase {

    //ステータス
    public int hp = 500;
    public int fraghp;
    public float gravity = 0;

    public int maxAtk = 120;
    public int minAtk = 80;

    //クールタイム
    public float coolTime;

    private int attack2_cool = 2;

    private int fire_cool = 4;

    public float time_cool = 20f;

    private float warp_cool = 1.0f;

    //TimeReaper用のカウンター
    public int phase;

    //ランダム値
    System.Random r;

    //現在のプレイヤークラス
    Player player;

    //プレイヤーの座標
    public Vector3 playerPosithion;

    //攻撃モーション中かどうか
    private bool isAttack;

    //各攻撃の判定フラグ
    public bool isTime;

    //アニメーションステートに渡す値を保持
    private int stayId;
    private int attack1Id;
    private int attack2Id;
    private int fireId;
    private int timeId;

    //攻撃時に生成するエフェクト
    private GameObject slash1;
    private GameObject slash2;
    private GameObject fire;
    private GameObject time;


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

        //ヒールオブジェクトを取得
        heal = (GameObject)Resources.Load("Prefabs/heal_item");

        //HPの最大値を取得する
        fraghp = hp / 2;

        //最初は攻撃を行わない状態
        state = State.Stay;

        isAttack = false;

        isTime = false;

        //各トリガーへ渡すハッシュを取得
        anime = transform.root.gameObject.GetComponent<Animator>();
        stayId = Animator.StringToHash("stay");
        attack1Id = Animator.StringToHash("attack1");
        attack2Id = Animator.StringToHash("attack2");
        fireId = Animator.StringToHash("fire");
        timeId = Animator.StringToHash("time");

        //生成するゲームオブジェクトをプレハブから読み込む
        slash1 = (GameObject)Resources.Load("Prefabs/BossC_ef1");
        slash2 = (GameObject)Resources.Load("Prefabs/BossC_ef2");
        fire = (GameObject)Resources.Load("Prefabs/BossC_fire");
        time = (GameObject)Resources.Load("Prefabs/TimeReaper");

        //TimeReaper用のカウンターを初期化
        phase = 1;

        //ランダムのシード値を設定
        r = new System.Random((int)Time.time);


        //ゲーム開始時のクールタイムは少し長めに
        coolTime = 4.0f;
	
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

        //timeのクールを減少させる
        time_cool -= Time.deltaTime;

        //timeのクールが0になった時点で次回の攻撃を確定する



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

        //HPが50％を切った時点でフラグが立つ
        if (hp < fraghp)
        {
            isTime = true;
        }

        //stateに応じた分岐
        switch (state)
        {
            //Stayの間はクールタイムの扱いとして動かない
            case State.Stay:
                
                //各クールタイムを減少させる
                warp_cool -= Time.deltaTime;
                coolTime -= Time.deltaTime;

                //ワープクールが0以下になると一定の確率でワープする
                if (warp_cool <= 0)
                {
                    //先にシード値を更新する
                    r  = new System.Random(r.Next());
                    if (r.Next(1, 100) > 90)
                    {
                        warp_cool = 1.0f;
                        Warp();
                    }
                }

                //クールタイムが0以下になった時点で攻撃に戻る
                if (coolTime <= 0)
                {
                    anime.SetBool(stayId, false);
                    state = State.Attack;
                }

                break;

            case State.Attack:

                //時間攻撃用のフラグが立っているか
                if (isTime)
                {
                    //timeのクールを最優先で確認
                    if (time_cool <= 0)
                    {
                        //時間を刈り取りに入る
                        anime.SetTrigger(timeId);                        
                        attack2_cool--;
                        fire_cool--;
                        isAttack = true;
                        break;
                    }
                    else
                    {
                        Attacking();
                    }
                }
                else
                {
                    Attacking();                    
                }
                break;

        }


    }

    //各攻撃の判定をここで行う　通常斬撃：attack1　飛斬撃：attack2　ランダムファイア：fire　TimeReaper(時間を刈り取るやつ)：time
    void Attacking()
    {

        //攻撃モーション中で無ければいずれかのアニメーションを再生
        if (!isAttack)
        {
            //時間攻撃用のフラグが立っているか
            if (isTime)
            {
                //timeのクールを最優先で確認
                if (time_cool <= 0)
                {
                    //時間を刈り取りに入る
                    anime.SetTrigger(timeId);
                    time_cool = 20f;
                    attack2_cool--;
                    fire_cool--;
                    isAttack = true;
                }
            }

            //次点でfireを確認、その後通常攻撃の判定へ            
            if (fire_cool <= 0)
            {
                //fireのアニメーションを再生
                anime.SetTrigger(fireId);
                fire_cool = 4;
                attack2_cool--;
                isAttack = true;
            }
            else if (attack2_cool <= 0)
            {
                //attack2のアニメーションを再生
                anime.SetTrigger(attack2Id);
                attack2_cool = 2;
                fire_cool--;
                isAttack = true;
            }
            else
            {
                //attack1のアニメーションを再生
                anime.SetTrigger(attack1Id);
                attack2_cool--;
                fire_cool--;
                isAttack = true;
            }



        }
    }


    //**以下アニメーションイベント用**//
    
    //通常斬撃
    void Attack1()
    {
        //これから攻撃する箇所に攻撃エフェクトを生成
        Instantiate(slash1, player.position,slash1.transform.rotation);
    }


    //飛斬撃
    void Attack2()
    {
        //飛ぶ攻撃エフェクトを生成
        Instantiate(slash2, this.transform.FindChild("PopManager").transform.position, slash2.transform.rotation);
    }


    //フィールド内にランダムに爆発を発生させる
    void Fire()
    {
        for (int i = 0; i < 5; i++)
        {
            //シード値を更新後、エフェクトを生成                          
            r = new System.Random(r.Next());
            Instantiate(fire, new Vector3(r.Next(20, 70), fire.transform.position.y, r.Next(3, 17)), transform.rotation);
        }

    }


    //時間を刈り取る全体攻撃
    void TimeReaper()
    {
        //アニメーションイベントの呼び出す回数に応じてフェイズを移行して行く
        switch (phase)
        {
            case 1:
                //タイマーの時間減少を止める
                GameObject.Find("Canvas").GetComponent<TimerApi>().Mode = TimerMode.STOP;
                break;
            case 2:
                //自身をフィールドの中央に移動する
                this.transform.position = new Vector3(47f, transform.position.y, 10f);
                break;
            case 3:
                //自身の座標を基点にこのボスのみが召喚する特殊なモブ(白ヤーノ)を召喚する
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Pop(Enemy.Y, 4, transform.position);
                break;
            case 4:
                //固定の位置にパーティクルを生成する
                Instantiate(time, time.transform.position, time.transform.rotation);
                break;
            case 5:
                //フェイズをリセットし、タイマーを元に戻す
                GameObject.Find("Canvas").GetComponent<TimerApi>().Mode = TimerMode.NORMAL;
                time_cool = 20f;
                phase = 0;
                break;
        }
        //フェイズを1つ進める
        phase++;
    }

    //フィールド内の特定の範囲内にワープする
    public void Warp()
    {
        //自身の座標をランダム値で移動後、向いている方向を修正
        r = new System.Random(r.Next());
        Vector3 p = transform.position = new Vector3(r.Next(30, 60), transform.position.y, r.Next(3, 17));

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

    //プレイヤー付近へワープする
    public void WarpPlayer()
    {
        //プレイヤーとの位置関係に応じてワープする
        if (playerPosithion.x < transform.position.x)
        {
            //プレイヤーの右側に位置を移動
            transform.position = new Vector3(playerPosithion.x + 6.0f, playerPosithion.y, playerPosithion.z);
        }
        else
        {
            //プレイヤーの左側に位置を移動
            transform.position = new Vector3(playerPosithion.x - 6.0f, playerPosithion.y, playerPosithion.z);
        }

        //ワープ後に向きを修正

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


    //クールタイムの発生
    void CoolOn()
    {
        state = State.Stay;
        coolTime = 2.0f;
        isAttack = false;
        anime.SetBool(stayId, true);
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
