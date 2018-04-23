using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour {
	MagicSquareManager MSM;
	
	void Awake(){
		BattleData.BM = this;
	}
	void Start(){
		BattleData.turnNumber = 1;
		MSM = GetComponentInChildren<MagicSquareManager>();
		MSM.EnableEmptySqauresClick ();
	}

	public void OnDecideSetPosition(int index){
		int AIposIndex = AIDecide();
		if(index != AIposIndex){
			MSM.InstallMagicCircle(Side.Player, index, "Fire");
		}
		MSM.InstallMagicCircle(Side.Opponent, AIposIndex, "Metal");
		ThreeMatch();
		BattleData.turnNumber ++;
	}
	public int AIDecide(){
		if(BattleData.turnNumber == 1){
			return 1;
		}else if(MSM.grid[5].IsEmpty()){
			return 5;
		}else if(MSM.grid[1].IsEmpty()){
			return 1;
		}else if(MSM.grid[3].IsEmpty()){
			return 3;
		}else if(MSM.grid[7].IsEmpty()){
			return 7;
		}else if(MSM.grid[9].IsEmpty()){
			return 9;
		}else if(MSM.grid[2].IsEmpty()){
			return 2;
		}else if(MSM.grid[4].IsEmpty()){
			return 4;
		}else if(MSM.grid[6].IsEmpty()){
			return 6;
		}else{
			return 8;
		}
	}
	public void ThreeMatch(){
		Dictionary<int, Square> grid = MSM.grid;
		List<int> matchedRows = new List<int>();
		for (int row = 1; row <= 3; row++) {
			List<int> indexes = Util.GetLineIndexes(LineType.Row, row);
			if(grid[indexes[0]].GetOwnerSide() != Side.None && indexes.GroupBy(i_ => grid[i_].GetOwnerSide()).Count() == 1){
				matchedRows.Add(row);
			}
		}
		List<int> matchedColumns = new List<int>();
		for (int column = 1; column <= 3; column++) {
			List<int> indexes = Util.GetLineIndexes(LineType.Column, column);
			if(grid[indexes[0]].GetOwnerSide() != Side.None && indexes.GroupBy(i_ => grid[i_].GetOwnerSide()).Count() == 1){
				matchedColumns.Add(column);
			}
		}
		List<int> matchedDiagonals = new List<int>();
		for (int diag = 1; diag <= 2; diag++) {
			List<int> indexes = Util.GetLineIndexes(LineType.Diagonal, diag);
			if(grid[indexes[0]].GetOwnerSide() != Side.None && indexes.GroupBy(i_ => grid[i_].GetOwnerSide()).Count() == 1){
				matchedDiagonals.Add(diag);
			}
		}
		foreach(int row in matchedRows){
			MSM.DestroyLine(LineType.Row, row);
		}
		foreach(int column in matchedColumns){
			MSM.DestroyLine(LineType.Column, column);
		}
		foreach(int diag in matchedDiagonals){
			MSM.DestroyLine(LineType.Diagonal, diag);
		}
	}
}
