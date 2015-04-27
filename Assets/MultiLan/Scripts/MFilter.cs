using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Filter {
	public class MFilter {
		
		public static string error;
		
		// Check name format
		public static bool CheckName(string name){
			if(name != null && name != ""){
				Regex checkMask = new Regex("^([a-zA-Z0-9_ ]+)$");
				if(checkMask.IsMatch(name)){
					return true;
				} else {
					error = "filter";
				}
			} else {
				error = "empty";
			}
			return false;
		}
		
		// Check password format
		public static bool CheckPass(string pass){
			if(pass != null && pass != ""){
				Regex checkMask = new Regex("^([a-zA-Z0-9]+)$");
				if(checkMask.IsMatch(pass) && pass.Length >= 4){
					return true;
				} else {
					error = "filter";
				}
			} else {
				error = "empty";
			}
			return false;
		}
		
		// Check e-mail format
		public static bool CheckMail(string mail){
			if(mail != null && mail != ""){
				Regex checkMask = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
				if(checkMask.IsMatch(mail)){
					return true;
				} else {
					error = "filter";
				}
			} else {
				error = "empty";
			}
			return false;
		}
		
		// Check number (check that the value contains only numbers)
		public static bool CheckNumber(string number){
			if(number != null && number != "") {
				try {
					int.Parse(number);						
				} catch(FormatException) {
					error = "filter";	
					return false;
				}	
			} else {
				error = "empty";
			}
			return true;
		}
	}
}
