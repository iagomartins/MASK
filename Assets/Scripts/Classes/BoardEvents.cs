using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardEvents
{
    public int spotToGo;
    public Question question;
    public string eventText;
    public bool isShortcut = false;
    public EventType type;

    public void AddHealthPoint (GameObject player) {
        int _checkHealthMaxValue = player.GetComponent<FollowThePath>().health;
        player.GetComponent<FollowThePath>().health++;
    }

    public void MoveThroughStairs (GameObject player, int index) {
        FollowThePath _playerControl = player.GetComponent<FollowThePath>();
        while (player.transform.position != _playerControl.waypoints[index].transform.position) {
            player.transform.position = Vector2.MoveTowards(player.transform.position,
            _playerControl.waypoints[index].transform.position,
            _playerControl.GetPlayerSpeed() * Time.deltaTime);
        }
        _playerControl.waypointIndex = index;
    }
}

[System.Serializable]
public class Question {
    public string myQuestion;
    public string[] options;
    public int correctAnswerIndex;

    public bool SubmitAnswer(int index) {
        return index == correctAnswerIndex;
    }
}

public enum EventType {
    AddLife,
    Virus,
    AddSpots
}