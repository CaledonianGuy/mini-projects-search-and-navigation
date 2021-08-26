using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitSettings settings;
    public Transform target;

    Vector3[] path;
    int index;

    void Start()
    {
        PathManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] path, bool pathFound)
    {
        if (pathFound)
        {
            this.path = path;
            index = 0;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                index++;

                if (index >= path.Length)
                {
                    break;
                }

                currentWaypoint = path[index];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, settings.speed * Time.deltaTime);
            Vector3 dir = currentWaypoint - transform.position;
            transform.forward = Vector3.Slerp(transform.forward, dir, settings.slerpSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
