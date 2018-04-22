using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
	string engName;
	string korName;
	string explanation;
	GameObject nameText;
	GameObject explanationText;

	public void SetText(string korName, string explanation){
		this.korName = korName;
		this.explanation = explanation;
		nameText.GetComponent<Text>().text = korName;
		explanationText.GetComponent<Text>().text = explanation;
	}
}
