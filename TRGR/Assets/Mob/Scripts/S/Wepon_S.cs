using UnityEngine;
using System.Collections;
/*
 * モブの遠距離攻撃を管理するスクリプトです。
 * 2015/6/4 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Wepon_S : MonoBehaviour
{

    //アタッチしているゲームオブジェクト
    GameObject wepon;
    
    //武器の飛んで行く速度
    public float weponSpeed = 10000f;

    //飛んで行く時間
    public float rimitTime = 6.0f;

    //経過時間を取得
    public float nowTime = 0.0f;

	public int maxAtk = 140;
	public int minAtk = 110;

    //HPを保持するクラス
    private HpApi hpapi;


    // Use this for initialization
    void Start()
    {
        //現在アタッチされているオブジェクトを読み込む
        wepon = transform.root.gameObject;
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();
    }

    // Update is called once per frame
    void Update()
    {
        //時間を進め、rimitTimeに達していなければ移動し、達した場合は消える
        nowTime += Time.deltaTime;

        if(nowTime > rimitTime)
        {
            Destroy(wepon);
        }


        //武器が飛んで行く
        wepon.GetComponent<Rigidbody>().velocity = new Vector3(-weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
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
        }
    }

}
