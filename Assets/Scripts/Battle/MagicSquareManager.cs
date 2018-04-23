using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MagicSquareManager : MonoBehaviour {
	public GameObject magicCirclePrefab;

	public Dictionary<int, Square> grid;

	void Awake(){
		grid = new Dictionary<int, Square> ();
		for (int row = 1; row <= 3; row++) {
			for (int column = 1; column <= 3; column++) {
				int index = Util.GetSquareIndex(row, column);
				grid [index] = new Square (row, column);
			}
		}
	}

	public void DisableAllSquaresClick(){
		foreach (var kv in grid) {
			kv.Value.EnableClick (false);
		}
	}
	public void EnableEmptySqauresClick(){
		foreach (var kv in grid) {
			kv.Value.EnableClick (kv.Value.IsEmpty ());
		}
	}
	public void OnClickSquare(int index){
		BattleData.BM.OnDecideSetPosition(index);

		EnableEmptySqauresClick();
	}

	public void InstallMagicCircle(Side side, int index, string spriteName){
		GameObject magicCircleObject = Instantiate(magicCirclePrefab) as GameObject;
		magicCircleObject.transform.SetParent(grid[index].gameObject.transform, false);
		MagicCircle magicCircle = magicCircleObject.GetComponent<MagicCircle>();
		grid[index].SetMagicCircle(magicCircle);
		magicCircle.SetMagicCircle(side, spriteName, spriteName);
	}

	public void ActivateLine(LineType type, int num){
		List<int> indexes = Util.GetLineIndexes(type, num);
		indexes.All(index => {grid[index].ActivateMagicCircle(); return true;});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

	public class Square{
		int row;
		int column;
		Vector2 pos;
		int index;
		public GameObject gameObject;
		MagicCircle magicCircle;
		public Square(int row, int column){
			this.row = row;
			this.column = column;
			pos = new Vector2((column - 2) * 120, (2 - row) * 120);
			index = Util.GetSquareIndex(row, column);
			gameObject = GameObject.Find("Cell" + index.ToString());
			magicCircle = null;
		}
		public void EnableClick(bool enable){
			gameObject.GetComponent<Button> ().interactable = enable;
		}
		public void SetMagicCircle(MagicCircle magicCircle){
			this.magicCircle = magicCircle;
		}
		public void ActivateMagicCircle(){
			if(!IsEmpty()){
				magicCircle.GetActivated();
				magicCircle = null;
			}
		}
		public bool IsEmpty(){
			return magicCircle == null;
		}
		public Side GetOwnerSide(){
			if(IsEmpty()){
				return Side.None;
			}else{
				return magicCircle.side;
			}
		}
	}