using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class PathWalker : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private NavMeshAgent agent;
    private int currentDestinationIndex = -1;
    public bool loop = true;
    public float waitTimeAtWaypoint = 1.0f; // Time to wait at each waypoint

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(GoToNextPoint()); // Start the routine to go through waypoints
    }

    private IEnumerator GoToNextPoint()
    {
        while (true) // Keep this running
        {
            if (waypoints.Count == 0)
                yield break;

            currentDestinationIndex = (currentDestinationIndex + 1) % waypoints.Count;
            // Debug.Log("Going to waypoint: " + currentDestinationIndex);

            // Create a new Vector3 that takes the agent's current Y position (height)
            // and the X and Z positions from the next waypoint. This effectively ignores
            // the Y component of the waypoint.
            Vector3 nextWaypointPosition = waypoints[currentDestinationIndex].position;
            Vector3 nextPosition = new Vector3(
                nextWaypointPosition.x,
                agent.transform.position.y,
                nextWaypointPosition.z
            );

            // Set the agent to go to the above modified position.
            agent.SetDestination(nextPosition);

            // Wait until the agent has reached the current destination
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null; // Wait for the next frame
            }
            // Debug.Log("Reached waypoint: " + currentDestinationIndex);

            // Wait at the waypoint
            yield return new WaitForSeconds(waitTimeAtWaypoint);

            // If at the end of the list and not looping, break out of the loop
            if (currentDestinationIndex == waypoints.Count - 1 && !loop)
            {
                // Debug.Log("Reached final waypoint, will not loop.");
                yield break;
            }
        }
    }
}
