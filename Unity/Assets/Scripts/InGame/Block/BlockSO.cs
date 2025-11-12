using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Green,                 // 5만원
    Rad,                   // 10만원
    Blue,                  // 15만원
    Black,                 // 20만원
    Gold,                  // 30만원
    Soull,                 // 40만원
    Start                  
}

[CreateAssetMenu( fileName = "block", menuName = "scriptableObject")]
public class BlockSO : ScriptableObject
{
    public int Block_id;
    public string Block_name;
    public BlockType blockType;
    public int Block_price;
}
