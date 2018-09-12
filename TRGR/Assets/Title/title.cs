using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class title : MonoBehaviour {

	private bool onceFlag;

	// Use this for initialization
	void Start ()
	{
		BgmManager.Instance.Play("Title", true);
		this.onceFlag = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!this.onceFlag)
		{
			if (Input.GetButtonDown("Cancel"))
			{
				this.onceFlag = true;
				BgmManager.Instance.SePlay("Return");
				Application.Quit();
			}

			if (Input.GetButtonDown("Submit"))
			{
				this.onceFlag = true;
				BgmManager.Instance.SePlay("Decide");
				FadeManager.Instance.LoadLevel("selecttutrial", 0.9f);
			}
		}
	}
}
