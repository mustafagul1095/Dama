using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckBlackWins : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    private void Update()
    {
        if (GetComponentsInChildren<Piece>().Length <= 0)
        {
            gameOverText.SetText("Black Wins !");
            gameOverText.enabled = true;
        }
    }
}
