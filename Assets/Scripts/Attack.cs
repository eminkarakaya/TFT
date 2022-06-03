using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool attack;
    Animator animator;
    [SerializeField] protected Range rangeClass;
    protected Unit unit;
    [SerializeField] protected float attackRate;
    protected float attackRateTemp;
    [SerializeField] protected int ad;
    protected int ap;
    public float range = 3;
    [SerializeField] protected Unit target;
    protected virtual void Start()
    {
        attackRateTemp = attackRate;
        unit = GetComponent<Unit>();
        animator = unit.animator;
    }
    protected virtual void Update()
    {
        attackRate-= Time.deltaTime;
        
        if(attackRate < 0)
        {
            //if(target != null)
//                animator.SetTrigger("Attack");
            attackRate = attackRateTemp;
            attack = true;
        }
    
    }
    protected void Attackk()
    {
        unit.state = State.attack;
        target.GetComponent<Health>().hp -= ad;
        for (int i = 0; i < rangeClass.enemiesInRange.Count; i++)
        {
            if(rangeClass.enemiesInRange[i].GetComponent<Health>().isDeath)
            {
                rangeClass.enemiesInRange.Remove(rangeClass.enemiesInRange[i]);
                i--;
            }
        }
        unit.mana += 10;
    }
}
