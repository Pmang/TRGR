using UnityEngine;
using System.Collections;
/*
 * モブの基本的な動作を行うスクリプトです。
 * 2015/8/27 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */
public class Mob_L : MobBase
{

    //ステータス
    public int hp = 85;
    public float speed = 0.005f;
    public float gravity = 0;
    public int maxAtk = 60;
    public int minAtk = 40;

    //callまでの残り時間
    public float call = 8.0f;

    //callのタイムカウントを動かすかどうかのフラグ
    public bool isCall;

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


    //現在のプレイヤークラス
    private Player m;

    private Animator anime;

    private HpApi hpapi;

    //mobのstate
    public enum State
    {
        Stay,
        Move,
        Attack,
        Return
    }

    public State state;

    //攻撃用のフラグ
    public bool attack;

    //攻撃用のトリガー
    private int attack_Id;

    //移動用のフラグ
    private int move_Id;

    //召喚用のトリガー
    private int call_Id;

    // Use this for initialization
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

        //オブジェクトに引っ付いてるファイルを読み込む
        anime = transform.root.gameObject.GetComponent<Animator>();
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();

        //ヒールオブジェクトを取得
        heal = (GameObject)Resources.Load("Prefabs/heal_item");

        //初期stateはstay
        state = State.Stay;

        //フラグを初期化
        attack = false;

        isCall = false;

        //トリガー準備
        attack_Id = Animator.StringToHash("attack");

        move_Id = Animator.StringToHash("move");

        call_Id = Animator.StringToHash("call");

    }

    // Update is called once per frame
    public override void Update()
    {

        // MobBaseクラスのUpdate呼び出し
        base.Update();

        if (hp < 0)
        {
            // MobBaseクラスのDepop呼び出し
            base.Depop();
        }

        //一度でもプレイヤーを検知していればタイマーのカウントを減らす
        if (isCall)
        {
            call -= Time.deltaTime;

            //タイマーが0以下の場合はそのまま下記のswitchでStayへ移行させる
            if (call <= 0)
            {
                state = State.Stay;
            }
            
        }

        switch (state)
        {

            case State.Stay:
                
                //フラグをチェック
                if (isCall)
                {
                    //callタイマーが0以下かどうか
                    if (call <= 0)
                    {
                        //アニメーションを再生、それ以降のタイマーはカウントしない
                        anime.SetBool(move_Id, false);
                        anime.SetTrigger(call_Id);
                        isCall = false;
                    }
                }
                break;


            case State.Move:
                Moveing();
                break;

            case State.Attack:
                anime.SetBool(move_Id, false);
                Attacking();
                break;

            case State.Return:
                Returning();
                break;
        }

    }


    void Moveing()      //プレイヤーに忍び寄る
    {

        //移動のアニメーションを開始
        anime.SetBool(move_Id, true);

        //自分の位置を取得
        nowPosithion = this.transform.position;

        //プレイヤーの位置を取得して目的地とする
        endPosithion = m.position;

        //目的地と現在地の距離と方角の差を求める
        directhion = endPosithion - nowPosithion;

        //移動量を求める
        moveSpeed = directhion * speed;

        //移動先に応じて画像の向きを変える
        if (directhion.x > 0)
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

        //描画する位置を調整する(つまり移動する)
        Vector3 p = this.transform.position;
        p = this.transform.position = new Vector3(p.x + moveSpeed.x, p.y, p.z + moveSpeed.z);
    }

    void Returning()    //元の位置に戻る
    {

        //移動のアニメーションを開始
        anime.SetBool(move_Id, true);

        //自分の位置を取得
        nowPosithion = this.transform.position;

        //目的地と現在地の距離と方角の差を求める
        directhion = startPosithon - nowPosithion;

        //移動量を求める
        moveSpeed = directhion * speed;

        //移動先に応じて画像の向きを変える
        if (directhion.x > 0)
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
            //そのまま
        }

        //描画する位置を調整する(つまり移動する)
        Vector3 p = this.transform.position;
        p = this.transform.position = new Vector3(p.x + moveSpeed.x, p.y, p.z + moveSpeed.z);
    }

    void Attacking()  //攻撃
    {
        //各フラグに応じた攻撃アニメーションを再生する
        if (attack)
        {
            anime.SetTrigger(attack_Id);
        }
        else
        {
            //どちらのフラグも立っていない場合は移動に戻る
            state = State.Move;
        }

    }

    //アニメーションイベントで使用する
    void Attack1()
    {
        int damage = MobBase.calcDamage(minAtk, maxAtk);
        hpapi.Damage(damage);
    }

    void Call()
    {
        //自身を基点に銃剣兵を2体召喚
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Pop(Enemy.G, 2,this.transform.position);
    }


    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの攻撃の場合はダメージを受ける
        if (other.tag == "Wepon")
        {
            int damage = new System.Random().Next(m.attack - 5, m.attack + 5);
            hp -= damage;
            StartCoroutine(Flush());
        }
    }
}
