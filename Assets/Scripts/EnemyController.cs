using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int currentWaypointIndex = 1;
    private GameManager gameManager;
    private List<Transform> waypoints = new List<Transform>();
    private bool isMassiveVapeReached = false;
    public float speed;
    public int health = 200;

    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();

        foreach(Transform waypoint in GameObject.Find("Waypoints").transform)
        {
            waypoints.Add(waypoint);
        }
        
        transform.position = waypoints[0].position;
    }

    private void FixedUpdate()
    {
        if(waypoints.Count == 0 || isMassiveVapeReached)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.fixedDeltaTime;

        if(Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if(currentWaypointIndex >= waypoints.Count)
            {
                OnMassiveVapeReached();
            }
        }
    }

    private void OnMassiveVapeReached()
    {
        isMassiveVapeReached = true;
        gameManager.GameOver();
    }

    void OnParticleCollision(GameObject other)
    {
        health--;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
