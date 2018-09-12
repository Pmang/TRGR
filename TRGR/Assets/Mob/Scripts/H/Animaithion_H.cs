using UnityEngine;
using System.Collections;

/*
 * モブのアニメーションコントローラーの管理を行うスクリプトです。
 * 2015/6/8 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気
 */

public class Animaithion_H : MonoBehaviour {

    //各クラスをキャッシュする変数
    Animator a;
    Mob_H m;

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
        m = transform.root.gameObject.GetComponent<Mob_H>();

        moveId = Animator.StringToHash("move");
        attackId = Animator.StringToHash("attack");

        nowState = this.a.GetCurrentAnimatorStateInfo(0);
    }
	
	//毎フレーム呼び出される
	void Update () {
        
        //現在のステートに応じたアニメーションステートをtrueに、他をfalseへ
        switch (m.state)
        {
            case Mob_H.State.Moveing:

                a.SetBool(attackId, false);
                a.SetBool(moveId, true);
                break;

            case Mob_H.State.Attack:

                a.SetBool(moveId, false);
                a.SetBool(attackId, true);
                break;

            case Mob_H.State.Normal:

                a.SetBool(attackId, false);
                a.SetBool(moveId, false);
                break;

            case Mob_H.State.Returning:

                a.SetBool(attackId, false);
                a.SetBool(moveId, true);
                break;

        }


	
	}
}
