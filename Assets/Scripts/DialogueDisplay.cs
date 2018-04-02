﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {
	static DialogueDisplay instance;
	public static DialogueDisplay Instance {
		get { return instance; }
	}

	public Transform manager;
	DialogueManager dm;

	Image background;
	Transform illustObject;
	Image portrait;
	Image nameBox;
	Text nameText;
	Image contentBox;
	Text contentText;
	GameObject statChangeBox;
	Text statChangeText;

	public GameObject SmallButton;
	public List<GameObject> choiceButtons;
	Transform choiceButtonsContainer;

	GameObject dialogueLogScrollView;
	GameObject dialogueLogBox;
	Text dialogueLogContentText;
	Text dialogueLogNameText;

	Text skipButtonText;

	Sprite transparentSprite;

	void Awake () {
		instance = this;

		Dialogue.dd = this;
		DialogueManager.dd = this;

		manager=GameObject.Find ("DialogueManager").GetComponent<Transform>();
		dm = manager.GetComponent<DialogueManager> ();
		
		background=manager.Find ("Background").GetComponent<Image>();
		illustObject=manager.Find ("Illust");
		portrait=manager.Find ("Portrait").GetComponent<Image>();
		nameBox=manager.Find ("NameBox").GetComponent<Image>();
		nameText=manager.Find ("NameBox").Find ("NameText").GetComponent<Text>();
		contentBox=manager.Find ("ContentBox").GetComponent<Image>();
		contentText=manager.Find ("ContentBox").Find ("ContentText").GetComponent<Text>();
		statChangeBox = manager.Find ("StatChangeBox").gameObject;
		statChangeText = statChangeBox.transform.Find ("StatChangeText").GetComponent<Text> ();
		choiceButtonsContainer = manager.Find ("ChoiceButtonsContainer");
		dialogueLogScrollView = manager.Find ("DialogueLogScrollView").gameObject;
		dialogueLogBox = GameObject.Find ("DialogueLogBox").gameObject;
		dialogueLogNameText = dialogueLogBox.transform.Find ("DialogueLogNameText").GetComponent<Text>();
		dialogueLogContentText = dialogueLogBox.transform.Find ("DialogueLogContentText").GetComponent<Text> ();
		dialogueLogScrollView.SetActive (false);
		skipButtonText = manager.Find ("SkipButton").transform.GetComponentInChildren<Text> ();

		transparentSprite = Resources.Load<Sprite> ("UIImages/transparent");

		SCREENSCALE = new Vector2 (Screen.width / 1280f, Screen.height / 720f);

		dialogueLogs = new List<DialogueLog> ();
	}
	void Start(){
		DialogueDisplayClear ();
	}
	public void DialogueDisplayClear(){
		RemovePortraitSprite ();
		RemoveIllustSprite ();
		DisableNameBox ();
		DisableContentBox ();
		DisableStatChangeBox ();
		PutNameText (null);
		PutContentText (null);
		StopQuivering ();
	}
	public void DisableNameBox(){
		nameBox.enabled = false;
	}
	public void EnableNameBox(){
		nameBox.enabled = true;
	}
	public void DisableContentBox(){
		contentBox.enabled = false;
	}
	public void EnableContentBox(){
		contentBox.enabled = true;
	}
	public void DisableStatChangeBox(){
		statChangeBox.SetActive (false);
	}
	public void EnableStatChangeBox(){
		statChangeBox.SetActive (true);
	}
	public void PutNameText(string text){
		nameText.text = text;
	}
	public void PutContentText(string text){
		contentText.text = text;
	}
	public void PutStatChangeText(string text){
		statChangeText.text = text;
		AddDialogueLogWithString("",  "<color=#FFB6C1>" + text + "</color>");
	}
	public void RemoveBackgroundSprite(){
		PutBackgroundSprite (transparentSprite);
	}
	void PutBackgroundSprite(Sprite sprite){
		background.sprite = sprite;
	}
	public void PutBackgroundSprite(string name){
		Sprite sprite=Resources.Load<Sprite>("Backgrounds/"+name);
		PutBackgroundSprite (sprite);
	}
	IEnumerator FadeInBackgroundSpriteIEnumerator(string name){
		isFading = true;
		if (background.sprite != transparentSprite) {
			yield return FadeOut ();
		}
		PutBackgroundSprite (name);
		yield return FadeIn ();
		isFading = false;
	}
	public void FadeInBackgroundSprite(string name){
		StartCoroutine (FadeInBackgroundSpriteIEnumerator (name));
	}
	bool isFading = false;
	public bool IsFading(){
		return isFading;
	}
	public void RemovePortraitSprite(){
		PutPortraitSprite (transparentSprite);
	}
	void PutPortraitSprite(Sprite sprite){
		portrait.sprite = sprite;
	}
	public void PutPortraitSprite(String name){
		Sprite sprite=Resources.Load<Sprite>("Portraits/"+name.Replace("^", ""));
		if (sprite == null) {
			RemovePortraitSprite ();
		} else {
			PutPortraitSprite (sprite);
		}
		if (name.Contains ("^")) {
			BlackenPortraitSprite ();
		} else {
			BrightenPortraitSprite ();
		}
	}
	public void BlackenPortraitSprite(){
		portrait.color = Color.black;
	}
	public void BrightenPortraitSprite(){
		portrait.color = Color.white;
	}
	public void RemoveIllustSprite(){
		PutIllustSprite (transparentSprite);
	}
	void PutIllustSprite(Sprite sprite){
		illustObject.GetComponent<Image>().sprite = sprite;
		illustObject.GetComponent<RectTransform> ().sizeDelta = sprite.rect.size;
	}
	public void PutIllustSprite(String name){
		Sprite sprite=Resources.Load<Sprite>("Illusts/"+name);
		PutIllustSprite(sprite);
	}

	static float QUAKEPOWER = 2.0f;
	public IEnumerator Quake(){
		float remainShakePower = QUAKEPOWER;
		while (remainShakePower > 0) {
			remainShakePower -= 1.0f * Time.deltaTime;
			Vector2 offset = 10 * UnityEngine.Random.insideUnitCircle * remainShakePower * SCREENSCALE.y;
			gameObject.transform.localPosition = new Vector3 (offset.x, offset.y, gameObject.transform.localPosition.z);
			yield return null;
		}
		gameObject.transform.localPosition = new Vector3 (0, 0, gameObject.transform.localPosition.z);
	}
	bool isQuivering = false;
	static float QUIVERPOWER = 0.5f;
	public IEnumerator Quiver(){
		float remainShakePower = QUIVERPOWER;
		isQuivering = true;
		while (isQuivering) {
			Vector2 offset = 10 * UnityEngine.Random.insideUnitCircle * remainShakePower * SCREENSCALE.y;
			gameObject.transform.localPosition = new Vector3 (offset.x, offset.y, gameObject.transform.localPosition.z);
			yield return null;
		}
		gameObject.transform.localPosition = new Vector3 (0, 0, gameObject.transform.localPosition.z);
	}
	public void StopQuivering(){
		isQuivering = false;
	}

	static float FADETIME = 0.2f;
	public IEnumerator FadeIn(){
		float remainFadeTime = FADETIME;
		while (remainFadeTime > 0) {
			remainFadeTime -= 1.0f * Time.deltaTime;
			float value = 1 - remainFadeTime / FADETIME;
			background.color = new Color (value, value, value);
			yield return null;
		}
		background.color = new Color (1, 1, 1);
	}
	public IEnumerator FadeOut(){
		float remainFadeTime = FADETIME;
		while (remainFadeTime > 0) {
			remainFadeTime -= 1.0f * Time.deltaTime;
			float value = remainFadeTime / FADETIME;
			background.color = new Color (value, value, value);
			yield return null;
		}
		background.color = new Color (0, 0, 0);
	}

	public IEnumerator ShowStatChange(){
		float remainStatChangeDisplayTime = 2.0f;
		EnableStatChangeBox ();
		while (remainStatChangeDisplayTime > 0) {
			remainStatChangeDisplayTime -= Time.deltaTime;
			yield return null;
		}
		DisableStatChangeBox ();
	}

	//  Choices showing part starts
	public void CreateChoiceButtons(List<string> choices){
		dm.duringChoice = true;
		dm.ForciblyTurnOffSkip ();

		int x = 0;
		int y = (int)(150f * SCREENSCALE.y);
		int ySpace = (int)(75f * SCREENSCALE.y);
		choiceButtons = new List<GameObject> ();
		for (int i = 0; i < choices.Count; i++) {
			if (choices [i] == "") {
				break;
			}
			GameObject button;
			int index = i;
			button = Util.CreateButton (SmallButton, choiceButtonsContainer, x, y, choices [index], () => {
				dm.duringChoice = false;
				dm.choiceNum = index + 1;
				DestroyChoiceButtons();
				AddDialogueLogWithString("",  "<color=yellow>" + choices[index] + "</color>");
				dm.ToNextLine();
			});
			choiceButtons.Add (button);
			y -= ySpace;
		}
	}
	void DestroyChoiceButtons(){
		for (int i = choiceButtons.Count - 1; i >= 0; i--) {
			GameObject button = choiceButtons [i];
			GameObject.Destroy (button);
		}
		choiceButtons = new List<GameObject> ();
	}
	//  Choices showing part ends

	// Dialogue log part starts
	public class DialogueLog{
		public string speakerName;
		public string content;
		public DialogueLog(string speakerName, string content){
			this.speakerName = speakerName;
			this.content = content;
		}
	}
	public static List<DialogueLog> dialogueLogs = new List<DialogueLog> ();
	public void AddDialogueLog(){
		dialogueLogs.Add (new DialogueLog (nameText.text, contentText.text.Replace("\n"," ")));
	}
	public void AddDialogueLogWithString(string name, string content){
		dialogueLogs.Add (new DialogueLog (name, content));
	}
	void PrintDialogueLog(){
		if (dialogueLogs.Count == 0) {
			return;
		}
		foreach (DialogueLog log in dialogueLogs) {
			dialogueLogNameText.text += "\n" + log.speakerName;
			dialogueLogContentText.text += "\n" + log.content;
		}
		dialogueLogNameText.text = dialogueLogNameText.text.Substring ("\n".Length);
		dialogueLogContentText.text = dialogueLogContentText.text.Substring ("\n".Length);
	}
	public void OpenDialogueLog(){
		dialogueLogNameText.text = null;
		dialogueLogContentText.text = null;
		PrintDialogueLog ();
		RectTransform logBoxRect = dialogueLogBox.GetComponent<RectTransform> ();
		logBoxRect.sizeDelta = new Vector2 (logBoxRect.rect.width, Math.Max (dialogueLogScrollView.GetComponent<RectTransform>().rect.height, dialogueLogNameText.preferredHeight + 30));

		dialogueLogScrollView.SetActive (true);
		dialogueLogScrollView.GetComponentInChildren<Scrollbar> ().value = 0;
	}
	public void CloseDialogueLog(){
		dialogueLogScrollView.SetActive (false);
	}
	public bool IsDialogueLogOpen(){
		return dialogueLogScrollView.activeInHierarchy;
	}
	// Dialogue log part ends

	public void UpdateSkipButtonText(bool duringSkip){
		if (duringSkip) {
			skipButtonText.text = "정지";
		} else {
			skipButtonText.text = "스킵";
		}
	}

	void Update () {
	}

	static Vector2 SCREENSCALE = new Vector2 (1f, 1f);
}
