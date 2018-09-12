using UnityEngine;
using System.Collections;

/*
 * プレイヤーの攻撃時発生する斬撃の処理を行うスクリプトです。
 * 2015/6/19 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Player_ef2 : MonoBehaviour {

    //アタッチしているゲームオブジェクト
    GameObject wepon;

    //自身を生成するプレイヤーのクラス
    GameObject player;

    //一度生成されると同じ方向へ飛んで行くように
    bool first;

    //武器の飛んで行く速度
    public float weponSpeed = 15f;

    //飛んで行く時間
    public float rimitTime = 2.0f;

    //経過時間を取得
    public float nowTime = 0.0f;

	// Use this for initialization
	void Start () {

        //プレイヤークラスを取得
        player = GameObject.FindWithTag("Player");

        //現在アタッチされているオブジェクトを読み込む
        wepon = transform.root.gameObject;

        //初回のみtrue
        first = true;
	
	}
	
	// Update is called once per frame
	void Update () {

        //時間を進め、rimitTimeに達していなければ移動し、達した場合は消える
        nowTime += Time.deltaTime;

        if (nowTime > rimitTime)
        {
            Destroy(wepon);
        }

        //一度のみ速度を与える事で消えるまで一定の方向へ進んでいく(プレイヤーが向きを変えても残撃の方向が変わらない)
        if (first)
        {
            //プレイヤーの座標に応じて方向を変えて武器が飛んで行く
            if (player.transform.rotation.y == 0)
            {
                wepon.GetComponent<Rigidbody>().velocity = new Vector3(weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
            }
            else
            {
                wepon.GetComponent<Rigidbody>().velocity = new Vector3(-weponSpeed, wepon.transform.position.y, wepon.transform.position.z);
            }

            first = false;
        }
        else
        {
            //何もしない
        }
        
	
	}

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合はダメージを与えて消す
        if (other.tag == "Enemy")
        {         
            Destroy(wepon);
        }
    }
}
