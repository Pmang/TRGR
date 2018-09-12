using UnityEngine;
using System.Collections;
/*
 * モブの共通処理を一括して行うスクリプトです。
 * 2015/9/15  ITスペシャリスト学科 情報処理コース    2年 藤田 勇気
 */
public class SafeArea : MonoBehaviour {

    //プレイヤークラス
    Player player;

    //タイマーApi
    private TimerApi time;

	// Use this for initialization
	void Start () {

        //プレイヤーを読み込む
        player = player = GameObject.FindWithTag("Player").GetComponent<Player>();

        //タイマーApiを読み込む
        time = GameObject.Find("Canvas").GetComponent<TimerApi>();	
	}
	
	// Update is called once per frame
	void Update () {

        //タイマーが動き出すと保護を解除し、自身を破壊する
        if (time.Mode != TimerMode.STOP)
        {
            player.isDamage = true;
            Destroy(this.transform.root.gameObject);
        }
	
	}

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの場合はプレイヤーを保護する
        if (other.tag == "Player")
        {
            player.isDamage = false;
        }
    }

    //範囲内に何かが入ると動く
    private void OnTriggerExit(Collider other)
    {
        //プレイヤーの場合はプレイヤーを保護する
        if (other.tag == "Player")
        {
            player.isDamage = true;
        }
    }

}
