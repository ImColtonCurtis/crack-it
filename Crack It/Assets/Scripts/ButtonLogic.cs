using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField]
    [Range(0, 14)] // questionmark, leaderboard, settings, blue, green, purple, orange, pink, yellow, backspace, enter, exit
    int buttonType;

    void OnTouchUp()
    {
        // public static bool questionMarkHit, leaderboardHit, settingsHit, blueHit, greenHit, purpleHit, orangeHit, pinkHit, yellowHit, backspaceHit, enterHit, exitHit;
        switch (buttonType)
        {
            case 0:
                GameManager.questionMarkHit = true;
                break;
            case 1:
                GameManager.leaderboardHit = true;
                break;
            case 2:
                GameManager.settingsHit = true;
                break;
            case 3:
                GameManager.blueHit = true;
                break;
            case 4:
                GameManager.greenHit = true;
                break;
            case 5:
                GameManager.purpleHit = true;
                break;
            case 6:
                GameManager.orangeHit = true;
                break;
            case 7:
                GameManager.pinkHit = true;
                break;
            case 8:
                GameManager.yellowHit = true;
                break;
            case 9:
                GameManager.redHit = true;
                break;
            case 10:
                GameManager.whiteHit = true;
                break;
            case 11:
                GameManager.darkBlueHit = true;
                break;
            case 12:
                GameManager.backspaceHit = true;
                break;
            case 13:
                GameManager.enterHit = true;
                break;
            case 14:
                GameManager.exitHit = true;
                break;
            default:
                Debug.Log("error: button number not present");
                break;
        }
    }
}
