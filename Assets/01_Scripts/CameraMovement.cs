using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerToFollow;
    public float distanceInX;
    public float distanceInY;
    public float distanceInZ;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerToFollow = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerToFollow.position.x - distanceInX, playerToFollow.position.y - distanceInY, playerToFollow.position.z - distanceInZ);
    }
}
