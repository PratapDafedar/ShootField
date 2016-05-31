using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

public class Login : MonoBehaviour 
{
    public InputField usernameField;

	public Text invalidNameText;
    
    public Toggle teamRed;
    public Toggle teamBlue;

    void Start()
    {
        string savedName = PlayerPrefs.GetString("PLAYER_NAME");
        if (savedName != null && savedName != "")
        {
            usernameField.text = savedName;
        }

        int isBlue = PlayerPrefs.GetInt("TEAM_IS_BLUE");
        teamBlue.isOn = (isBlue == 1) ? true : false;
        teamRed.isOn = (isBlue == 0) ? true : false;
    }
	
    public void OnEnterButtonPressed()
    {
		if (usernameField.text != null &&
		          usernameField.text != "" &&
		          IsValidUsername (usernameField.text)) {
			string name = usernameField.text;
			Player.Team team = teamRed.isOn ? Player.Team.Red : Player.Team.Blue;

			Player player = new Player (name, team);

			GameManager.Instance.cPlayer = player;
			GameManager.Instance.LoadFindRoomScreen ();

			PlayerPrefs.SetString ("PLAYER_NAME", player.name);
			PlayerPrefs.SetInt ("TEAM_IS_BLUE", teamBlue.isOn ? 1 : 0);
		} else {
			invalidNameText.gameObject.SetActive (true);
		}
    }

    //////user name validation methods.//////
    public static bool IsValidUsername(string username)
    {
        if (username == null)
        {
            return false;
        }

        var length = username.Length;
        if (length < 3 || length > 32)
        {
            return false;
        }

        if (!IsLowerAlpha(username[0]))
        {
            return false;
        }

        if (!IsLowerAlphanumeric(username[length - 1]))
        {
            return false;
        }

        if (!Regex.IsMatch(username, "^[a-z0-9._-]*$"))
        {
            return false;
        }

        if (Regex.IsMatch(username, "[0-9]{5,}"))
        {
            return false;
        }

        // Each username can contain only one of '.', '_', '-'.
        var punctuation = new[] { '.', '_', '-' };
        if (punctuation.Count(c => username.Contains(c)) > 1)
        {
            return false;
        }

        // Each '.', '_', and '-' should be followed by an alpha-numeric.
        for (var i = 0; i < length - 1; i++)
        {
            if (punctuation.Contains(username[i]) && !IsLowerAlphanumeric(username[i + 1]))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsLowerAlpha(char c)
    {
        return c >= 'a' && c <= 'z';
    }

    private static bool IsLowerAlphanumeric(char c)
    {
        return IsLowerAlpha(c) || (c >= '0' && c <= '9');
    }
}
