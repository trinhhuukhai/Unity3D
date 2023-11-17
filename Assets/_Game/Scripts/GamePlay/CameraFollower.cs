using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{

    [SerializeField] Transform player;
    [SerializeField] Vector3 intialOffset;
    [SerializeField] Vector3 offset;
    public Vector3 Offset { get => offset; set => offset = value; }


    private void Awake()
    {   
        GetInstance();
    }
    void Update()
    {
        if (player != null)
            transform.position = Vector3.Lerp(transform.position, player.position + offset, 0.5f);
    }
    public void SetTargetFollow(Transform target)
    {
        player = target;
    }
    public void Play()
    {
        offset = intialOffset;
    }

}
