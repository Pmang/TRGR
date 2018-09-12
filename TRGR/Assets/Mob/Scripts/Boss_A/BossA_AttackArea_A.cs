using UnityEngine;
using System.Collections;

/*
 * ボスの攻撃範囲の管理を行うスクリプトです。
 * 2015/6/11 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class BossA_AttackArea_A : MonoBehaviour {

    //アタッチしている先のスクリプトを保持
    Boss_A boss;


	// Use this for initialization
	void Start () {
        //ボスのスクリプトを取得
        boss = transform.root.GetComponent<Boss_A>();
	
	}

    //他のコライダがアタッチしているコライダに接触している場合、そのコライダを受け取る
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は攻撃Aのフラグを立てる
        if (other.tag == "Player")
        {
            boss.isAttack_A = true;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //プレイヤータグの場合はフラグを戻す
        if (other.tag == "Player")
        {
            boss.isAttack_A = false;
        }
    }
}
