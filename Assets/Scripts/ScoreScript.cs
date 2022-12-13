using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreScript : MonoBehaviour
{
    public static int leeway = 1;
    public int leewayassign = 10;
    float total = 1.0f;
    public float failgrade = 50f;

    public int numbertosortparam = 50;
    static int numbertosort;

    static float recenttotal = 1.0f;
    static int counter = 1;
    public static float currentscore;
    public static float totalscore;
    public List<float> recentscores;
    public static bool reset = false;
    public static bool paused = false;



    public static float countdown = 3.0f;
    static float starttime = 0.0f;

    //ui interaction
    public GameObject uitext;
    public Slider currentscoreslider;
    public Slider percentcompletecomponent;


    //managing game state
    public enum GameState{Ready,Starting,Running,Pause,Win,Lose};
    public static GameState state;

    public delegate void GameStateChange(GameState state);

    public static event GameStateChange OnGameStateChange;
    // Start is called before the first frame update
    void Start()
    {
        leeway = leewayassign;
        //leeway should always be bigger than 0
        if (leeway < 1)
            leeway = 1;

        //set total, recent total and counter values so that the average has a buffer room and we don't immediately game over.
        total = leeway;
        recenttotal = leeway;

        counter = leeway;
        //number of items that must be sorted before the wincondition is met.
        numbertosort = numbertosortparam+ leeway;

        //use this to keep track of the last x number of sorts. This is used for the score so that the average doesn't make everything boring.
        recentscores = new List<float>();
        for (int i = 0; i < leeway; i++)
        {
            recentscores.Add(1.0f);
        }
        updateWorldUI("Game Ready");
    }

    private void OnEnable()
    {
        //BasketScript.OnRecievedObject += UpdateScore;
        OnGameStateChange += updateGameState;
    }

    private void OnDisable()
    {
        OnGameStateChange -= updateGameState;
    }



    public static void ResetGame()
    {
        print("call resetgame");
        reset = true;
        paused = false;
    }

    public static void PauseGame()
    {
        print("toggle pause game");
        paused = !paused;
    }

    public static void PauseGame(bool pause)
    {
        print("set paused to " + pause);
        paused = pause;
    }

    void UpdateScore(float value)
    {
        print("called update score: " + value);
        total += value;
        counter++;
        recentscores.Insert(0, value);
        recenttotal += value;
        
        if (recentscores.Count > leeway)
        {
            recenttotal -= recentscores[recentscores.Count-1];
            recentscores.RemoveAt(recentscores.Count - 1);
        }
        print("recent total: " + recenttotal);
        print("percent complete = "+ getPercentDone());
    }

    float GetTotalScore()
    {
        totalscore = 100 * total / counter;
        return 100*total / counter;
    }

    public static GameState GetGameState()
    {
        return state;
    }

    public static float GetRecentScore()
    {
        
        currentscore = 100f * recenttotal / leeway;
        return 100f * recenttotal / leeway;
    }
    string GetTextScore(float format)
    {
        return string.Format("{0:00.00}%",format);
    }

    public static float getPercentDone()
    {
        print("counter = " + counter);
        print("numbertosort = " + numbertosort);
        return ((float)(counter-leeway) / (float)(numbertosort-leeway));
    }

    public void updateWorldUI(string value) {
        var test = uitext.GetComponent<UnityEngine.UI.Text>();
        test.text = value;
    }

    public void oldUiUpdate(string value)
    {
        var textmesh = GetComponentInChildren(typeof(TextMesh), false) as TextMesh;
        textmesh.text = value;
    }

    void ResetScore()
    {
        //set total, recent total and counter values so that the average has a buffer room and we don't immediately game over.
        total = leeway;
        recenttotal = leeway;
        counter = leeway;
        //number of items that must be sorted before the wincondition is met.
        numbertosort = numbertosortparam + leeway;
        currentscore = 100f;
        totalscore = 100f;
        //use this to keep track of the last x number of sorts. This is used for the score so that the average doesn't make everything boring.
        recentscores.Clear();
        for (int i = 0; i < leeway; i++)
        {
            recentscores.Add(1.0f);
        }
        updateWorldUI("Game Ready");
    }

    void updateGameState(GameState newstate)
    {
        
        switch (newstate)
        {
            case GameState.Ready:
                ResetScore();
                break;

            case GameState.Starting:
                starttime = 0f;
                break;
            case GameState.Running:
                BasketScript.OnRecievedObject += UpdateScore;
                break;
            case GameState.Pause:
                updateWorldUI("Game Paused");
                BasketScript.OnRecievedObject -= UpdateScore;
                break;
            case GameState.Win:
                updateWorldUI("You win!\n" + "Final Score: " + GetTextScore(totalscore));
                BasketScript.OnRecievedObject -= UpdateScore;
                break;
            case GameState.Lose:
                updateWorldUI("Game Over\n" + "Final Score: " + GetTextScore(totalscore));
                BasketScript.OnRecievedObject -= UpdateScore;
                break;
            default:
                break;
        }
        state = newstate;
    }


    public void OnGameChangeWrapper(GameState state)
    {
        if (OnGameStateChange != null)
        {
            print("Changing game state to " + state);
            OnGameStateChange(state);
        }
    }

    // Update is called once per frame
    void Update()
    {


        //reset logic
        if (state == GameState.Ready && reset)
        {
            reset = false;
            OnGameChangeWrapper(GameState.Starting);
        }else if (reset)
        {
            reset = false;
            OnGameChangeWrapper(GameState.Ready);
        }

        //unpause from ready
        if (state == GameState.Pause && !paused)
            OnGameChangeWrapper(GameState.Running);


        //ready from starting
        if (state == GameState.Starting)
        {
            if(starttime < countdown) {
                starttime += Time.deltaTime;
                updateWorldUI("Starting in " +Mathf.Ceil(countdown-starttime));
            } else {
                OnGameChangeWrapper(GameState.Running);
            }
        }
        //while running
        if (state == GameState.Running) { 
            updateWorldUI("Score:\n" + GetTextScore(GetTotalScore()));
            currentscoreslider.value = GetRecentScore();
           
            percentcompletecomponent.value = getPercentDone()*100f;
            //print("Current Score " + currentscore);
            if(paused == true)
            {
                OnGameChangeWrapper(GameState.Pause);
            }
            if (currentscore < failgrade)
            {
                OnGameChangeWrapper(GameState.Lose);
            }
            if(counter >= numbertosort)     
            {
                OnGameChangeWrapper(GameState.Win);
            }
        }

    }
}
