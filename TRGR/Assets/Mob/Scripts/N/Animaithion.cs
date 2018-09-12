using UnityEngine;
using System.Collections;

/*
 * モブのアニメーションコントローラーの管理を行うスクリプトです。
 * 2015/5/23 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Animaithion : MonoBehaviour {

    //各クラスをキャッシュする変数
    Animator a;
    Mob_N m;

    //前回のステートを保存しておく
    public AnimatorStateInfo nowState;

    //アニメーションステートを変更する用
    public int moveId;
    public int attackId;
    public int woodId;

	//最初はココが動く
    void Start()
    {
        a = transform.root.gameObject.GetComponent<Animator>();
        m = transform.root.gameObject.GetComponent<Mob_N>();

        moveId = Animator.StringToHash("move");
        attackId = Animator.StringToHash("attack");

        nowState = this.a.GetCurrentAnimatorStateInfo(0);
    }
	
	//毎フレーム呼び出される
	void Update () {
        
        //現在のステートに応じたアニメーションステートをtrueに、他をfalseへ
        switch (m.state)
        {
            case Mob_N.State.Moveing:

                a.SetBool(attackId, false);
                a.SetBool(moveId, true);
                break;

            case Mob_N.State.Attack:

                a.SetBool(moveId, false);
                a.SetBool(attackId, true);
                break;

            case Mob_N.State.Normal:

                a.SetBool(attackId, false);
                a.SetBool(moveId, false);
                break;

            case Mob_N.State.Returning:

                a.SetBool(attackId, false);
                a.SetBool(moveId, true);
                break;

        }


	
	}
}
