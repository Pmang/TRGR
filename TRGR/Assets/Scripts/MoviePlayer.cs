using UnityEngine;
using System.Collections;

/*
 *  ムービーをコントロールするクラスです
 */
public class MoviePlayer : MonoBehaviour {

    // 
    private MovieTexture texture;

    private bool onceFlag;

    // 遷移先シーン
    public string nextScene;

	// Use this for initialization
	void Start () {

        this.onceFlag = false;

        // オブジェクトからムービーテクスチャーを取得
        this.texture = ((MovieTexture)gameObject.GetComponent<GUITexture>().texture);

        // 再生する
        this.texture.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.onceFlag)
        {
            if (Input.GetButtonDown("Submit"))
            {
                this.texture.Stop();
                this.onceFlag = true;
                FadeManager.Instance.LoadLevel(this.nextScene, 0.9f);
            }

            if (!this.texture.isPlaying)
            {
                this.onceFlag = true;
                FadeManager.Instance.LoadLevel(this.nextScene, 0.9f);
            }
        }
	}
}
