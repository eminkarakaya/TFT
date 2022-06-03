using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
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
            if(attack)
            {
                Attackk();
                attack = false;
            }
        }
        
        
    }
}
