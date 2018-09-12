using UnityEngine;
using System.Collections;
/*
 * ステージ3のボスが生成するパーティクルを管理するスクリプトです
 * 2015/9/14 専門学校ビーマックス 2年 藤田 勇気
 */

public class TimeReaper_Boss_C : MonoBehaviour {

    //プレイヤークラス
    Player player;
    
    //タイマーに与えるダメージ量
    public int damage = 20;

	// Use this for initialization
	void Start () {

        //プレイヤーオブジェクトを取得
        player = GameObject.Find("Player").GetComponent<Player>();
	
	}
	
	// Update is called once per frame
	void Update () {

        //自身がアタッチしているパーティクルが放出しているかを確認
        if (!transform.root.particleSystem.isPlaying)
        {
            //プレイヤーが無敵エリアに入っていない場合はタイマー側にダメージを与える
            if (player.isDamage)
            {
                GameObject.Find("Canvas").GetComponent<TimerApi>().Damage(damage);
            }

            //自身のアタッチするオブジェクトを削除
            Destroy(this.transform.root.gameObject);
        }
	
	}
}
