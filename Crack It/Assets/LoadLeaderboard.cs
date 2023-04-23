using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadLeaderboard : MonoBehaviour
{
    [SerializeField] TextMeshPro playedCountText, winPercText, currentStreakText, maxStreakText;
    float winercentage = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        playedCountText.text = PlayerPrefs.GetInt("GamesPlayed", 0).ToString();
        if (PlayerPrefs.GetInt("GamesPlayed", 0) == 0)
            playedCountText.text = "0";

        winercentage = PlayerPrefs.GetInt("GamesWon", 0) / (float)PlayerPrefs.GetInt("GamesPlayed", 0);
        
        winercentage *= 100;
        winPercText.text = winercentage.ToString("0");
        if (PlayerPrefs.GetInt("GamesPlayed", 0) == 0)
            winPercText.text = "0";

        currentStreakText.text = PlayerPrefs.GetInt("CurrentStreak", 0).ToString();
        if (PlayerPrefs.GetInt("CurrentStreak", 0) == 0)
            currentStreakText.text = "0";

        maxStreakText.text = PlayerPrefs.GetInt("MaxStreak", 0).ToString();
        if (PlayerPrefs.GetInt("MaxStreak", 0) == 0)
            maxStreakText.text = "0";

        StartCoroutine(WaitABit());
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForFixedUpdate();

        playedCountText.text = PlayerPrefs.GetInt("GamesPlayed", 0).ToString();
        if (PlayerPrefs.GetInt("GamesPlayed", 0) == 0)
            playedCountText.text = "0";

        winercentage = PlayerPrefs.GetInt("GamesWon", 0) / (float)PlayerPrefs.GetInt("GamesPlayed", 0);

        winercentage *= 100;
        winPercText.text = winercentage.ToString("0");
        if (PlayerPrefs.GetInt("GamesPlayed", 0) == 0)
            winPercText.text = "0";

        currentStreakText.text = PlayerPrefs.GetInt("CurrentStreak", 0).ToString();
        if (PlayerPrefs.GetInt("CurrentStreak", 0) == 0)
            currentStreakText.text = "0";

        maxStreakText.text = PlayerPrefs.GetInt("MaxStreak", 0).ToString();
        if (PlayerPrefs.GetInt("MaxStreak", 0) == 0)
            maxStreakText.text = "0";
    }
}
