using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoon : MonoBehaviour
{
    public Transform player;
	void Update ()
    {
        transform.RotateAround(player.position, Vector3.up, 10);
    }
}
