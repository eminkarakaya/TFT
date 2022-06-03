using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    Animator animator;
    public List<Unit> allEnemies;
    Unit unit;
    [HideInInspector] public NavMeshAgent agent;
    public Unit target;
    public float speed = 2;
    void Start()
    {
        unit = GetComponent<Unit>();
        agent = GetComponent<NavMeshAgent>();
        animator = unit.animator;
    }
    public void Move()
    {
        agent.SetDestination(target.gameObject.transform.position);
    }
    void Update()
    {
        if(GameManager.Instance.gameStage == GameStage.inGame)
        {
            if(unit.state == State.walk)
            {
                agent.speed = speed;
                NearestEnemy();
                if(target == null)
                {
                    return;
                }
               // animator.SetBool("Move" , true);
                Debug.Log("MOVE TRUE ");
                speed = 3;
            }
            else
            {
                speed = 0;
               // animator.SetBool("Move" , false);
            }
        }
        Move();
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
            Debug.Log("nearest " + nearestEnemy);
        }
        target = nearestEnemy;
    }
}
