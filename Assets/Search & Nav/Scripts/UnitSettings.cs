using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class UnitSettings : ScriptableObject
{
    [Header("Unit Settings")]
    public float speed = 10;
    public float slerpSpeed = 1;
}
