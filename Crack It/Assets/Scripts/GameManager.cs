using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Xml.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] answerStrokes0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerStrokes1 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerStrokes2 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerStrokes3 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerStrokes4 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerStrokes5 = new SpriteRenderer[4];

    [SerializeField] SpriteRenderer[] buttonStrokes0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] buttonStrokes1 = new SpriteRenderer[4];

    [SerializeField] SpriteRenderer[] answerFill0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerFill1 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerFill2 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerFill3 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerFill4 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerFill5 = new SpriteRenderer[4];

    [SerializeField] SpriteRenderer[] buttonFill0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] buttonFill1 = new SpriteRenderer[4];

    [SerializeField] SpriteRenderer[] answerKeyStroke0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyStroke1 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyStroke2 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyStroke3 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyStroke4 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyStroke5 = new SpriteRenderer[4];

    [SerializeField] SpriteRenderer[] answerKeyFill0 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyFill1 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyFill2 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyFill3 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyFill4 = new SpriteRenderer[4];
    [SerializeField] SpriteRenderer[] answerKeyFill5 = new SpriteRenderer[4];

    [SerializeField] Color[] hoveredColors = new Color[9];

    [SerializeField] Color[] keyColors = new Color[3];
    [SerializeField] GameObject loserObj, winnerObj;

    [SerializeField] SpriteRenderer[] answerDots1 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots2 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots3 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots4 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots5 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots6 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots7 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots8 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots9 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots10 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots11 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots12 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots13 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots14 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots15 = new SpriteRenderer[9];
    [SerializeField] SpriteRenderer[] answerDots16 = new SpriteRenderer[9];

    int prevColorSelected;
    public static bool playerWon, playerLost;

    // questionmark, leaderboard, settings, blue, green, purple, orange, pink, yellow, backspace, enter, exit
    public static bool questionMarkHit, leaderboardHit, settingsHit, blueHit, greenHit, purpleHit, orangeHit, pinkHit, yellowHit, redHit, whiteHit, darkBlueHit, backspaceHit, enterHit, exitHit;
    int currentRow = 0;
    int setUpCurrentRow = 0;
    int currentColumn = 0;
    int tempColumn = 0;
    
    int[] code = new int[4];
    int[] currentResponse = new int[4];

    int amountCorrect = 0;

    SpriteRenderer chosenSR;
    Color chosenColor = Color.blue;

    bool showingHowTo = false, showingStats = false;
    [SerializeField] GameObject howToObj, statsObj, headerObj;

    [SerializeField] SpriteRenderer transitionBlock;

    private void Awake()
    {
        playerWon = false;
        playerLost = false;
        howToObj.SetActive(false);
        statsObj.SetActive(false);

        transitionBlock.color = Color.black;
        transitionBlock.enabled = true;

        prevColorSelected = 0;

        GetDaysSincePlayed();
        if (PlayerPrefs.GetInt("DaysSincePlayed", 0) >= 1)
        {
            // new code
            CreateCode();
            PlayerPrefs.SetInt("GamePrimed", 1);
            PlayerPrefs.SetInt("PlayerRow", 0);
            PlayerPrefs.SetInt("PlayerHasWon", 0);
            PlayerPrefs.SetInt("PlayerHasLost", 0);

            if (PlayerPrefs.GetInt("DaysSincePlayed", 0) > 1)
                PlayerPrefs.SetInt("CurrentStreak", 0);
        }
        else
        {
            SetCode();
            setUpCurrentRow = PlayerPrefs.GetInt("PlayerRow", 0);
            // spawn prev level
            if (setUpCurrentRow > 0)
                StartCoroutine(SetOrbs());

            if (PlayerPrefs.GetInt("PlayerHasWon", 0) == 1)
            {
                StartCoroutine(BringHeaderIn());
                winnerObj.SetActive(true);
                loserObj.SetActive(false);
                playerWon = true;
            }

            if (PlayerPrefs.GetInt("PlayerHasLost", 0) == 1)
            {
                StartCoroutine(BringHeaderIn());
                winnerObj.SetActive(false);
                loserObj.SetActive(true);
                playerLost = true;
            }
        }    
        
        if (PlayerPrefs.GetInt("HowToShown", 0) == 0)
        {
            showingHowTo = true;
            StartCoroutine(ShowHowTo());
            PlayerPrefs.SetInt("HowToShown", 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutTransition());
    }

    // Update is called once per frame
    void Update()
    {
        if (questionMarkHit)
        {
            if (!showingHowTo && !showingStats)
            {
                showingHowTo = true;
                StartCoroutine(ShowHowTo());
            }
            questionMarkHit = false;
        }
        else if (leaderboardHit)
        {
            if (!showingHowTo && !showingStats)
            {
                showingStats = true;
                StartCoroutine(ShowLeaderboard());
            }
            leaderboardHit = false;
        }
        else if (settingsHit)
        {
            settingsHit = false;
        }
        else if (!playerLost && !playerWon)
        {
            if (blueHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(0));
                blueHit = false;
            }
            else if (greenHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(1));
                greenHit = false;
            }
            else if (purpleHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(2));
                purpleHit = false;
            }
            else if (orangeHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(3));
                orangeHit = false;
            }
            else if (pinkHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(4));
                pinkHit = false;
            }
            else if (yellowHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(5));
                yellowHit = false;
            }
            else if (redHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(6));
                redHit = false;
            }
            else if (whiteHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(7));
                whiteHit = false;
            }
            else if (darkBlueHit)
            {
                if (PlayerPrefs.GetInt("GamePrimed", 0) == 1)
                {
                    PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 0) + 1); // increment
                    PlayerPrefs.SetInt("GamePrimed", 0);
                }
                StartCoroutine(ChangeHoverFill(8));
                darkBlueHit = false;
            }
            else if (backspaceHit)
            {
                if (currentColumn > 0)
                    currentColumn--;
                RemoveHoverFill();
                backspaceHit = false;
            }
            else if (enterHit)
            {                
                if (currentColumn == 4)
                {
                    StartCoroutine(AnswerKeyCheck());
                }
                enterHit = false;
            }
        }
        if (exitHit)
        {
            if (showingHowTo)
            {
                StartCoroutine(HideHowTo());
                showingHowTo = false;
            }
            else if (showingStats)
            {
                StartCoroutine(HideLeaderboard());
                showingStats = false;
            }

            exitHit = false;
        }
    }

    IEnumerator FadeOutTransition()
    {
        transitionBlock.color = Color.black;
        transitionBlock.enabled = true;

        yield return new WaitForSecondsRealtime(0.2f);

        float timer = 0, totalTimer = 8;

        while (timer <= totalTimer)
        {
            transitionBlock.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), timer / totalTimer);
            yield return new WaitForFixedUpdate();
            timer++;
        }

    }

    IEnumerator BringHeaderIn()
    {
        float timer = 0, totalTime = 30;

        yield return new WaitForSecondsRealtime(1f);

        Vector3 endingPos = new Vector3(0, 1.02f, 0);
        Vector3 startingPos = endingPos + new Vector3(0, 1.1f, 0);

        SpriteRenderer tempSR = headerObj.GetComponent<SpriteRenderer>();
        while (timer <= totalTime)
        {
            tempSR.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0), Color.white, timer / totalTime);
            headerObj.transform.localPosition = Vector3.Lerp(startingPos, endingPos, timer / totalTime);
            timer++;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ShowHowTo()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(SlideHowTo(true));
    }

    IEnumerator SlideHowTo(bool goingUp)
    {
        float timer = 0, totalTime = 9;
        Vector3 endingPos = new Vector3(0, -7.629395e-05f, 0);
        Vector3 startingPos = endingPos - new Vector3(0, 0.3f, 0);

        SpriteRenderer tempSR = howToObj.GetComponent<SpriteRenderer>();
        if (!goingUp)
        {
            endingPos = howToObj.transform.localPosition - new Vector3(0, 0.4f, 0);
            startingPos = howToObj.transform.localPosition;
        }
        else
        {
            tempSR.color = new Color(0.5f, 0.5f, 0.5f, 0);
            howToObj.SetActive(true);
        }

        while (timer <= totalTime)
        {
            // fade in
            if (goingUp)
                tempSR.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0), Color.white, timer / totalTime);
            else
                tempSR.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer / totalTime);

            howToObj.transform.localPosition = Vector3.Lerp(startingPos, endingPos, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        if (!goingUp)
            howToObj.SetActive(false);
    }

    IEnumerator HideHowTo()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(SlideHowTo(false));
    }

    IEnumerator ShowLeaderboard()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(SlideLeasderboard(true));
    }

    IEnumerator SlideLeasderboard(bool goingUp)
    {
        float timer = 0, totalTime = 9;
        Vector3 endingPos = new Vector3(0, -7.629395e-05f, 0);
        Vector3 startingPos = endingPos - new Vector3(0, 0.3f, 0);

        SpriteRenderer tempSR = statsObj.GetComponent<SpriteRenderer>();

        if (!goingUp)
        {
            endingPos = statsObj.transform.localPosition - new Vector3(0, 0.4f, 0);
            startingPos = statsObj.transform.localPosition;
        }
        else
        {
            tempSR.color = new Color(0.5f, 0.5f, 0.5f, 0);
            statsObj.SetActive(true);
        }

        while (timer <= totalTime)
        {
            // fade in
            if (goingUp)
                tempSR.color = Color.Lerp(new Color(0.5f, 0.5f, 0.5f, 0), Color.white, timer / totalTime);
            else
                tempSR.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timer / totalTime);

            statsObj.transform.localPosition = Vector3.Lerp(startingPos, endingPos, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        if (!goingUp)
            statsObj.SetActive(false);
    }

    IEnumerator HideLeaderboard()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(SlideLeasderboard(false));
    }

    void CreateCode()
    {
        int colorsAmnt = 9;
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = UnityEngine.Random.Range(0, colorsAmnt);

            switch (i)
            {
                case 0:
                    break;
                case 1:
                    while (code[1] == code[0])
                        code[1] = UnityEngine.Random.Range(0, colorsAmnt);
                    break;
                case 2:
                    while (code[2] == code[1] || code[2] == code[0])
                        code[2] = UnityEngine.Random.Range(0, colorsAmnt);
                    break;
                case 3:
                    while (code[3] == code[2] || code[3] == code[1] || code[3] == code[0])
                        code[3] = UnityEngine.Random.Range(0, colorsAmnt);
                    break;
                default:
                    Debug.Log("Error: code creator");
                    break;
            }

            Debug.Log(code[i].ToString());
        }
        PlayerPrefs.SetInt("Code0", code[0]);
        PlayerPrefs.SetInt("Code1", code[1]);
        PlayerPrefs.SetInt("Code2", code[2]);
        PlayerPrefs.SetInt("Code3", code[3]);
    }

    void SetCode()
    {
        code[0] = PlayerPrefs.GetInt("Code0", 0);
        code[1] = PlayerPrefs.GetInt("Code1", 1);
        code[2] = PlayerPrefs.GetInt("Code2", 2);
        code[3] = PlayerPrefs.GetInt("Code3", 3);
    }

    IEnumerator ChangeAnswerCircleSize()
    {
        float timer = 0, totalTime = 2;
        Transform tempTransform = answerStrokes0[currentColumn].transform;
        switch (currentRow)
        {
            case 0:
                tempTransform = answerStrokes0[currentColumn].transform;
                break;
            case 1:
                tempTransform = answerStrokes1[currentColumn].transform;
                break;
            case 2:
                tempTransform = answerStrokes2[currentColumn].transform;
                break;
            case 3:
                tempTransform = answerStrokes3[currentColumn].transform;
                break;
            case 4:
                tempTransform = answerStrokes4[currentColumn].transform;
                break;
            case 5:
                tempTransform = answerStrokes5[currentColumn].transform;
                break;
            default:
                Debug.Log("Error: size transform");
                break;
        }
        // grow
        Vector3 startScale = tempTransform.localScale;
        Vector3 endScale = tempTransform.localScale*1.125f;
        while (timer <= totalTime)
        {
            tempTransform.localScale = Vector3.Lerp(startScale, endScale, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        timer = 0;
        totalTime = 6;
        while (timer <= totalTime)
        {
            tempTransform.localScale = Vector3.Lerp(endScale, startScale, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    void ChangeDots(SpriteRenderer[] myAnswerDots, int selectedColor)
    {
        switch (selectedColor)
        {
            case 0:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = false;
                myAnswerDots[5].enabled = false;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = false;
                myAnswerDots[8].enabled = false;
                break;
            case 1:
                myAnswerDots[0].enabled = false;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = false;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = false;
                break;
            case 2:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = false;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = false;
                break;
            case 3:
                myAnswerDots[0].enabled = false;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            case 4:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            case 5:
                myAnswerDots[0].enabled = false;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = true;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = true;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            case 6:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = true;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = true;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            case 7:
                myAnswerDots[0].enabled = false;
                myAnswerDots[1].enabled = true;
                myAnswerDots[2].enabled = true;
                myAnswerDots[3].enabled = true;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = true;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            case 8:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = true;
                myAnswerDots[2].enabled = true;
                myAnswerDots[3].enabled = true;
                myAnswerDots[4].enabled = true;
                myAnswerDots[5].enabled = true;
                myAnswerDots[6].enabled = true;
                myAnswerDots[7].enabled = true;
                myAnswerDots[8].enabled = true;
                break;
            default:
                myAnswerDots[0].enabled = true;
                myAnswerDots[1].enabled = false;
                myAnswerDots[2].enabled = false;
                myAnswerDots[3].enabled = false;
                myAnswerDots[4].enabled = false;
                myAnswerDots[5].enabled = false;
                myAnswerDots[6].enabled = false;
                myAnswerDots[7].enabled = false;
                myAnswerDots[8].enabled = false;
                break;
        }
    }

    IEnumerator ChangeHoverFill(int colorNum)
    {        
        // set dots here
        prevColorSelected = colorNum;
        int selectedAnswer = (currentColumn + 1) + (currentRow * 4);

        if (!(currentRow + 1 == 1 && selectedAnswer == 5))
        {
            switch (selectedAnswer)
            {
                case 1:
                    ChangeDots(answerDots1, prevColorSelected);
                    break;
                case 2:
                    ChangeDots(answerDots2, prevColorSelected);
                    break;
                case 3:
                    ChangeDots(answerDots3, prevColorSelected);
                    break;
                case 4:
                    ChangeDots(answerDots4, prevColorSelected);
                    break;
                case 5:
                    ChangeDots(answerDots5, prevColorSelected);
                    break;
                case 6:
                    ChangeDots(answerDots6, prevColorSelected);
                    break;
                case 7:
                    ChangeDots(answerDots7, prevColorSelected);
                    break;
                case 8:
                    ChangeDots(answerDots8, prevColorSelected);
                    break;
                case 9:
                    ChangeDots(answerDots9, prevColorSelected);
                    break;
                case 10:
                    ChangeDots(answerDots10, prevColorSelected);
                    break;
                case 11:
                    ChangeDots(answerDots11, prevColorSelected);
                    break;
                case 12:
                    ChangeDots(answerDots12, prevColorSelected);
                    break;
                case 13:
                    ChangeDots(answerDots13, prevColorSelected);
                    break;
                case 14:
                    ChangeDots(answerDots14, prevColorSelected);
                    break;
                case 15:
                    ChangeDots(answerDots15, prevColorSelected);
                    break;
                case 16:
                    ChangeDots(answerDots16, prevColorSelected);
                    break;
                default:
                    break;
            }
        }

        if (currentColumn < 4)
        {
            currentResponse[currentColumn] = colorNum;

            chosenSR = answerFill0[currentColumn];
            StartCoroutine(ChangeAnswerCircleSize());
            chosenColor = hoveredColors[colorNum];
            switch (currentRow)
            {
                case 0:
                    chosenSR = answerFill0[currentColumn];
                    // set color player prefs
                    switch (currentColumn)
                    {
                        case 0:
                            PlayerPrefs.SetInt("Orb00", colorNum);
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Orb01", colorNum);
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Orb02", colorNum);
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Orb03", colorNum);
                            break;
                        default:
                            Debug.Log("Error: setting color player pref");
                            break;
                    }
                    break;
                case 1:
                    chosenSR = answerFill1[currentColumn];
                    // set color player prefs
                    switch (currentColumn)
                    {
                        case 0:
                            PlayerPrefs.SetInt("Orb10", colorNum);
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Orb11", colorNum);
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Orb12", colorNum);
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Orb13", colorNum);
                            break;
                        default:
                            Debug.Log("Error: setting color player pref");
                            break;
                    }
                    break;
                case 2:
                    chosenSR = answerFill2[currentColumn];
                    // set color player prefs
                    switch (currentColumn)
                    {
                        case 0:
                            PlayerPrefs.SetInt("Orb20", colorNum);
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Orb21", colorNum);
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Orb22", colorNum);
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Orb23", colorNum);
                            break;
                        default:
                            Debug.Log("Error: setting color player pref");
                            break;
                    }
                    break;
                case 3:
                    chosenSR = answerFill3[currentColumn];
                    // set color player prefs
                    switch (currentColumn)
                    {
                        case 0:
                            PlayerPrefs.SetInt("Orb30", colorNum);
                            break;
                        case 1:
                            PlayerPrefs.SetInt("Orb31", colorNum);
                            break;
                        case 2:
                            PlayerPrefs.SetInt("Orb32", colorNum);
                            break;
                        case 3:
                            PlayerPrefs.SetInt("Orb33", colorNum);
                            break;
                        default:
                            Debug.Log("Error: setting color player pref");
                            break;
                    }
                    break;
                case 4:
                    chosenSR = answerFill4[currentColumn];
                    break;
                case 5:
                    chosenSR = answerFill5[currentColumn];
                    break;
                default:
                    Debug.Log("Error: color picker");
                    break;
            }
            currentColumn++;
        }

        float timer = 0, totalTime = 3;
        Color startColor = chosenSR.color;
        while (timer <= totalTime)
        {
            // chosenColor
            chosenSR.color = Color.Lerp(startColor, chosenColor, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator SetOrbs()
    {
        // currentRow
        for (int i = 0; i < setUpCurrentRow; i++)
        {
            switch (i)
            {
                case 0:
                    ChangeDots(answerDots1, PlayerPrefs.GetInt("Orb00", 0));
                    ChangeDots(answerDots2, PlayerPrefs.GetInt("Orb01", 0));
                    ChangeDots(answerDots3, PlayerPrefs.GetInt("Orb02", 0)); 
                    ChangeDots(answerDots4, PlayerPrefs.GetInt("Orb03", 0));

                    answerFill0[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].b, 1);
                    answerFill0[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].b, 1);
                    answerFill0[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].b, 1);
                    answerFill0[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].b, 1);

                    answerStrokes0[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb00", 0)].b, 1);
                    answerStrokes0[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb01", 0)].b, 1);
                    answerStrokes0[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb02", 0)].b, 1);
                    answerStrokes0[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb03", 0)].b, 1);

                    currentResponse[0] = PlayerPrefs.GetInt("Orb00", 0);
                    currentResponse[1] = PlayerPrefs.GetInt("Orb01", 0);
                    currentResponse[2] = PlayerPrefs.GetInt("Orb02", 0);
                    currentResponse[3] = PlayerPrefs.GetInt("Orb03", 0);

                    break;
                case 1:
                    ChangeDots(answerDots5, PlayerPrefs.GetInt("Orb10", 0));
                    ChangeDots(answerDots6, PlayerPrefs.GetInt("Orb11", 0));
                    ChangeDots(answerDots7, PlayerPrefs.GetInt("Orb12", 0));
                    ChangeDots(answerDots8, PlayerPrefs.GetInt("Orb13", 0));

                    answerFill1[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].b, 1);
                    answerFill1[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].b, 1);
                    answerFill1[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].b, 1);
                    answerFill1[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].b, 1);

                    answerStrokes1[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb10", 0)].b, 1);
                    answerStrokes1[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb11", 0)].b, 1);
                    answerStrokes1[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb12", 0)].b, 1);
                    answerStrokes1[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb13", 0)].b, 1);

                    currentResponse[0] = PlayerPrefs.GetInt("Orb10", 0);
                    currentResponse[1] = PlayerPrefs.GetInt("Orb11", 0);
                    currentResponse[2] = PlayerPrefs.GetInt("Orb12", 0);
                    currentResponse[3] = PlayerPrefs.GetInt("Orb13", 0);

                    break;
                case 2:
                    ChangeDots(answerDots9, PlayerPrefs.GetInt("Orb20", 0));
                    ChangeDots(answerDots10, PlayerPrefs.GetInt("Orb21", 0));
                    ChangeDots(answerDots11, PlayerPrefs.GetInt("Orb22", 0));
                    ChangeDots(answerDots12, PlayerPrefs.GetInt("Orb23", 0));

                    answerFill2[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].b, 1);
                    answerFill2[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].b, 1);
                    answerFill2[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].b, 1);
                    answerFill2[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].b, 1);

                    answerStrokes2[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb20", 0)].b, 1);
                    answerStrokes2[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb21", 0)].b, 1);
                    answerStrokes2[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb22", 0)].b, 1);
                    answerStrokes2[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb23", 0)].b, 1);

                    currentResponse[0] = PlayerPrefs.GetInt("Orb20", 0);
                    currentResponse[1] = PlayerPrefs.GetInt("Orb21", 0);
                    currentResponse[2] = PlayerPrefs.GetInt("Orb22", 0);
                    currentResponse[3] = PlayerPrefs.GetInt("Orb23", 0);

                    break;
                case 3:
                    ChangeDots(answerDots13, PlayerPrefs.GetInt("Orb30", 0));
                    ChangeDots(answerDots14, PlayerPrefs.GetInt("Orb31", 0));
                    ChangeDots(answerDots15, PlayerPrefs.GetInt("Orb32", 0));
                    ChangeDots(answerDots16, PlayerPrefs.GetInt("Orb33", 0));

                    answerFill3[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].b, 1);
                    answerFill3[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].b, 1);
                    answerFill3[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].b, 1);
                    answerFill3[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].b, 1);

                    answerStrokes3[0].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb30", 0)].b, 1);
                    answerStrokes3[1].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb31", 0)].b, 1);
                    answerStrokes3[2].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb32", 0)].b, 1);
                    answerStrokes3[3].color = new Color(hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].r, hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].g, hoveredColors[PlayerPrefs.GetInt("Orb33", 0)].b, 1);

                    currentResponse[0] = PlayerPrefs.GetInt("Orb30", 0);
                    currentResponse[1] = PlayerPrefs.GetInt("Orb31", 0);
                    currentResponse[2] = PlayerPrefs.GetInt("Orb32", 0);
                    currentResponse[3] = PlayerPrefs.GetInt("Orb33", 0);

                    break;
                default:
                    Debug.Log("Error Setting orbs");
                    break;
            }
            tempColumn = 0;
            // grow first answer key
            //StartCoroutine(GrowAnswerKey());
            // fade first answer key color in
            StartCoroutine(FadeAnswerKey());
            yield return new WaitForFixedUpdate();

            tempColumn = 1;
            // grow first answer key
            //StartCoroutine(GrowAnswerKey());
            // fade first answer key color in
            StartCoroutine(FadeAnswerKey());
            yield return new WaitForFixedUpdate();

            tempColumn = 2;
            // grow first answer key
            //StartCoroutine(GrowAnswerKey());
            // fade first answer key color in
            StartCoroutine(FadeAnswerKey());
            yield return new WaitForFixedUpdate();

            tempColumn = 3;
            // grow first answer key
            //StartCoroutine(GrowAnswerKey());
            // fade first answer key color in
            StartCoroutine(FadeAnswerKey());
            yield return new WaitForFixedUpdate();
            currentRow++;
        }
    }

    IEnumerator AnswerKeyCheck()
    {
        amountCorrect = 0;

        tempColumn = 0;
        // fade first answer in
        StartCoroutine(FadeAnswerIn());
        yield return new WaitForSecondsRealtime(0.01f);
        // grow first answer key
        StartCoroutine(GrowAnswerKey());
        // fade first answer key color in
        StartCoroutine(FadeAnswerKey());
        yield return new WaitForSecondsRealtime(0.3f);

        tempColumn = 1;
        // fade first answer in
        StartCoroutine(FadeAnswerIn());
        yield return new WaitForSecondsRealtime(0.01f);
        // grow first answer key
        StartCoroutine(GrowAnswerKey());
        // fade first answer key color in
        StartCoroutine(FadeAnswerKey());
        yield return new WaitForSecondsRealtime(0.3f);

        tempColumn = 2;
        // fade first answer in
        StartCoroutine(FadeAnswerIn());
        yield return new WaitForSecondsRealtime(0.01f);
        // grow first answer key
        StartCoroutine(GrowAnswerKey());
        // fade first answer key color in
        StartCoroutine(FadeAnswerKey());
        yield return new WaitForSecondsRealtime(0.3f);

        tempColumn = 3;
        // fade first answer in
        StartCoroutine(FadeAnswerIn());
        yield return new WaitForSecondsRealtime(0.01f);
        // grow first answer key
        StartCoroutine(GrowAnswerKey());
        // fade first answer key color in
        StartCoroutine(FadeAnswerKey());

        if (amountCorrect == 4)
        {
            playerWon = true;
            PlayerPrefs.SetInt("CurrentStreak", PlayerPrefs.GetInt("CurrentStreak", 0)+1);

            if (PlayerPrefs.GetInt("CurrentStreak", 0) > PlayerPrefs.GetInt("MaxStreak", 0))
                PlayerPrefs.SetInt("MaxStreak", PlayerPrefs.GetInt("CurrentStreak", 0));

            currentRow++;
            PlayerPrefs.SetInt("GamesWon", PlayerPrefs.GetInt("GamesWon", 0) + 1);
            PlayerPrefs.SetInt("PlayerRow", currentRow);
            PlayerPrefs.SetInt("PlayerHasWon", 1);
            StartCoroutine(BringHeaderIn());
            winnerObj.SetActive(true);
            loserObj.SetActive(false);
            Debug.Log("Game Won");
        }
        else
        {
            // wrap-up
            if (currentRow < 3)
            {
                currentColumn = 0;
                currentRow++;
                PlayerPrefs.SetInt("PlayerRow", currentRow);
            }
            else
            {
                playerLost = false;
                currentRow++;
                PlayerPrefs.SetInt("PlayerRow", currentRow);
                PlayerPrefs.SetInt("PlayerHasLost", 1);
                StartCoroutine(BringHeaderIn());
                PlayerPrefs.SetInt("CurrentStreak", 0);
                winnerObj.SetActive(false);
                loserObj.SetActive(true);
                Debug.Log("Game Over");
            }
        }
    }

    IEnumerator GrowAnswerKey()
    {
        Transform tempTransform = answerKeyStroke0[tempColumn].transform;
        switch (currentRow)
        {
            case 0:
                tempTransform = answerKeyStroke0[tempColumn].transform;
                break;
            case 1:
                tempTransform = answerKeyStroke1[tempColumn].transform;
                break;
            case 2:
                tempTransform = answerKeyStroke2[tempColumn].transform;
                break;
            case 3:
                tempTransform = answerKeyStroke3[tempColumn].transform;
                break;
            case 4:
                tempTransform = answerKeyStroke4[tempColumn].transform;
                break;
            case 5:
                tempTransform = answerKeyStroke5[tempColumn].transform;
                break;
            default:
                Debug.Log("Error: answer key transform");
                break;
        }

        // grow
        Vector3 startScale = tempTransform.localScale;
        Vector3 endScale = tempTransform.localScale * 1.125f;
        float timer = 0, totalTime = 3;
        while (timer <= totalTime)
        {
            tempTransform.localScale = Vector3.Lerp(startScale, endScale, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        timer = 0;
        totalTime = 8;
        while (timer <= totalTime)
        {
            tempTransform.localScale = Vector3.Lerp(endScale, startScale, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeAnswerKey()
    {
        SpriteRenderer tempSRStroke = answerKeyStroke0[tempColumn], tempSRFill = answerKeyFill0[tempColumn];

        switch (currentRow)
        {
            case 0:
                tempSRStroke = answerKeyStroke0[tempColumn];
                tempSRFill = answerKeyFill0[tempColumn];
                break;
            case 1:
                tempSRStroke = answerKeyStroke1[tempColumn];
                tempSRFill = answerKeyFill1[tempColumn];
                break;
            case 2:
                tempSRStroke = answerKeyStroke2[tempColumn];
                tempSRFill = answerKeyFill2[tempColumn];
                break;
            case 3:
                tempSRStroke = answerKeyStroke3[tempColumn];
                tempSRFill = answerKeyFill3[tempColumn];
                break;
            case 4:
                tempSRStroke = answerKeyStroke4[tempColumn];
                tempSRFill = answerKeyFill4[tempColumn];
                break;
            case 5:
                tempSRStroke = answerKeyStroke5[tempColumn];
                tempSRFill = answerKeyFill5[tempColumn];
                break;
            default:
                Debug.Log("Error: answer key SR");
                break;
        }

        Color startStrokeColor = tempSRStroke.color;
        Color startFillColor = tempSRFill.color;
        Color endColor = keyColors[0];

        // check for greens first, then yellows?

        // check for correct placement
        if (currentResponse[tempColumn] == code[tempColumn])
        {
            endColor = keyColors[2];
            amountCorrect++;
        }
        else
        {
            // check if color exists in answer
            for (int i = 0; i < code.Length; i++)
            {
                if (currentResponse[tempColumn] == code[i])
                {
                    // before awarding yellow, ensure its not a duplicate
                    if (currentResponse[i] != code[i])
                    {
                        // Ensure yellow hasn't already been awarded for this color
                        // loop through current response to find earlier duplicate
                        int dupCount = 0;
                        for (int j = 0; j <= tempColumn; j++)
                        {
                            if (currentResponse[tempColumn] == currentResponse[j])
                                dupCount++;
                        }

                        // loop through code to find duplicates count
                        for (int j = 0; j < code.Length; j++)
                        {
                            if (currentResponse[tempColumn] == code[j])
                                dupCount--;
                        }

                        if (dupCount <= 0)
                        {
                            endColor = keyColors[1];
                            break;
                        }
                    }
                }
            }
        }
        float timer = 0, totalTime = 5;
        while (timer <= totalTime)
        {
            tempSRStroke.color = Color.Lerp(startStrokeColor, endColor, timer / totalTime);
            tempSRFill.color = Color.Lerp(startFillColor, endColor, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeAnswerIn()
    {
        SpriteRenderer tempAnswerSRStroke = answerKeyStroke0[tempColumn], tempAnswerSRFill = answerKeyFill0[tempColumn];

        switch (currentRow)
        {
            case 0:
                tempAnswerSRStroke = answerStrokes0[tempColumn];
                tempAnswerSRFill = answerFill0[tempColumn];
                break;
            case 1:
                tempAnswerSRStroke = answerStrokes1[tempColumn];
                tempAnswerSRFill = answerFill1[tempColumn];
                break;
            case 2:
                tempAnswerSRStroke = answerStrokes2[tempColumn];
                tempAnswerSRFill = answerFill2[tempColumn];
                break;
            case 3:
                tempAnswerSRStroke = answerStrokes3[tempColumn];
                tempAnswerSRFill = answerFill3[tempColumn];
                break;
            case 4:
                tempAnswerSRStroke = answerStrokes4[tempColumn];
                tempAnswerSRFill = answerFill4[tempColumn];
                break;
            case 5:
                tempAnswerSRStroke = answerStrokes5[tempColumn];
                tempAnswerSRFill = answerFill5[tempColumn];
                break;
            default:
                Debug.Log("Error: answer SR");
                break;
        }

        Color startStrokeColor = tempAnswerSRStroke.color;
        Color startFillColor = tempAnswerSRFill.color;
        Color endColor = new Color(tempAnswerSRFill.color.r, tempAnswerSRFill.color.g, tempAnswerSRFill.color.b, 1);
        float timer = 0, totalTime = 5;
        while (timer <= totalTime)
        {
            tempAnswerSRStroke.color = Color.Lerp(startStrokeColor, endColor, timer / totalTime);
            tempAnswerSRFill.color = Color.Lerp(startFillColor, endColor, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    void RemoveHoverFill()
    {
        switch (currentRow)
        {
            case 0:
                answerFill0[currentColumn].color = new Color(answerFill0[currentColumn].color.r, answerFill0[currentColumn].color.g, answerFill0[currentColumn].color.b, 0);
                break;
            case 1:
                answerFill1[currentColumn].color = new Color(answerFill1[currentColumn].color.r, answerFill1[currentColumn].color.g, answerFill1[currentColumn].color.b, 0);
                break;
            case 2:
                answerFill2[currentColumn].color = new Color(answerFill2[currentColumn].color.r, answerFill2[currentColumn].color.g, answerFill2[currentColumn].color.b, 0);
                break;
            case 3:
                answerFill3[currentColumn].color = new Color(answerFill3[currentColumn].color.r, answerFill3[currentColumn].color.g, answerFill3[currentColumn].color.b, 0);
                break;
            case 4:
                answerFill4[currentColumn].color = new Color(answerFill4[currentColumn].color.r, answerFill4[currentColumn].color.g, answerFill4[currentColumn].color.b, 0);
                break;
            case 5:
                answerFill5[currentColumn].color = new Color(answerFill5[currentColumn].color.r, answerFill5[currentColumn].color.g, answerFill5[currentColumn].color.b, 0);
                break;
            default:
                Debug.Log("Error: color");
                break;
        }
    }

    int dayCount;
    DateTime compStartDateTime, timeAtStart;

    // determine days since played 
    void GetDaysSincePlayed()
    {
        compStartDateTime = new DateTime(2022, 8, 10, 0, 0, 0);
        timeAtStart = InterpolatedUtcNow;
        timeAtStart = timeAtStart.AddHours(-7); // get current time

        dayCount = timeAtStart.DayOfYear - compStartDateTime.DayOfYear;
        PlayerPrefs.SetInt("DaysSincePlayed", dayCount - PlayerPrefs.GetInt("PrevDayCount", 0));
        PlayerPrefs.SetInt("PrevDayCount", dayCount);
        Debug.Log("days since last played: " + PlayerPrefs.GetInt("DaysSincePlayed", 0));
    }

    public DateTime LastSyncedLocalTime
    {
        get;
        private set;
    }

    public DateTime LastSyncedServerTime
    {
        get;
        private set;
    }

    public DateTime InterpolatedUtcNow
    {
        get
        {
            return DateTime.UtcNow + (LastSyncedServerTime - LastSyncedLocalTime);
        }
    }

    public IEnumerator Sync()
    {
        using (var www = UnityWebRequest.Get("https://nist.time.gov/actualtime.cgi"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||  www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                yield break;
            }

            var timestamp = XElement.Parse(www.downloadHandler.text);
            var ticks = long.Parse(timestamp.Attribute(XName.Get("time")).Value) * 10;

            LastSyncedLocalTime = DateTime.UtcNow;
            LastSyncedServerTime = new DateTime(1970, 1, 1).AddTicks(ticks);
        }
    }
}
