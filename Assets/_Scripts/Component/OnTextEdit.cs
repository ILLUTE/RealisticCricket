using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnTextEdit : MonoBehaviour
{
    public float maxValue, minValue;  // Could be easily pluggable if we use scriptable objects for storing these constant values

    public TMP_InputField m_Text;

    public void OnTextEditEnd()
    {
        if (string.IsNullOrEmpty(m_Text.text))
        {
            m_Text.text = "0";
        }

        float.TryParse(m_Text.text, out float value);

        if (value > maxValue)
        {
            value = maxValue;
        }
        else if (value < minValue)
        {
            value = minValue;
        }
        m_Text.text = value.ToString();
    }
}
