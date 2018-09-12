using UnityEngine;
using System.Collections;

/*
 * 画面切り替え時のフェードイン、アウトの処理を行うスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class FadeManager : SingletonMonoBehaviour<FadeManager> {

	private float fadeAlpha = 0;

	// ステージ遷移用のフェードフラグ
	private bool isFading = false;

	// 時間操作用のフェードフラグ
	private bool isMasking = false;

	// 時間操作用のフェードカラー
	private Color maskColor;

	// ステージ遷移用のフェードカラー
	public Color fadeColor = Color.black;

	public Color pauseColor = Color.black;

	// ポーズ状態
	private bool isPause;

	// 起動時に一回実行
	public void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void Pause()
	{
		isPause = true;
	}

	public void Resume()
	{
		isPause = false;
	}

	// Javaのpaintみたいなもの
	public void OnGUI()
	{
		if (isPause)
		{
			this.pauseColor.a = 0.5f;
			GUI.color = this.pauseColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
		else
		{
			if (!this.isFading)
				return;

			if (isMasking)
			{
				this.maskColor.a = this.fadeAlpha;
				GUI.color = this.maskColor;
			}
			else
			{
				this.fadeColor.a = this.fadeAlpha;
				GUI.color = this.fadeColor;
			}

			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}
	}

	public void LoadLevel(string scene, float interval)
	{
		this.isMasking = false;

		StartCoroutine(TransScene(scene, interval));
	}

	private IEnumerator TransScene(string scene, float interval)
	{
		this.isFading = true;
		float time = 0;
		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		Application.LoadLevel(scene);

		time = 0;
		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
			time += Time.fixedDeltaTime;
			yield return 0;
		}

		this.isFading = false;
	}

	public void Mask(Color color)
	{
		this.maskColor = color;
		this.isFading = true;
		this.isMasking = true;

		StartCoroutine(Darken());
	}

	public void UnMask()
	{
		StartCoroutine(Brighten());
	}

	private IEnumerator Darken()
	{
		float interval = 0.3f;
		float time = 0;

		if (isFading || isMasking)
			yield return 0;

		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp(0f, 0.05f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}
	}

	private IEnumerator Brighten()
	{
		float interval = 0.3f;
		float time = 0;

		if (!isFading || !isMasking)
			yield return 0;

		while (time <= interval)
		{
			this.fadeAlpha = Mathf.Lerp(0.05f, 0f, time / interval);
			time += Time.deltaTime;
			yield return 0;
		}

		this.isMasking = false;
		this.isFading = false;
	}
}
