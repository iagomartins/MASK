using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameControl : MonoBehaviour {

    private static GameObject player1MoveText, player2MoveText;

    private static GameObject player1, player2;
    private static FollowThePath playerRef1, playerRef2;
    public static int playerToRespond = 1;
    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static bool isGamePaused;
    public static bool isQuestionActivated = false;

    public static bool gameOver = false;

    //Setup your game mechanics here.
    public GameObject menuPanel;
    public GameObject eventPanel;
    public GameObject whoWinsTextShadow;
    public BoardEvents[] eventMechanics;
    public Text player1Life, player2Life;
    public TextMeshProUGUI eventText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] optionsTexts;
    public GameObject questionPanel;
    public GameObject questionMarker;
    public GameObject maskIcon;
    public GameObject coronaIcon;
    public List<int> questionPositions;
    public List<int> maskPositions;
    public List<int> coronaPositions;
    public Transform[] waypoints;

    public static GameControl instance;

    // Use this for initialization
    void Start () {

        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        playerRef1 = player1.GetComponent<FollowThePath>();
        playerRef2 = player2.GetComponent<FollowThePath>();

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
        questionPanel.SetActive(false);
        menuPanel.SetActive(false);
        eventPanel.SetActive(false);

        SpawnIcons();

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        player1Life.text = "Vidas: " + playerRef1.health;
        player2Life.text = "Vidas: " + playerRef2.health;
        menuPanel.SetActive(isGamePaused);

        if(Input.GetKeyDown(KeyCode.Escape)) {
            isGamePaused = !isGamePaused;
        }
        if (player1.GetComponent<FollowThePath>().waypointIndex > 
            player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            isQuestionActivated = false;
            player1MoveText.gameObject.SetActive(false);
            playerToRespond = 1;
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;

            if (eventMechanics[player1StartWaypoint - 1].question.myQuestion != "")
            {
                if(!isQuestionActivated) {
                    questionPanel.SetActive(true);
                    isQuestionActivated = true;
                }
                questionText.text = eventMechanics[player1StartWaypoint - 1].question.myQuestion;
                optionsTexts[0].text = eventMechanics[player1StartWaypoint - 1].question.options[0];
                optionsTexts[1].text = eventMechanics[player1StartWaypoint - 1].question.options[1];
                optionsTexts[2].text = eventMechanics[player1StartWaypoint - 1].question.options[2];
                optionsTexts[3].text = eventMechanics[player1StartWaypoint - 1].question.options[3];
            }

            if (eventMechanics[player1StartWaypoint - 1].eventText != "" && (!playerRef1.moveAllowed || !playerRef1.movingBack))
            {
                eventText.text = eventMechanics[player1StartWaypoint - 1].eventText;
                eventPanel.SetActive(true);
            }

            if (eventMechanics[player1StartWaypoint - 1].isShortcut) {
                eventMechanics[player1StartWaypoint - 1].MoveThroughStairs(player1, eventMechanics[player1StartWaypoint - 1].spotToGo);
                player1StartWaypoint = eventMechanics[player1StartWaypoint - 1].spotToGo;
            }
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex >
            player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            isQuestionActivated = false;
            player2MoveText.gameObject.SetActive(false);
            playerToRespond = 2;
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;

            if (eventMechanics[player2StartWaypoint - 1].question.myQuestion != "")
            {
                if(!isQuestionActivated) {
                    questionPanel.SetActive(true);
                    isQuestionActivated = true;
                }
                questionText.text = eventMechanics[player2StartWaypoint - 1].question.myQuestion;
                optionsTexts[0].text = eventMechanics[player2StartWaypoint - 1].question.options[0];
                optionsTexts[1].text = eventMechanics[player2StartWaypoint - 1].question.options[1];
                optionsTexts[2].text = eventMechanics[player2StartWaypoint - 1].question.options[2];
                optionsTexts[3].text = eventMechanics[player2StartWaypoint - 1].question.options[3];
            }

            if (eventMechanics[player2StartWaypoint - 1].eventText != "" && (!playerRef2.moveAllowed || !playerRef2.movingBack))
            {
                eventText.text = eventMechanics[player2StartWaypoint - 1].eventText;
                eventPanel.SetActive(true);
            }

            if (eventMechanics[player2StartWaypoint - 1].isShortcut) {
                eventMechanics[player2StartWaypoint - 1].MoveThroughStairs(player2, eventMechanics[player2StartWaypoint - 1].spotToGo);
                player2StartWaypoint = eventMechanics[player1StartWaypoint - 1].spotToGo;
            }
        }


        if (player1.GetComponent<FollowThePath>().waypointIndex == 
            player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            whoWinsTextShadow.GetComponentInChildren<Text>().text = "O jogador 1 venceu!";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponentInChildren<Text>().text = "O jogador 2 venceu!";
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }

    public void SpawnIcons() {
        for (int i = 0; i < questionPositions.Count; i++)
        {
            var myNextObject = Instantiate(questionMarker, waypoints[questionPositions[i]].position, Quaternion.identity , waypoints[questionPositions[i]]);
        }
        for (int i = 0; i < coronaPositions.Count; i++)
        {
            var myNextObject = Instantiate(coronaIcon, waypoints[coronaPositions[i]].position, Quaternion.identity , waypoints[coronaPositions[i]]);
        }
        for (int i = 0; i < maskPositions.Count; i++)
        {
            var myNextObject = Instantiate(maskIcon, waypoints[maskPositions[i]].position, Quaternion.identity , waypoints[maskPositions[i]]);
        }
    }

    public void ActivateEvent() {
        int player = playerToRespond;
        switch (player) {
            case 1:
                switch (eventMechanics[player1StartWaypoint - 1].type) {
                    case EventType.AddLife:
                        eventMechanics[player1StartWaypoint - 1].AddHealthPoint(player1);
                        eventMechanics[player1StartWaypoint - 1].eventText = "";
                        eventPanel.SetActive(false);
                        break;
                    case EventType.Virus:
                        playerRef1.moveAllowed = false;
                        playerRef1.GoBack(eventMechanics[player1StartWaypoint - 1].spotToGo);
                        eventPanel.SetActive(false);
                        break;
                    case EventType.AddSpots:
                        playerRef1.moveAllowed = true;
                        eventPanel.SetActive(false);
                        break;
                }
                break;
            case 2:
                switch (eventMechanics[player2StartWaypoint - 1].type) {
                    case EventType.AddLife:
                        eventMechanics[player2StartWaypoint - 1].AddHealthPoint(player2);
                        eventMechanics[player2StartWaypoint - 1].eventText = "";
                        eventPanel.SetActive(false);
                        break;
                    case EventType.Virus:
                        playerRef2.GoBack(eventMechanics[player2StartWaypoint - 1].spotToGo);
                        playerRef2.moveAllowed = false;
                        eventPanel.SetActive(false);
                        break;
                    case EventType.AddSpots:
                        playerRef2.moveAllowed = true;
                        eventPanel.SetActive(false);
                        break;
                }
                break;
        }
    }

    public void TestQuestion(int answer) {
        int index = 0;
        switch (playerToRespond) {
            case 1:
                index = player1StartWaypoint - 1;
                break;
            case 2:
                index = player2StartWaypoint - 1;
                break;
        }
        Debug.Log(eventMechanics[index].question.myQuestion);
        Debug.Log(index);
        if(eventMechanics[index].question.SubmitAnswer(answer)) {
            isQuestionActivated = true;
            questionPanel.SetActive(false);
            return;
        }
        else {
            FollowThePath playerRef = null;
            switch(playerToRespond) {
                case 1:
                    playerRef = player1.GetComponent<FollowThePath>();
                    playerRef.health--;
                    break;
                case 2:
                    playerRef = player2.GetComponent<FollowThePath>();
                    playerRef.health--;
                    break;
                default:
                    break;
            }
        }
        questionPanel.SetActive(false);
    }

    public void ReturnToGame() {
        isGamePaused = false;
    }
}
