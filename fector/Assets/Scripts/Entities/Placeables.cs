using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Placeables : ScriptableObject
{
    public Placeable[] buildings;
}

[Serializable]
public class Placeable
{
    public GameObject prefab;
    public GameObject preview;
    public Vector3 pos_offset;
    public Vector3 rot_offset;
    public Vector3 pre_pos_offset;
    public Vector3 pre_rot_offset;
}
