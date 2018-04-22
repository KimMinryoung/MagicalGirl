using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSquareManager : MonoBehaviour {
	public GameObject magicCirclePrefab;

	Dictionary<int, Square> grid;
	public class Square{
		int row;
		int column;
		Vector2 pos;
		int index;
		public GameObject gameObject;
		MagicCircle magicCircleObject;
		public Square(int row, int column){
			this.row = row;
			this.column = column;
			pos = new Vector2((column - 2) * 120, (2 - row) * 120);
			index = (row - 1) * 3 + column;
			gameObject = GameObject.Find("Cell" + index.ToString());
			magicCircleObject = null;
		}
		public void EnableClick(bool enable){
			gameObject.GetComponent<Button> ().interactable = enable;
		}
		public void SetMagicCircle(MagicCircle magicCircleObject){
			this.magicCircleObject = magicCircleObject;
		}
		public bool IsEmpty(){
			return magicCircleObject == null;
		}
	}

	void Awake(){
		grid = new Dictionary<int, Square> ();
		for (int row = 1; row <= 3; row++) {
			for (int column = 1; column <= 3; column++) {
				int index = (row - 1) * 3 + column;
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
		GameObject magicCircleObject = Instantiate(magicCirclePrefab) as GameObject;
		magicCircleObject.transform.SetParent(grid[index].gameObject.transform, false);
		MagicCircle magicCircle = magicCircleObject.GetComponent<MagicCircle>();
		grid[index].SetMagicCircle(magicCircle);
		magicCircle.SetMagicCircle("불꽃 씨앗", "Fire");
		EnableEmptySqauresClick();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
