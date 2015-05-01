using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Login : MonoBehaviour {

    public InputField usernameField;
    
    public Toggle teamRed;
    public Toggle teamBlue;

	
    public void OnEnterButtonPressed()
    {
        User player = new User ();

        player.name     = usernameField.text;
        player.cTeam    = teamRed.isOn ? User.Team.RED : User.Team.BLUE;
        player.id       = new System.Random ().Next (1000, 100000);
        
        GameManager.Instance.cPlayer = player;

        GameManager.LoadLobbyScreen();
    }
}
