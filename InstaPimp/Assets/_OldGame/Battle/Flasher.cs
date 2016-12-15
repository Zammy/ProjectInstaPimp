using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Flasher : MonoBehaviour
{
    public Image Image;

    public void Flash()
    {
        var color = Image.color;
        color.a = .65f;
        Image.color = color;

        Image.DOFade(0f, 0.5f);
    }
}
	
