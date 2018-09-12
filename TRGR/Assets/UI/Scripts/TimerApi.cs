using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * ゲーム画面上のタイマーの処理を管理するスクリプトです。
 * 2015/5/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */


public class TimerApi : MonoBehaviour
{

	private Slider slider;

	private float power;

	private float heal;

	private TimerMode _mode;

	private Image timerImage;

	public const int MAX = 60;

	public Sprite normal;

	public Sprite fast;

	public Sprite slow;

	private int Timelimit;

    //ヤーノをpopさせるインターバル
    public float yano;

    //プレイヤークラス
    private Player player;

	private StageManager sm;

	private string bgmName;

	private float timedelay;

	private Transform pin;

	// Use this for initialization
	void Start()
	{
        //プレイヤークラスのフラグを変更する必要がある為取得
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
		this.slider = GameObject.Find("Timer").GetComponent<Slider>();
		this.sm = GameObject.Find("StageManager").GetComponent<StageManager>();
		
		//this.slider.maxValue = MAX;
		this.Timelimit = MAX;
		this.pin = this.slider.gameObject.transform.FindChild("Fill Area");
		this.timerImage = this.slider.gameObject.transform.FindChild("Fill Area").transform.FindChild("Fill").GetComponent<Image>();
		this.Mode = TimerMode.NORMAL;

		// ワールドマネージャーからそのステージの再生すべきBGMを取得する
		bgmName = WorldManager.Instance.playBgm;

		BgmManager.Instance.Play(bgmName, true);

		StartCoroutine(Count());
	}

	// Update is called once per frame
	void Update()
	{
		if (this.Timelimit > MAX)
		{
			this.Timelimit = MAX;
			this.pin.transform.eulerAngles = new Vector3(0, 0, 0);
		}

		if (!this.sm.isFreeze)
		{
			//カウントを増やす
			yano += Time.deltaTime;

			//タイマーの状態がスロウかどうか
			if (Mode == TimerMode.SLOW)
			{
				//一定時間が経過しているかどうか
				if (yano > 10f)
				{
					GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Pop(Enemy.H, 3, player.position);
					yano = 0;
				}
				else
				{
					//何もしない
				}
			}
			else
			{
				//何もしない
			}

			
            //STOP中はモードの切り替えを行わせない
            if (Mode == TimerMode.STOP)
            {

            }
            else
            {
                if (Input.GetButtonDown("UP"))
                {
                    if (Mode == TimerMode.FAST)
                    {
                        //プレイヤーの遠距離攻撃を止める
                        player.isUp = false;
                        Mode = TimerMode.NORMAL;
                        BgmManager.Instance.Play(bgmName, true);
                        FadeManager.Instance.UnMask();
                    }
                    else
                    {
                        //プレイヤーの遠距離攻撃を開始
                        player.isUp = true;
                        Mode = TimerMode.FAST;
                        BgmManager.Instance.Play("BattleUp", true);
                        FadeManager.Instance.Mask(new Color(128, 0, 0));
                    }
                }
                if (Input.GetButtonDown("DOWN"))
                {
                    if (Mode == TimerMode.SLOW)
                    {
                        //プレイヤーの遠距離攻撃を止める
                        player.isUp = false;
                        Mode = TimerMode.NORMAL;
                        BgmManager.Instance.Play(bgmName, true);
                        FadeManager.Instance.UnMask();
                    }
                    else
                    {
                        //プレイヤーの遠距離攻撃を止める
                        player.isUp = false;
                        Mode = TimerMode.SLOW;
                        BgmManager.Instance.Play("BattleDown", true);
                        FadeManager.Instance.Mask(new Color(0, 0, 128));
                    }
                }
            }
           
		}
	}

	IEnumerator Count()
	{
		while (this.Timelimit > 0)
		{
			yield return new WaitForSeconds(this.power);

			if (!this.sm.isFreeze)
			{
                //STOP中はタイマーは動作しない
                if (Mode == TimerMode.STOP)
                {
                }
                else
                {
                    this.Timelimit--;

					/****************************************/
					/* タイムアラート用追記場所				*/
					/****************************************/
					if (this.Timelimit < 15)
					{
						Debug.Log("Time = " + (Time.time - timedelay));
						if (Time.time - timedelay > 2.5f)
						{
							BgmManager.Instance.SePlay("Warning");
							this.timedelay = Time.time;
						}
						Debug.Log("Aleart!");
					}
					/****************************************/
					/* ここまで								*/
					/****************************************/

					Debug.Log((MAX - this.Timelimit) * 6 / 360f);
                    //Quaternion q = new Quaternion(this.pin.transform.rotation.x, this.pin.transform.rotation.y,  (360 -(MAX - this.Timelimit) * 6)/360f, this.pin.transform.rotation.w);
                    //this.pin.transform.Rotate(Vector3.forward, -360 / MAX);
                    this.pin.transform.eulerAngles = new Vector3(0, 0, this.Timelimit * (360 / MAX));
                    Debug.Log(this.pin.transform.rotation);
                }

				
			}
		}

		if (!WorldManager.Instance.isGameOver)
		{
			WorldManager.Instance.isGameOver = true;
			FadeManager.Instance.LoadLevel("gameover", 0.9f);
		}
	}

	public void Heal(float value)
	{
		this.heal = this.Timelimit + value;

		if (this.heal > MAX)
		{
			this.heal = MAX;
		}

		StartCoroutine(IncreaseTimer());
	}

    //指定した時間分タイマーを減少させる
    public void Damage(int damage)
    {
        this.Timelimit -= damage;
        this.pin.transform.eulerAngles = new Vector3(0, 0, this.Timelimit * (360 / MAX));

    }


	public TimerMode Mode
	{
		set
		{
			this._mode = value;

			switch (value)
			{
				case TimerMode.NORMAL:
					this.power = 1.0f;
					this.timerImage.sprite = this.normal;
					break;

				case TimerMode.FAST:
					this.power = 0.5f;
					this.timerImage.sprite = this.fast;
					break;

				case TimerMode.SLOW:
					this.power = 2.0f;
					this.timerImage.sprite = this.slow;
					GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Pop(Enemy.H, 3, player.position);
					break;

			}
		}

		get
		{
			return this._mode;
		}
	}

	private IEnumerator IncreaseTimer()
	{
		while (this.Timelimit < this.heal)
		{
			yield return new WaitForSeconds(0f);
			if (this.Timelimit <= MAX)
			{
				this.Timelimit++;
				this.pin.transform.eulerAngles = new Vector3(0, 0, this.Timelimit * (360 / MAX));
			}
			else
			{
				break;
			}
			Debug.Log(this.Timelimit);
		}
	}
}

public enum TimerMode
{
	NORMAL,
	FAST,
	SLOW,
    STOP,
}