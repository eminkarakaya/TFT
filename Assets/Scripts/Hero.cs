using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum Ozellik
{
    kartel,
    kimyasal,
    atilgan,
    yordle,
    kavgaci,
    hextech,
    fedai,
    sihirUstadi,
    kazanova,
    koruma,
    cifteAtis,
    bos
}
public enum State
{
    walk,
    attack
}
public abstract class Hero : Unit
{
    protected abstract void UseSkill();
    [HideInInspector] public Attack attack;
    [HideInInspector] public Movement movement;
    public HeroData heroData;
    Ozellik ozellik;
    Ozellik ozellik2;
    Ozellik ozellik3;
    protected override void Start()
    {
        base.Start();
        attack = GetComponent<Attack>();
        movement = GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
    }
    // public void OzellikleriAc()
    // {
    //     if(GetComponent<KarakterYerlestirme>().hangiZemin != null)
    //     {
    //         if(GetComponent<KarakterYerlestirme>().hangiZemin.GetComponent<GridController>().yedekMi == false) 
    //         {
    //             attack.enabled = true;
    //             movement.enabled = true;
    //         }
    //     }
    // }
    public void PoolPoint()
    {
        transform.position = GameManager.Instance.poolPos.position;
    }
}
