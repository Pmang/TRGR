using UnityEngine;
using System.Collections;
/*
 * プレイヤーの攻撃範囲を可視化する為のターゲットマーカーを管理するスクリプトです。
 * 2015/9/15 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Mark_Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //エネミータグの場合は対象の保持するマーカーを可視化する
        if (other.tag == "Enemy")
        {
            //子オブジェクトのシールドアニメーションを可視化する
            other.gameObject.transform.FindChild("mark").renderer.enabled = true;
        }
    }

    //範囲内から何かが出ると動く
    private void OnTriggerExit(Collider other)
    {
        //エネミータグの場合は対象の保持するマーカーを非可視化する
        if (other.tag == "Enemy")
        {
            //子オブジェクトのシールドアニメーションを可視化する
            other.gameObject.transform.FindChild("mark").renderer.enabled = false;
        }
    }
	

}
