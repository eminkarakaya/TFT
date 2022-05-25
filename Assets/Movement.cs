using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    public List<Unit> allEnemies;
    Unit unit;
    public NavMeshAgent agent;
    public Unit target;
    public float speed = 2;
    void Start()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Move()
    {
        agent.SetDestination(target.transform.position);
    }
    void Update()
    {
        
        if(GameManager.Instance.gameStage == GameStage.preparatory)
        {
            
        }
        else
        {
            if(unit.state == State.walk)
            {
                agent.speed = speed;
                NearestEnemy();
                if(target == null)
                {
                    return;
                }
                Move();
            }
        }
    }
    public void NearestEnemy()
    {
        if(allEnemies.Count == 0)
        {
            return;
        }
        var nearestEnemy = allEnemies[0];
        var uzaklik = Vector3.Distance(this.transform.position , allEnemies[0].transform.position);
        for (int i = 0; i < allEnemies.Count-1; i++)
        {
            if(Vector3.Distance(this.transform.position , allEnemies[i].transform.position) < uzaklik)
            {
                uzaklik = Vector3.Distance(this.transform.position , allEnemies[i].transform.position);
                nearestEnemy = allEnemies[i];
            }
        }
        target = nearestEnemy;
    }
}
