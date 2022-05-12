using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Vector3 forwardDirection;

    public Vector3 direction => forwardDirection;
}
