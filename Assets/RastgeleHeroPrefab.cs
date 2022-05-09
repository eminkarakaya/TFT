using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RastgeleHeroPrefab : MonoBehaviour 
{
    public bool satildiMi;
    public Hero hero;    
    public HeroData heroData;
    public Text nameText;
    public Image sprite;
    public Text costText;
    public Text ozellik1Name;
    public Text ozellik2Name;
    public Text ozellik3Name;

    void Start()
    {
        HeroPanel.BuyHeroClick += BuyHero;
        SetBuyHeroButton();
    }
    public void SetBuyHeroButton()
    {
        hero = GameController.Instance.heroPool[Random.Range(0,GameController.Instance.heroPool.Count-1)];
        GameController.Instance.heroPool.Remove(hero);
        heroData = hero.heroData;
        nameText.text = heroData.name;
        sprite.sprite = heroData.sprite;
        costText.text = heroData.cost.ToString();
        ozellik1Name.text = heroData.ozellik1Name;
        ozellik2Name.text = heroData.ozellik2Name;
        ozellik3Name.text = heroData.ozellik3Name;
        GameController.Instance.heroPool.Remove(hero);
        satildiMi = false;
    }
    public void BuyHero()
    {
            // if(HeroPanel.Instance.MusaitGridBul(hero)== null)
            // {
            //     Debug.Log("Yer yok");
            //     return;
            // }
        if(HeroPanel.Instance.MusaitGridBul(hero)!= null)
        {
            satildiMi = true;
            HeroPanel.Instance.gold -= heroData.cost;
            this.gameObject.SetActive(false);
        }
    }
    public void SellHero()
    {

    }
    public void Reflesh()
    {
        if(satildiMi)
        {
            satildiMi = false;
            this.gameObject.SetActive(true);
        }
        else
        {
            GameController.Instance.heroPool.Add(hero);
        }
        SetBuyHeroButton();
        
    }
}
