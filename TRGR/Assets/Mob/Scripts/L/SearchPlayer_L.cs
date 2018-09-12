using UnityEngine;
using System.Collections;

public class SearchPlayer_L : MonoBehaviour
{

    //Mobクラス
    Mob_L mob;

    //1回目の検知かどうかを判断するフラグ
    private bool first;

    // Use this for initialization
    void Start()
    {

        //自身の親オブジェクトにアタッチしているスクリプトを読み込む
        mob = this.transform.root.gameObject.GetComponent<Mob_L>();

        //フラグを立てておく
        first = true;

    }

    //自身のコライダー内に他のコライダーが進入した場合の処理
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            //初回に限り親オブジェクトのフラグを立てる
            if (first)
            {
                //対象を内部のみでfalseにし、二度とフラグが立たないようにするs
                mob.isCall = true;
                first = false;
            }

            mob.state = Mob_L.State.Move;
        }
    }

    //コライダから外れた場合
    private void OnTriggerExit(Collider other)
    {
        //プレイヤータグの場合は移動を開始する
        if (other.tag == "Player")
        {
            mob.state = Mob_L.State.Return;
        }
    }
}
