using UnityEngine;
using UnityEditor; 
using System.Collections;

[CustomEditor(typeof(MMenu))]
public class MMenuEditor : Editor {	
	public override void OnInspectorGUI () {}
	
	/*private void AddOption(){
		if(edit.persPlayersOptions.Length > 0){
			int[] options = new int[edit.persPlayersOptions.Length+1];
			int j = 0;
			for(int i = 0; i < edit.persPlayersOptions.Length; i++){
				options[j] = edit.persPlayersOptions[i];
				j++;			
			}
			options[j] = 0;
			edit.persPlayersOptions = options;
		} else {
			edit.persPlayersOptions = new int[1];			
		}
	}
	
	private void DeleteOption(){
		if(edit.persPlayersOptions.Length > 0){
			if(edit.persPlayersOptions.Length > 1){
				int[] options = new int[edit.persPlayersOptions.Length-1];
				int j = 0;
				for(int i = 0; i < edit.persPlayersOptions.Length-1; i++){
					options[j] = edit.persPlayersOptions[i];
					j++;			
				}
				edit.persPlayersOptions = options;
			} else {
				edit.persPlayersOptions = new int[0];
			}
		}
	}//OnInspectorGUI */
}//MMenuEditor
