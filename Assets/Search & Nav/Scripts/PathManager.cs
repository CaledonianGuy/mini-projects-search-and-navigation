using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathManager : MonoBehaviour
{
    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    static PathManager instance;
    PathFinding pathFinding;

    bool isProcessing;

    void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(Vector3 start, Vector3 finish, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(start, finish, callback);
        instance.requestQueue.Enqueue(newRequest);
        instance.ProcessNext();
    }

    void ProcessNext()
    {
        if (!isProcessing && requestQueue.Count > 0)
        {
            isProcessing = true;
            currentRequest = requestQueue.Dequeue();
            pathFinding.StartFindPath(currentRequest.start, currentRequest.finish);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        ProcessNext();
    }

    struct PathRequest
    {
        public Vector3 start;
        public Vector3 finish;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 finish, Action<Vector3[], bool> callback)
        {
            this.start = start;
            this.finish = finish;
            this.callback = callback;
        }
    }
}
