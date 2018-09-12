using UnityEngine;
using System.Collections;
/*
 * モブの移動作度を上げる範囲の管理を行うスクリプトです。
 * 2015/8/27 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class DashArea_A : MonoBehaviour {

    //親オブジェクトを保持
    Mob_A mob;


    //最初はここ
    void Start()
    {
        //このスクリプトがアタッチしている先にあるMobクラスを呼び出す
        mob = transform.root.gameObject.GetComponent<Mob_A>();

    }



    //自身のコライダー内に他のコライダーが進入した場合の処理
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は対応したフラグを立て、判定を消す
        if (other.tag == "Player")
        {
            mob.state = Mob_A.State.Move;
            mob.dash = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }


}
