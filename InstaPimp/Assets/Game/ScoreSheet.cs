using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSheet : MonoBehaviour {

    public Text Kills;
    public Image PlayerColor;

	public void SetKills(Color playerColor, int kills)
    {
        Kills.text = kills.ToString();
        PlayerColor.color = playerColor;
    }
}
