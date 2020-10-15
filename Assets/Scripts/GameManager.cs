using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("Song Variables")]
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    public TextAsset timingScript;

    public StateMachine machine;

    public List<TimingEvent> allTimingEvents = new List<TimingEvent>();
    [SerializeField]
    int currentTimingEventNum = 0;
    public float songStartTime;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music


        parseTimingScript();

        startSongAtTime(songStartTime);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
        updateState();


        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SceneManager.LoadScene("ElevatorScene");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneManager.LoadScene("SoonerMinigame");
        }
        //update state here
    }

    private void startSongAtTime(float time) {
        foreach (TimingEvent te in allTimingEvents) {
            if (allTimingEvents[currentTimingEventNum].time < time)
            {
                currentTimingEventNum++;
            }
            else {
                break;
            }
        }
        musicSource.time = time;
        dspSongTime -= time;
        musicSource.Play();

    }

    public void parseTimingScript() {

        TimingEvent prevLine = null;
        int lineNum = 0;
        foreach (string line in timingScript.text.Split('\n')) {
            if (lineNum > 0) {
                prevLine = allTimingEvents[lineNum - 1];
            }
            TimingEvent newTimingEvent = parseTimingLine(line, prevLine);
            allTimingEvents.Add(newTimingEvent);
            Debug.Log(newTimingEvent.name);
            lineNum++;

        }
    }
    private TimingEvent parseTimingLine(string _line, TimingEvent _prevTimingEvent) {
        string[] lineArray = _line.Split('-');
        lineArray[1] = lineArray[1].Trim();
        if (lineArray[1].Contains("Minigame"))
        {
            return new TimingEvent(parseTimingFloat(lineArray[0]), lineArray[1], TimingEvent.StateType.Game);
        }
        else {
            return new TimingEvent(parseTimingFloat(lineArray[0]), lineArray[1], TimingEvent.StateType.Elevator);
        }
        
        
    }
    private float parseTimingFloat(string _floatString) {
        _floatString = _floatString.Trim();
        string[] floatArray = _floatString.Split('m');
        float min = 0;
        float sec = 0;
        if (floatArray.Length == 1) {
            sec = float.Parse(floatArray[0]);
        }
        else {

            min = float.Parse(floatArray[0]);
            sec = float.Parse(floatArray[1]);
        }

        return 60f * min + sec;
    }

    private void initializeStateMachine() {
        machine.AddState(FindObjectOfType<ElevatorState>(), StateMachine.StateType.Elevator);
        machine.AddState(FindObjectOfType<MinigameState>(), StateMachine.StateType.Minigame);
        machine.setInitialState(StateMachine.StateType.Elevator);


    }

    //updates the song state based on time
    private void updateState() {
        if (songPosition > allTimingEvents[currentTimingEventNum].time) {

            switch (allTimingEvents[currentTimingEventNum].stateType) {
                case TimingEvent.StateType.Game:
                    Debug.Log("loading game");
                    break;
                case TimingEvent.StateType.Elevator:
                    Debug.Log("loading elevator");
                    break;
                default:
                    break;
            }

            currentTimingEventNum++;
        }
    }

   
}

public class TimingEvent {

    public float time;
    public string name;
    public enum StateType {
        Game,
        Elevator,
        Boss,
        Tutorial
    }
    public StateType stateType;
    public TimingEvent(float _time, string _name, StateType _stateType) {
        time = _time;
        name = _name;
        stateType = _stateType;
    }
}