using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopCounter : MonoBehaviour
{
    [NotNull]
    public GameManager manager;

    public TextMeshProUGUI countdownText;
    public void Update()
    {
        countdownText.text = $"{manager.copsKilled}/{manager.copsToKill}";
    }
}
