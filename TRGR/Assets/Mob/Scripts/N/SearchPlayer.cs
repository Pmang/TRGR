using UnityEngine;
using System.Collections;

/*
 * モブのプレイヤーを索敵する範囲を管理するスクリプトです。
 * 2015/5/23 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class SearchPlayer : MonoBehaviour {

    //Mobクラス
    Mob_N mob;



	//最初はここ
	void Start () {
        //このスクリプトがアタッチしている先にあるMobクラスを呼び出す
        mob = transform.root.gameObject.GetComponent<Mob_N>();

        

	
	}
	
	//他のコライダがアタッチしているコライダに接触している場合、そのコライダを受け取る
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_N.State.Moveing;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_N.State.Returning;
        }
    }
}
