using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    public Text Text;

    public void DisplayTimeLeft(float time)
    {
        float milisecs = time - (float)(int)time;
        Text.text = string.Format("{0}:{1:D2}", (int)time, (int)(milisecs * 100)); ;
    }
}
