using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoftButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_LoftText;

    private void Start()
    {
        m_LoftText.text = "Grounded";
    }
    public void OnButtonPressed()
    {
        bool loft = GameManager.Instance.PlayLoftedShot();

        m_LoftText.text = loft ? "Lofted" : "Grounded";
    }
}
