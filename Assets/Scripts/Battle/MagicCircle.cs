using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCircle : MonoBehaviour {
	public Side side;

	string engName;
	string korName;
	string spriteName;
	Image image;

	public void SetMagicCircle(Side side, string korName, string spriteName){
		this.side = side;
		this.korName = korName;
		this.spriteName = spriteName;
		image = GetComponentInChildren<Image>();
		image.sprite = Resources.Load<Sprite>("Battle/MagicCircle/"+spriteName);
	}

	public void GetActivated(){
		Util.GetEnemySideUnit(side).GetDamageOrHeal(-Util.GetSideUnit(side).GetStat(Stat.Power));
		GetDestroyed();
	}
	public void GetDestroyed(){
		StartCoroutine(FadeOut(1f));
	}
	public IEnumerator FadeOut(float FADETIME){
		float remainFadeTime = FADETIME;
		while (remainFadeTime > 0) {
			remainFadeTime -= 1.0f * Time.deltaTime;
			float value = remainFadeTime / FADETIME;
			image.color = new Color (1f, 1f, 1f, value);
			yield return null;
		}
		image.color = new Color (1f, 1f, 1f, 0f);
		Destroy(gameObject);
	}
}
