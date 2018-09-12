using UnityEngine;
using System.Collections;

/*
 * 敵を倒すとドロップする回復アイテムの処理を行うスクリプトです。
 * 2015/6/24 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class HealItem : MonoBehaviour {

    //プレイヤーのHPを管理するクラス
    private HpApi hpapi;

    //回復量
    public int heal = 100;

	// Use this for initialization
	void Start () {

        //オブジェクトに引っ付いてるファイルを読み込む
        hpapi = GameObject.Find("Canvas").GetComponent<HpApi>();
	
	}

    //範囲内に何かが入ると動く
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合はHPを回復し消す
        if (other.tag == "Player")
        {
			BgmManager.Instance.SePlay("GetAid");
            hpapi.Hp += heal;
            Destroy(this.transform.root.gameObject);
        }
    }
}
