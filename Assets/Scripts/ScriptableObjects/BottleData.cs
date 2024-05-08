using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottleType{
    Empty,
    Full
}
public class BottleData : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    public BottleType bottleType;
    public GameObject prefab;
}
