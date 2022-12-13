using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] clips;
    public AudioSource[] sources;
    public ScoreScript.GameState lastState;
    public void GameStateUpdated(ScoreScript.GameState state)
    {
        switch (state)
        {
            case ScoreScript.GameState.Starting:
                //stop music
                sources[0].Stop();
                //start timer
                sources[1].loop = true;
                sources[1].clip = clips[1];
                sources[1].Play();

                //schedule Starter Jingle.
                sources[2].clip = clips[2];
                sources[2].PlayScheduled(AudioSettings.dspTime + ScoreScript.countdown);
                break;

            case ScoreScript.GameState.Running:
                //play music after starting timer.
                sources[1].Stop();
                if (lastState == ScoreScript.GameState.Starting) {
                    sources[3].clip = clips[3];
                    sources[3].PlayScheduled(AudioSettings.dspTime + clips[2].length);
                    sources[3].ignoreListenerPause = false;
                }
                AudioListener.pause = false;
                break;
            //resume game audio
            case ScoreScript.GameState.Pause:
                AudioListener.pause = true;
                break;


            //for sick victory
            case ScoreScript.GameState.Win:
                sources[3].Stop();
                sources[4].clip = clips[4];
                sources[4].Play();
                break;

            case ScoreScript.GameState.Lose:
                sources[3].Stop();
                sources[5].clip = clips[5];
                sources[5].Play();
                break;

            //reset game
            case ScoreScript.GameState.Ready:
                sources[0].clip = clips[0];
                sources[0].loop = true;
                sources[0].ignoreListenerPause = true;
                sources[0].Play();
                break;
        }
        lastState = state;
    }

    private void OnEnable()
    {
        ScoreScript.OnGameStateChange += GameStateUpdated;
    }

    private void OnDisable()
    {
        ScoreScript.OnGameStateChange -= GameStateUpdated;
    }
    void Start()
    {
        //music can play in pause menu when game not active
        sources[0].ignoreListenerPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
