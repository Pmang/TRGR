using UnityEngine;
using System.Collections;

public class BossA_Attack_ef : MonoBehaviour {

    //HPを保持しているクラス
    private HpApi hpapi;

    //自身を召喚するボスクラス
    private Boss_A boss;

    //現在地
    public Vector3 nowPosithion;

    //目的地までの距離
    public Vector3 directhion;

    //現在地と目的地の差
    public float distance;

    //移動速度
    public Vector3 moveSpeed;

    //最低速度
    public float minSpeed = 0.2f;

	public int maxAtk = 270;
	public int minAtk = 160;

    //突進の最大時間
    public float maxTime = 3.0f;

	// Use this for initialization
	void Start () {

        //ゲーム内からオブジェクトを検索し、そこで動作しているスクリプトを取得する
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();
        boss = GameObject.Find("Boss_A").GetComponent<Boss_A>();

        //ボスオブジェクトの子オブジェクトになる
        this.transform.parent = boss.transform;

        //自分の位置を取得
        nowPosithion = boss.transform.position;

        //目的地と現在地の距離と方角の差を求める
        directhion = boss.endPosithion - nowPosithion;

        //移動量を求める
        moveSpeed = directhion * boss.speed;

        //移動量が最低速度以下の場合、最低速度分早くする
        if (moveSpeed.x > 0 && moveSpeed.x < minSpeed)
        {
            moveSpeed.x += minSpeed;
        }
        else if (moveSpeed.x < 0 && moveSpeed.x > -minSpeed)
        {
            moveSpeed.x -= minSpeed;
        }
	
	}

     // Update is called once per frame
    void Update()
    {
        maxTime -= Time.deltaTime;
        
        //3秒経過時に自身を破壊
        if (maxTime < 0)
        {
            this.transform.parent = null;
            Destroy(this.transform.root.gameObject);
            boss.CoolOn();
        }

        //自分の位置を更新
        nowPosithion = boss.transform.position;

       //移動先に応じて画像の向きを変える
        if (boss.transform.localScale.x < 0)
        {
            //画像が反転しているかどうか
            if (transform.localScale.x > 0)
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
        Vector3 p = boss.transform.position;
        p = boss.transform.position = new Vector3(p.x + moveSpeed.x, p.y, p.z + moveSpeed.z);

        //エフェクトが消えるまでは他の攻撃はさせない
        boss.isAttack_now = true;

        //移動先に応じてエフェクトの判定を変える
        if (directhion.x < 0)
        {
            //プレイヤーの座標を超えたら停止
            if (nowPosithion.x < boss.endPosithion.x)
            {
                this.transform.parent = null;
                Destroy(this.transform.root.gameObject);
                boss.CoolOn();
            }
            else
            {
                //何もしない
            }
        }

        else
        {
            //プレイヤーの座標を超えたら停止
            if (nowPosithion.x > boss.endPosithion.x)
            {
                this.transform.parent = null;
                Destroy(this.transform.root.gameObject);
                boss.CoolOn();
            }
            else
            {
                //何もしない
            }
        }
    

    }

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合はダメージを与えて消す
        if (other.tag == "Player")
        {
			int damage = MobBase.calcDamage(minAtk, maxAtk);
            hpapi.Damage(damage);
            this.transform.parent = null;
            Destroy(this.transform.root.gameObject);
            boss.CoolOn();
        }
    }
	

}
