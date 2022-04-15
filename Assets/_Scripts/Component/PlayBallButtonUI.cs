using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBallButtonUI : MonoBehaviour
{
   public void PlayBall()
    {
        GameManager.Instance.StartBowling();
    }
}
