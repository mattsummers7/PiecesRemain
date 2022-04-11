using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour
{
    public int speedFloat;

    private void Awake()
    {
        speedFloat = Animator.StringToHash("Speed");
    }
}
