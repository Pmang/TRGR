using UnityEngine;
using System.Collections;
/*
 * モブの攻撃範囲の管理を行うスクリプトです。
 * 2015/8/27 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class AttackArea_L : MonoBehaviour
{

    //親オブジェクトを保持
    Mob_L mob;


    //最初はここ
    void Start()
    {
        //このスクリプトがアタッチしている先にあるMobクラスを呼び出す
        mob = transform.root.gameObject.GetComponent<Mob_L>();

    }



    //自身のコライダー内に他のコライダーが進入した場合の処理
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は対応した攻撃用のフラグを立てる
        if (other.tag == "Player")
        {
            mob.attack = true;
            mob.state = Mob_L.State.Attack;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //フラグを戻す
        if (other.tag == "Player")
        {
            mob.attack = false;
        }
    }
}
