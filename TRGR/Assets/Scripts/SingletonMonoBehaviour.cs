using UnityEngine;
using System.Collections;

/*
 * 
 * 2015/6/  ITスペシャリスト学科 情報処理コース    2年 大福 祐輔
 */

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if (instance == null)
				{
					Debug.LogError(typeof(T) + " is nothing.");
				}
			}

			return instance;
		}
	}

}
