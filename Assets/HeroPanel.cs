using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


 // 2-6-10-20-36-56-80-max
public class HeroPanel : Singleton<HeroPanel>
{
    public Button levelButton;
    public int toplamExp;
    int [] sliderMaxValues = new int[] {0,2,6,8,18,36,56,80,0};
    public int levelButtonCount = 4;
    public Button lockBtn;
    public Sprite lockClose;
    public Sprite lockOpen;
    public bool isLocked;
    public Slider expSlider;
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
    private int _exp;
    public int exp{ get => _exp;
    set{
        _exp = value;
        if(exp >= expSlider.maxValue)
        {
            int artanExp  = _exp -(int)expSlider.maxValue;
            level ++;
            expSlider.maxValue = sliderMaxValues[level];
            _exp = artanExp;
        }
        if(level >= 8)
        {
            expText.text = ("Max").ToString();
            levelButton.interactable = false;
        }
        else
        {
            expText.text = (_exp + "/" + (int)expSlider.maxValue).ToString();
        }
        expSlider.value = _exp;
    }
    }
    private int _level;
    public int level{
        get => _level;
        set {
            _level = value;
            levelText.text = _level.ToString();
            SelectManeger.Instance.sahadakiOyuncuSayisiText.text =SelectManeger.Instance.GetSahaIciOyuncuSayisi().ToString()+ "/" + _level;
        }
    }
    public static event System.Action BuyHeroClick; 
    public List<GridController> yedekKlubesi;
    public GridController musaitGrid;

    void Start()
    {
        exp = 0;
        expSlider.maxValue = 2;
        level = 1;
        BuyHeroClick += BuyHero;
    }
    public GridController MusaitGridBul(Hero hero)
    {
        for (int i = 0; i < yedekKlubesi.Count; i++)
        {
            if(yedekKlubesi[i].uzerindekiChar == null)
            {
                yedekKlubesi[i].uzerindekiChar = hero.gameObject;
                hero.transform.parent.position = yedekKlubesi[i].transform.position;
                hero.GetComponent<KarakterYerlestirme>().hangiZemin = yedekKlubesi[i].gameObject;
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
    public void GetLevel()
    {
        exp += levelButtonCount;
        toplamExp += levelButtonCount;
        gold -= levelButtonCount;
    }
}
