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
    public Animator animator;
    public float maxMana;
    public float mana;
    public Side side;
    public State state;
    [HideInInspector] public NavMeshAgent agent;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
}
