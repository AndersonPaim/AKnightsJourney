using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerBalancer", menuName = "PlayerBalancer")]
public class PlayerBalancer : ScriptableObject
{
    public float acceleration;
    public float deceleration;
    public float walkSpeed;
    public float runSpeed;
    public float speedMultiplier;
    public float jumpForce;
    public float dashForce;
    public float dashTime;
    public Vector3 dashDirection;
    public Vector3 gravity;
}
