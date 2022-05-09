using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : Singleton<HeroPanel>
{
    public Button lockBtn;
    public Sprite lockClose;
    public Sprite lockOpen;
    public bool isLocked;
    public Text goldText;
    public Text expText;
    public Text levelText;
    private int _gold;
    public int gold{ get => _gold;
    set{
        _gold = value;
        goldText.text = _gold.ToString();
    }
    }
    public int exp;
    public int level;
    public static event System.Action BuyHeroClick; 
    public List<GridController> yedekKlubesi;
    public GridController musaitGrid;

    void Start()
    {
        BuyHeroClick += BuyHero;
    }
    public GridController MusaitGridBul(Hero hero)
    {
        for (int i = 0; i < yedekKlubesi.Count; i++)
        {
            if(yedekKlubesi[i].uzerindekiChar == null)
            {
                Debug.Log("yerlestirildi");
                yedekKlubesi[i].uzerindekiChar = hero.gameObject;
                hero.transform.parent.position = yedekKlubesi[i].transform.position;
                hero.GetComponent<KarakterYerlestirme>().hangiZemin = yedekKlubesi[i].gameObject;
                Debug.Log(yedekKlubesi[i]);
                return yedekKlubesi[i];
            }
        }

        return null;
    }
    public void BuyHero()
    {
        BuyHeroClick?.Invoke();
    }
    public void Lock()
    {
        isLocked = !isLocked;
        if(isLocked)
        {
            lockBtn.image.sprite = lockClose;
        }
        else
        {
            lockBtn.image.sprite = lockOpen;
        }
    }
}
