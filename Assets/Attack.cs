using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject rangeClassObj;
    Range rangeClass;
    public Unit unit;
    public float attackRate;
    public int damage;
    public float range = 3;
    public Unit target;

    void Start()
    {
        rangeClass =rangeClassObj.GetComponent<Range>();
        unit = GetComponent<Unit>();
        damage = unit.damage;
    }    
    public void Attackk()
    {
        unit.state = State.attack;
        IEnumerator AttackCoroutine()
        {
            for (int i = 0; i < rangeClass.enemiesInRange.Count; i++)
            {
                if(rangeClass.enemiesInRange[i].GetComponent<Health>().isDeath)
                {
                    rangeClass.enemiesInRange.Remove(rangeClass.enemiesInRange[i]);
                    i--;
                }
            }
            target.GetComponent<Health>().hp -= damage;
            yield return new WaitForSeconds(attackRate);
        }
        StartCoroutine(AttackCoroutine());
    }
    void Update()
    {
        if(rangeClass.enemiesInRange.Count > 0)
        {
            target = rangeClass.enemiesInRange[0];
        }
        else
            target = null;
        if(target == null)
        {
            unit.state = State.walk;
            return;
        }
        else
            unit.state = State.attack;

        if(unit.state == State.attack)
        {
            if(TryGetComponent(out Movement movement))
                movement.agent.speed = 0;
            Attackk();
        }
    }
}
