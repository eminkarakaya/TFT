using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minyonlar : Unit
{
    public Movement movement;
    public Attack attack;
    void Start()
    {
        Health.OnDeath += SetEnemies;
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        GameManager.PreparatoryToInGame += SetEnemies;
    }
    public void SetEnemies()
    {
        attack.enabled = true;
        movement.enabled = true;
        movement.allEnemies = GameManager.Instance.livingHeros;
    }
}
