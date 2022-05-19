using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Side
{
    enemy,
    ally
}
public class Unit : MonoBehaviour
{
    public int maxHp;
    public Side side;
    public State state;
    public int speed;
    public float range = 3;
    public int damage;
    public int hp;
    public int mana;
    public float attackRate;
    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
