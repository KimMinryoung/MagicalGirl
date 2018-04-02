using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSquareManager : MonoBehaviour {

	Dictionary<int, Square> grid;
	public class Square{
		int row;
		int column;
		Vector2 pos;
		int index;
		GameObject gameObject;
		MagicCircle magicCircle;
		public Square(int row, int column){
			this.row = row;
			this.column = column;
			pos = new Vector2((column - 2) * 120, (2 - row) * 120);
			index = (row - 1) * 3 + column;
			gameObject = GameObject.Find("Cell" + index.ToString());
			magicCircle = null;
		}
		public void EnableClick(bool enable){
			gameObject.GetComponent<Button> ().interactable = enable;
		}
		public bool IsEmpty(){
			return magicCircle == null;
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

	// Use this for initialization
	void Start () {
		EnableEmptySqauresClick ();
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
		DisableAllSquaresClick ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
