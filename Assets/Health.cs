using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public bool isDeath;
    public static event System.Action OnDeath;
    public Slider hpSlider;
    [SerializeField] private int _hp;
    public int hp{
        get => _hp;
        set{
            _hp = value;
            hpSlider.value = value;
            if(_hp <= 0)
            {
                Death(this.GetComponent<Unit>());
                // Destroy(this.gameObject);
            }
        } 
    }    
    void Start()
    {
        HpReset();
        GameController.InGameToPreparatory += HpReset;        
    }
    public void HpReset()
    {
        if(TryGetComponent(out Unit unit))
        {
            hpSlider.maxValue = unit.maxHp;
            hp = unit.maxHp;
            hpSlider.value = hp;
        }
    }
    public void Death(Unit unit)
    {
        if(unit.TryGetComponent(out Hero hero))
        {
            GameController.Instance.yasayanHeros.Remove(unit);
            this.gameObject.SetActive(false);
        }
        else if(unit.TryGetComponent(out Minyonlar minyonlar))
        {
            GameController.Instance.sahadakiMinyonlar.Remove(unit);
            isDeath = true;
            this.gameObject.SetActive(false);
        }
            OnDeath?.Invoke();
    }

}
