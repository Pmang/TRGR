using UnityEngine;
using System.Collections;

/*
 * プレイヤーの攻撃時発生する斬撃の処理を行うスクリプトです。
 * 2015/6/19 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Player_ef1 : MonoBehaviour {


	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

	
	}

    //一定まで攻撃アニメーションが行われた際に攻撃判定を有効化する
    void AttackOn()
    {
        transform.root.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    //攻撃アニメーションの終わりと共に自身を消す
    void AttackOff()
    {
        Destroy(this.transform.root.gameObject);
    }



    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //エネミータグの場合は判定を消す
        if (other.tag == "Enemy")
        {
            transform.root.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
