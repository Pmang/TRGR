using UnityEngine;
using System.Collections;
/*
 * ボスの生成する斬撃を管理するスクリプトです。
 * 2015/9/7 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Bomb_ef : MonoBehaviour
{

    //HPを管理するスクリプト
    HpApi hpapi;

    //生成元のボススクリプト
    Boss_B boss;

    // Use this for initialization
    void Start()
    {

        //スクリプトを読み込む
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();
        boss = GameObject.Find("Boss_B").GetComponent<Boss_B>();


    }

    void Update()
    {
        
    }


    void AttackOn()
    {
        //自身のコライダ―(当たり判定)を可視化する
        transform.root.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void AttackOff()
    {
        //自身を削除する
        Destroy(this.transform.root.gameObject);
    }


    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合はダメージを与えて判定を消す
        if (other.tag == "Player")
        {
            int damage = MobBase.calcDamage((int)(boss.minAtk * 1.2),(int)(boss.maxAtk * 1.2));
            hpapi.Damage(damage);
            transform.root.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
