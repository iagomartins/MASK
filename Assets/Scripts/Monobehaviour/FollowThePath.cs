using UnityEngine;

public class FollowThePath : MonoBehaviour {

    public Transform[] waypoints;

    [SerializeField]
    public float moveSpeed = 1f;

    public int returnDestination = 0;
    [HideInInspector]
    public int waypointIndex = 0;

    public int health;
    public bool moveAllowed = false;
    public bool movingBack = false;

	// Use this for initialization
	private void Start () {
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
        if (health <= 0) {
            waypointIndex = 0;
            transform.position =  waypoints[waypointIndex].transform.position;
            health = 3;
            moveAllowed = false;
            movingBack = false;
            return;
        }
        if (moveAllowed)
            Move();
        if (movingBack)
            MoveBackwards();
	}

    private void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }

        }
    }

    private void MoveBackwards() {
        if (waypointIndex > returnDestination)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex -= 1;
                Debug.Log(returnDestination);
                Debug.Log(waypointIndex);
            }
        }
        else {
            movingBack = false;
        }
    }

    public void GoBack(int index) {
        returnDestination = index;
        moveAllowed = false;
        movingBack = true;
    }

    public float GetPlayerSpeed() {
        return moveSpeed;
    }
}
