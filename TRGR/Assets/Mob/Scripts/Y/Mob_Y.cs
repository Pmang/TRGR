using UnityEngine;
using System.Collections;
/*
 * 特殊なモブの動作を行うスクリプトです。
 * 2015/9/15 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Mob_Y : MobBase {

    //時空の歪みのオブジェクト
    GameObject yano;

    //タイマーApi
    private TimerApi time;

	// Use this for initialization
    public override void Start()
    {
        base.Start();

        //タイマーApiを読み込む
        time = GameObject.Find("Canvas").GetComponent<TimerApi>();

	    //オブジェクトを読み込む
        yano = (GameObject)Resources.Load("Prefabs/safearea");
	}
	
	// Update is called once per frame
    public override void Update(){

        base.Update();

        //タイマーが停止していなければ何もせず消える
        if (time.Mode != TimerMode.STOP)
        {
            base.Depop();
        }
	
	}

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの攻撃の場合はダメージを受ける
        if (other.tag == "Wepon")
        {
            //消える前に時空の歪みを召喚する
            Instantiate(yano, new Vector3(transform.position.x, yano.transform.position.y, transform.position.z), yano.transform.rotation);

            base.Depop();
        }
    }

}
