using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCircle : MonoBehaviour {
	string engName;
	string korName;
	string spriteName;
	Image image;

	public void SetMagicCircle(string korName, string spriteName){
		this.korName = korName;
		this.spriteName = spriteName;
		image = GetComponentInChildren<Image>();
		image.sprite = Resources.Load<Sprite>("Battle/MagicCircle/"+spriteName);
	}
}
