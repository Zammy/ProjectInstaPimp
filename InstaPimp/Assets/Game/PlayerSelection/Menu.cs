using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public MenuOption[] MenuOptions;

    private int currentOption = 0;
    public int CurrentOption
    {
        get
        {
            return currentOption;
        }
        set
        {
            value = value % MenuOptions.Length;
            if (value < 0)
                value = MenuOptions.Length - 1;

            MenuOptions[currentOption].Deactivate();
            currentOption = value;
            MenuOptions[currentOption].Activate();
        }
    }

    void Start()
    {
        this.CurrentOption = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var device = InputManager.ActiveDevice;
        if (device.DPadUp.WasPressed)
        {
            this.CurrentOption--;
        }
        if (device.DPadDown.WasPressed)
        {
            this.CurrentOption++;
        }

        if (device.Action1.WasPressed)
        {
            GameInfo.GameMode = (GameMode)this.CurrentOption;

            SceneManager.LoadScene("Test");
        }
    }
}
