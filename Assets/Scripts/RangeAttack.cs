using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : Attack
{
    public static event System.Action RangeAttackEvent;
    public GameObject arrowPrefab;
    public List<GameObject> arrows;
    public int arrowCount;
    [Range(0,100)] [SerializeField] float throwSpeed;
    void OnEnable()
    {
        RangeAttackEvent += Fire;
    }
    void OnDisable()
    {
        RangeAttackEvent -= Fire;
    }
    protected override void Start()
    {
        base.Start();
        Init();
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
            Fire();
        }
        
        
    }
    public void Init()
    {
        // rangeClass =rangeClassObj.GetComponent<Range>();
        for (int i = 0; i < arrowCount-1; i++)
        {
            var arrow = Instantiate(arrowPrefab);
            arrows.Add(arrow);
        }
    }
    public void Arrow()
    {
        IEnumerator ThrowArrow()
        {
            var arrow = arrows[0];
            arrow.transform.position = transform.position;
            arrow.SetActive(true);
            arrows.RemoveAt(0);
            while(true)
            {
                if(Vector3.Distance(arrow.transform.position,target.transform.position) < .05f)
                {
                    Attackk();
                    arrows.Add(arrow);
                    arrow.SetActive(false);
                    break;
                }
                var dir = target.transform.position - transform.position;
                arrow.transform.LookAt(target.transform);
                arrow.transform.position += dir*throwSpeed*Time.deltaTime;
                yield return null;
            }
        }
        StartCoroutine(ThrowArrow());
    }
    public void Fire()
    {
        if(attack)
        {
            Arrow();
            attack = false;
        }
    }
}
