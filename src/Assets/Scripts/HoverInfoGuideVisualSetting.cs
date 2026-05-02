using UnityEngine;
using TMPro;

public class HoverInfoGuideVisualSetting : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public static bool enabledOrNot = true;
    void Start()
    {
        if (!enabledOrNot)
        {
            buttonText.text = "<color=#e60000>Disabled";
            enabledOrNot = false;
        }
        else
        {
            buttonText.text = "<color=#26e300>Enabled";
            enabledOrNot = true;
        }
    }
    public void updateHoverInfoSettingText()
    {
        if (enabledOrNot)
        {
            buttonText.text = "<color=#e60000>Disabled";
            enabledOrNot = false;
        }
        else
        {
            buttonText.text = "<color=#26e300>Enabled";
            enabledOrNot = true;
        }
    }
}
