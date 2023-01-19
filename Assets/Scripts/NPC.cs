using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] float moveSpeed = 14f;
    [SerializeField] Waypoints enterWaypoints;
    [SerializeField] Waypoints leaveWaypoints;

    bool isDone = false;
    Animator animator;
    int waypointIndex;

    void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("NPCGraphic").GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log($"Are we done yet? {isDone}");
        if (isDone)
        {
            Debug.Log("move with leaveWaypoints");
            Move(leaveWaypoints);
        }
        else
        {
            Move(enterWaypoints);

        }
    }

    void Move(Waypoints waypoints)
    {
        Debug.Log("Inside the Move method?");
        Transform waypoint = waypoints.waypoints[waypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
        Vector3 direction = (waypoint.position - transform.position).normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        Debug.Log("Moving...");
        Debug.Log(waypointIndex);

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat("LastHorizontal", direction.x);
            animator.SetFloat("LastVertical", direction.y);
        }

        if (Vector2.Distance(transform.position, waypoint.position) == 0)
        {
            if (waypointIndex < waypoints.waypoints.Length - 1)
            {
                waypointIndex++;
            }
            else
            {
                DoSomething();
            }
        }
    }

    void DoSomething()
    {
        Debug.Log("Done the job...");
        isDone = true;
        waypointIndex = 0;
    }
}
