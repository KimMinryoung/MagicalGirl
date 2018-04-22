using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
	MagicSquareManager MSM;
	
	void Start(){
		MSM = GetComponentInChildren<MagicSquareManager>();
		MSM.EnableEmptySqauresClick ();
	}
}
