using UnityEngine;
using System.Collections;

/*
 * モブの攻撃範囲を管理するスクリプトです。
 * 2015/6/2 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class AttackArea_S : MonoBehaviour
{

    //親オブジェクトを保持
    Mob_S mob;


    //最初はここ
    void Start()
    {
        //このスクリプトがアタッチしている先にあるMobクラスを呼び出す
        mob = transform.root.gameObject.GetComponent<Mob_S>();

    }



    //他のコライダがアタッチしているコライダに接触している場合、そのコライダを受け取る
    private void OnTriggerStay(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_S.State.Attack;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_S.State.Moveing;
        }
    }
}
