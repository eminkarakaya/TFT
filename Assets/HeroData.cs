using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="HeroData")]
public class HeroData : ScriptableObject
{
    public int cost;
    public string name;
    public string ozellik1Name;
    public string ozellik2Name;
    public string ozellik3Name;
    public Sprite sprite;
    public float speed;
    public float damage;
    public float range;
    public Ozellik ozellik1;
    public Ozellik ozellik2;
    public Ozellik ozellik3;
}
