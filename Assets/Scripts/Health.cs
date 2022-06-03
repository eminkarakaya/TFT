using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHp;
    public float baseArmor;
    public bool isDeath;
    public static event System.Action OnDeath;
    public Slider hpSlider;
    [SerializeField] private int _hp;
    public int hp{
        get => _hp;
        set{
            _hp = value;
            
            hpSlider.value = value;
            if(_hp <= 0 && !isDeath)
            {
                Death(this.GetComponent<Unit>());
                // Destroy(this.gameObject);
            }
        } 
    }    
    void Start()
    {
        HpReset();
        GameManager.InGameToPreparatory += HpReset;        
    }
    public void HpReset()
    {
        if(TryGetComponent(out Unit unit))
        {
            hpSlider.maxValue = maxHp;
            hp = maxHp;
            hpSlider.value = hp;
            isDeath = false;
        }
    }
    public void Death(Unit unit)
    {
        if(unit.TryGetComponent(out Hero hero))
        {
            isDeath = true;
            GameManager.Instance.livingHeros.Remove(unit);
            GameManager.Instance.TakeDamage();
            this.gameObject.SetActive(false);
        }
        else if(unit.TryGetComponent(out Minyonlar minyonlar))
        {
            GameManager.Instance.minionsOnGround.Remove(unit);
            isDeath = true;
            this.gameObject.SetActive(false);
        }
        OnDeath?.Invoke();
    }

}
