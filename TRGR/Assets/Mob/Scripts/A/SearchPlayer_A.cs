using UnityEngine;
using System.Collections;

public class SearchPlayer_A : MonoBehaviour {
    
    //Mobクラス
    Mob_A mob;

	// Use this for initialization
	void Start () {

        //自身の親オブジェクトにアタッチしているスクリプトを読み込む
        mob = this.transform.root.gameObject.GetComponent<Mob_A>();
	
	}

    //自身のコライダー内に他のコライダーが進入した場合の処理
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_A.State.Move;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_A.State.Return;
        }
    }
}
