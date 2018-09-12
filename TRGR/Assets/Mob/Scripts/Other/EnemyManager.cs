using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * モブのpop処理を一括して行うスクリプトです。
 * 2015/6/11 ITスペシャリスト学科 ゲームクリエイターコース 2年 藤田 勇気(基本の動作を作成、基本コメントのある所)
 *           ITスペシャリスト学科 情報処理コース           2年 大福 祐輔(全てのモブを一括管理できる汎用クラスへ改修、基本コメントのない所)
 */

public class EnemyManager : MonoBehaviour {

    //各プレハブを保持
    GameObject mob_N;

    GameObject mob_S;

    GameObject mob_H;

    GameObject mob_G;

    GameObject mob_A;

    GameObject mob_L;

    GameObject mob_Y;

	//ランダムクラス
	System.Random r;

	Queue<Enemy> queue;

	//フィールドにpopさせられる最大数
	public int maxPopCount;

    //召喚する座標の基準
    private Vector3 position;

	public bool pop;

	// フレーム毎にポップさせる用
	private int counter;

	// 残りポップ可能数
	private int addable;

	public void IncresePop(int num)
	{
		this.addable -= num;
	}


    //召喚を要請したオブジェクトの座標、召喚するオブジェクトの種類、召喚する数を受け取る
	public void Pop(Enemy enemy, int num, Vector3 position)
	{
		this.counter = num;

        this.position = position;
		
		for (int i = 0; i < num; i++)
		{
			this.queue.Enqueue(enemy);
		}
	}

	public void Depop(MobBase mob)
	{
		Destroy(mob.transform.root.gameObject);
		this.addable++;

		if (this.addable == this.maxPopCount)
		{
			GameObject.Find("StageManager").GetComponent<StageManager>().Clear();
		}
	}

	// Use this for initialization
	void Start ()
	{
		//各オブジェクトを読み込む
        mob_N = (GameObject)Resources.Load("Prefabs/Enemy_N");
        mob_S = (GameObject)Resources.Load("Prefabs/Enemy_S");
        mob_H = (GameObject)Resources.Load("Prefabs/Enemy_H");
        mob_G = (GameObject)Resources.Load("Prefabs/Enemy_G");
        mob_A = (GameObject)Resources.Load("Prefabs/Enemy_A");
        mob_L = (GameObject)Resources.Load("Prefabs/Enemy_L");
        mob_Y = (GameObject)Resources.Load("Prefabs/Enemy_Y");

		// 初期で出現している分だけぽっぷさせられるカウンタをさげておく
		this.addable = this.maxPopCount - GameObject.FindGameObjectsWithTag("Enemy").Length;

		Debug.Log(string.Format("addable pop count is {0}.", this.addable));


		//シード値を設定
		// s = 0;
		r = new System.Random((int)Time.time);

		//いきなり沸くと困るのでfalse
		//pop = false;
		counter = 0;
		this.queue = new Queue<Enemy>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//ポップさせるかの判定を都度行う
		if (counter > 0)
		{
			//フィールド上に沸かせた数が一定値を超えたらpop処理を行わない
			if (addable < 0)
			{
				counter--;
				this.queue.Dequeue();
				return;
			}

			counter--;
			addable--;

			//基点からある程度ずらしてモブを沸かせる
			switch (this.queue.Dequeue())
			{
				case Enemy.H:
					Instantiate(mob_H, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
					break;
				case Enemy.N:
					Instantiate(mob_N, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
					break;
				case Enemy.S:
					Instantiate(mob_S, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
					break;
                case Enemy.G:
                    Instantiate(mob_G, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
                    break;
                case Enemy.A:
                    Instantiate(mob_A, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
                    break;
                case Enemy.L:
                    Instantiate(mob_L, new Vector3(position.x + r.Next(3, 12), 2.3f, position.z + r.Next(-4, 4)), transform.rotation);
                    break;
                case Enemy.Y:
                    Instantiate(mob_Y, new Vector3(position.x + r.Next(-10, 10), 2.3f, position.z + r.Next(-1, 1)), transform.rotation);
                    break;
			}

			//シード値を変更(沸く位置がずれないので)
			r = new System.Random((int)Time.time + counter);
		}
		else
		{

		}

	
	}
}

// Mobの種類
public enum Enemy
{
	S,H,N,G,L,A,Y
}