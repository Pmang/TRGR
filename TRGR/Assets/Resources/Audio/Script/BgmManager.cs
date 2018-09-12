using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * BGMの処理を一括して行うスクリプトです。
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class BgmManager : SingletonMonoBehaviour<BgmManager> {

	private Dictionary<string, AudioClip> audioClipDict;

	private Dictionary<string, AudioClip> seClipDict;

	private ArrayList playingSeList;

	private String playingBgmName;

	private int seCount;

	/// <summary>
	/// 再生中のAudioSource
	/// </summary>
	[NonSerialized]
	public AudioSource currentAudioSource = null;

	/// <summary>
	/// 起動時に実行されます
	/// </summary>
	public void Awake()
	{
		if (this != Instance)
		{
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject);

		// Resource/Audio/BGM階層内のサウンドファイルをすべて読み込む
		this.audioClipDict = new Dictionary<string, AudioClip>();
		foreach (AudioClip bgm in Resources.LoadAll<AudioClip>("Audio/BGM"))
		{
			this.audioClipDict.Add(bgm.name, bgm);
		}

		this.seClipDict = new Dictionary<string, AudioClip>();
		foreach (AudioClip se in Resources.LoadAll<AudioClip>("Audio/SE"))
		{
			this.seClipDict.Add(se.name, se);
		}

		this.playingSeList = new ArrayList();
		this.currentAudioSource = this.gameObject.AddComponent<AudioSource>();

		this.seCount = 0;
	}
	
	public void SePlay(string seName)
	{
		if (!this.seClipDict.ContainsKey(seName))
		{
			Debug.LogError(string.Format("SE[{0}]が見つかりません。", seName));
			return;
		}

		AudioSource seSource = this.gameObject.AddComponent<AudioSource>();
		seSource.clip = this.seClipDict[seName];
		seSource.loop = false;
		seSource.Play();

		this.playingSeList.Add(seSource);
	}


	public void Update()
	{
		if (seCount >= playingSeList.Count)
			seCount = 0;

		if (playingSeList.Count > 0 && !((AudioSource)playingSeList[seCount]).isPlaying)
		{
			DestroyObject((AudioSource)playingSeList[seCount]);
			playingSeList.RemoveAt(seCount);
		}

		seCount++;
	}


	/// <summary>
	/// BGMを再生します
	/// </summary>
	/// <param name="bgmName">BGM名</param>
	public void Play(string bgmName, bool isLoop)
	{
		if (!this.audioClipDict.ContainsKey(bgmName))
		{
			Debug.LogError(string.Format("BGM[{0}]が見つかりません。", bgmName));
			return;
		}

		/*if ((this.currentAudioSource != null) && (this.currentAudioSource.clip == this.audioClipDict[bgmName]))
		{
			// すでに指定されたBGMを再生中
			return;
		}*/

		if (this.currentAudioSource != null && this.currentAudioSource.isPlaying)
		{
			// 再生されているBGMを停止させる
			this.currentAudioSource.Stop();
		}

		this.playingBgmName = bgmName;
		this.currentAudioSource.clip = this.audioClipDict[bgmName];
		this.currentAudioSource.loop = isLoop;
		this.currentAudioSource.Play();
	}

	/// <summary>
	/// 現在再生しているBGMを停止します
	/// </summary>
	public void Stop()
	{
		this.currentAudioSource.Stop();
	}

	public bool IsPlaying()
	{
		return this.currentAudioSource.isPlaying;
	}

	public String GetPlayingBgmName()
	{
		return this.playingBgmName;
	}
}
