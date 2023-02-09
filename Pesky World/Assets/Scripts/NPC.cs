using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AnimatorKey
{
    IsTalking,
    Horizontal,
    Vertical,
    Speed
}

public class NPC : MonoBehaviour
{
    [SerializeField] float moveSpeed = 14f;
    [SerializeField] Waypoints enterWaypoints;
    [SerializeField] Waypoints leaveWaypoints;
    [SerializeField] private GameObject dialogPanel; 

    bool shouldLeave = true;
    float pauseTime = 3.0f;

    bool shouldPause = true;

    Animator animator;
    int currentWaypointIndex;

    void Awake()
    {
        animator = GameObject.FindGameObjectWithTag("NPCGraphic").GetComponent<Animator>();
    }

    void Update()
    {
        Move(enterWaypoints);
    }

    IEnumerator DoPause()
    {
        animator.SetBool("IsSittingLeft", true);
        dialogPanel.SetActive(true);
        yield return new WaitForSeconds(pauseTime);
        animator.SetBool("IsSittingLeft", false);

        if (shouldPause)
        {
            currentWaypointIndex = 0;
            enterWaypoints = leaveWaypoints;
            shouldPause = false;
        }
        dialogPanel.SetActive(false);
    }

    void Move(Waypoints waypoints)
    {
        Transform waypoint = waypoints.waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, waypoint.position, moveSpeed * Time.deltaTime);
        Vector3 direction = (waypoint.position - transform.position).normalized;

        animator.SetFloat(AnimatorKey.Horizontal.ToString(), direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat("LastHorizontal", direction.x);
            animator.SetFloat("LastVertical", direction.y);
        }

        if (Vector2.Distance(transform.position, waypoint.position) == 0)
        {
            if (currentWaypointIndex < waypoints.waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                if (shouldPause)
                {

                    StartCoroutine(DoPause());
                }
            }
        }

    }
}
