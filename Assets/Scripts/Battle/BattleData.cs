using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine;

public static class BattleData {
	public static Unit player;
	public static Unit opponent;
	public static BattleManager BM;
	public static int turnNumber;
}

public class Unit{
	string engName;
	string korName;
	static int DEFAULTMAXHEALTH = 50;
	int maxHealth;
	int currentHealth;
	Dictionary<Stat, int> stats = new Dictionary<Stat, int> ();
	public Text healthText;
	
	public int GetStat(Stat stat){
		return stats[stat];
	}

	public Unit(string engName, string korName, int power = 5, int defense = 5, int agile = 5){
		this.engName = engName;
		this.korName = korName;
		maxHealth = DEFAULTMAXHEALTH;
		currentHealth = maxHealth;
		stats.Add (Stat.Power, power);
		stats.Add (Stat.Defense, defense);
		stats.Add (Stat.Agile, agile);
	}
	public void GetDamageOrHeal(int originalHealthChange){
		int finalHealthChange;
		if (originalHealthChange >= 0) {
			finalHealthChange = originalHealthChange;
		} else {
			finalHealthChange = -Math.Max (1, -originalHealthChange - stats [Stat.Defense]);
		}
		currentHealth = Math.Min (maxHealth, currentHealth + finalHealthChange);
		UpdateHealthDisplay();
	}
	public void UpdateHealthDisplay(){
		healthText.text = currentHealth.ToString();
	}
}