using UnityEngine;
using System.Collections;
/*
 * ボスの飛斬撃の処理を行うスクリプトです。
 * 2015/9/15 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */
public class BossC_ef2 : MonoBehaviour {

    //アタッチしているゲームオブジェクト
    GameObject wepon;

    //HPを管理するスクリプト
    HpApi hpapi;

    //生成元のボススクリプト
    Boss_C boss;


    //一度生成されると同じ方向へ飛んで行くように
    bool first;

    //武器の飛んで行く速度
    public float weponSpeed = 15f;

    //飛んで行く時間
    public float rimitTime = 2.0f;

    //経過時間を取得
    public float nowTime = 0.0f;

    // Use this for initialization
    void Start()
    {

        //スクリプトを読み込む
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();
        boss = GameObject.Find("Boss_C").GetComponent<Boss_C>();

        //現在アタッチされているオブジェクトを読み込む
        wepon = transform.root.gameObject;

        //向きの修正
        if (boss.transform.position.x - boss.playerPosithion.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        //向きに応じて速度を与える
        if (boss.transform.localScale.x < 0)
        {
            wepon.GetComponent<Rigidbody>().velocity = new Vector3(weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
        }
        else
        {
            wepon.GetComponent<Rigidbody>().velocity = new Vector3(-weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
        }




    }

    // Update is called once per frame
    void Update()
    {

        //時間を進め、rimitTimeに達していなければ移動し、達した場合は消える
        nowTime += Time.deltaTime;

        if (nowTime > rimitTime)
        {
            Destroy(wepon);
        }


    }

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合はダメージを与えて判定を消す
        if (other.tag == "Player")
        {
            int damage = MobBase.calcDamage((int)(boss.minAtk * 0.5), (int)(boss.maxAtk * 0.5));
            hpapi.Damage(damage);
            transform.root.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
