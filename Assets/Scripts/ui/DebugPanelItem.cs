using TMPro;
using UnityEngine;

namespace ui
{
    public class DebugPanelItem : MonoBehaviour
    {
        public TextMeshProUGUI titleText;
        public TMP_InputField input;

        public void Set(string title, string value)
        {
            titleText.text = title;
            input.text = value;
        }

        public string Get()
        {
            return input.text;
        }
    }
}