using UnityEngine;
using System.Collections;
/*
 * ボスの遠距離攻撃を管理するスクリプトです。
 * 2015/6/12 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Boss_A_Wepon : MonoBehaviour
{

    //アタッチしているゲームオブジェクト
    GameObject wepon;

    //現在のプレイヤークラス
    Player m;

    //生成元のボスクラス
    Boss_A b;

    //一度生成されると同じ方向へ飛んで行くように
    bool first;

    //武器の飛んで行く速度
    public float weponSpeed = 2f;

    //飛んで行く時間
    public float rimitTime = 3f;

    //経過時間を取得
    public float nowTime = 0.0f;

    //HPを保持するクラス
    private HpApi hpapi;

	public int maxAtk = 180;
	public int minAtk = 120;

    // Use this for initialization
    void Start()
    {
        //現在アタッチされているオブジェクトを読み込む
        wepon = transform.root.gameObject;

        //プレイヤーの現在位置を取得したいのでフィールドを取得
        m = GameObject.Find("Player").GetComponent<Player>();

        //プレイヤーと同じく座標を取得する為
        b = GameObject.Find("Boss_A").GetComponent<Boss_A>();

        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();

        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        //時間を進め、rimitTimeに達していなければ移動し、達した場合は消える
        nowTime += Time.deltaTime;

        if (nowTime > rimitTime)
        {
            Destroy(wepon);
            b.CoolOn();
        }

        //初回以降速度を変更させない
        if (first)
        {
            //プレイヤーの座標に応じて方向を変えて武器が飛んで行く
            if (m.transform.position.x - b.transform.position.x < 0)
            {
                wepon.GetComponent<Rigidbody>().velocity = new Vector3(-weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
            }
            else
            {
                //反転させる
                wepon.transform.localScale = new Vector3(wepon.transform.localScale.x * -1, wepon.transform.localScale.y, wepon.transform.localScale.z - 1);
                wepon.GetComponent<Rigidbody>().velocity = new Vector3(weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
            }
            first = false;
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
            Destroy(wepon);
                b.CoolOn();
        }
    }

}
