using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{

    public Text ScoreText;
    public Image Background;

    public int Score
    {
        get
        {
            return int.Parse(ScoreText.text);
        }
        set
        {
            ScoreText.text = value.ToString();
        }
    }

    public Color Color
    {
        set
        {
            this.Background.color = value;
        }
    }

}

