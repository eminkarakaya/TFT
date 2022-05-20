using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum GameStage
{
    inGame,
    preparatory
}
public class GameController : Singleton<GameController>
{
    public List<Unit> sahadakiMinyonlar;
    public List<Unit> yasayanHeros;
    public Transform poolPos;
    public List<GameObject> minionWave;
    public static event System.Action InGameToPreparatory;
    public static event System.Action PreparatoryToInGame;
    public List<GameObject> imageList; 
    public Image activeImage;
    //public List<Image> turImages;
    private int _roundPhaseCount;
    public int roundPhaseCount{
        get => _roundPhaseCount;
        set{
            imageList[_roundPhaseCount].SetActive(false);
            _roundPhaseCount = value;
            imageList[_roundPhaseCount].SetActive(true);
        }
    }
    [SerializeField] private int _turSayisi;
    public int turSayisi{
        get => _turSayisi;
        set{
            _turSayisi = value;
        }
    }
    public GameStage gameStage;
    public Slider turSuresiSlider;
    [SerializeField] private float _turSuresi;
    public float turSuresi{
        get => _turSuresi;
        set{
            _turSuresi = value;
            turSuresiSlider.value = _turSuresi;
        }
    }
    float turSuresiTemp;
    public List<Hero> heroPool;
    
    void Start()
    {
        PreparatoryToInGame += SahadakiMinyonlariSifirla;
        PreparatoryToInGame += SahadakiMinyonlarEkle;
        PreparatoryToInGame += OzellikleriAc;
        InGameToPreparatory += OzellikleriKapa;
        Health.OnDeath += CheckEnemiesMinions;
        InGameToPreparatory += NewMinionWave;
        // InGameToPreparatory += OldPos;
        PreparatoryToInGame += SetEnemiesMinionsFirst;
        InGameToPreparatory += ChangeImage;
        InGameToPreparatory += InGameToPreparatoryChangeTag;
        PreparatoryToInGame += PreparatoryToInGameChangeTag;
        turSayisi = 1;
        turSuresiSlider.maxValue = turSuresi;
        turSuresiTemp = turSuresi;
        gameStage = GameStage.preparatory;
        activeImage = imageList[roundPhaseCount].transform.GetChild(turSayisi-1).GetComponent<Image>();
    }
    void Update()
    {
        RoundCalculater();
    }
    public void RoundCalculater()
    {
        turSuresi -= Time.deltaTime;
        if(turSuresi <= 0)
        {
            if(gameStage == GameStage.inGame)
            {
                InGameToPreparatory.Invoke();
                turSayisi ++;
            }
            else if(gameStage == GameStage.preparatory)
            {
                PreparatoryToInGame.Invoke();
            }
            turSuresi = turSuresiTemp;
        }
    }
    public void SahadakiMinyonlariSifirla()
    {
        sahadakiMinyonlar = new List<Unit>();
    }
    public void NewMinionWave()
    {
        Debug.Log("newmınıonwave");
        minionWave[0].SetActive(false);
        minionWave.RemoveAt(0);
        SetEnemiesMinionsFirst();
        minionWave[0].SetActive(true);
    }
    public void OzellikleriKapa()
    {    
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            var qw = SelectManeger.Instance.sahadakiHerolar[i];
            SelectManeger.Instance.sahadakiHerolar[i].gameObject.SetActive(true);
            qw.GetComponent<Hero>().attack.enabled = false;
            qw.GetComponent<Hero>(). movement.enabled = false;
            qw.GetComponent<Hero>(). agent.enabled = false;
            qw.transform.position = new Vector3(qw.transform.parent.position.x,qw.transform.parent.position.y+.5f,qw.transform.parent.position.z);
            qw.transform.parent.position = SelectManeger.Instance.sahadakiHerolar[i].GetComponent<KarakterYerlestirme>().hangiZemin.transform.position;
        }
    }
    public void OldPos()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            SelectManeger.Instance.sahadakiHerolar[i].transform.position = GetComponent<KarakterYerlestirme>().hangiZemin.transform.position;
        }
    }
    public void OzellikleriAc()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            yasayanHeros.Add(SelectManeger.Instance.sahadakiHerolar[i]);
            SelectManeger.Instance.sahadakiHerolar[i].GetComponent<Movement>().enabled = true;
            SelectManeger.Instance.sahadakiHerolar[i].GetComponent<Attack>().enabled = true;
            SelectManeger.Instance.sahadakiHerolar[i].GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    public void CheckEnemiesMinions()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            for (int j = 0; j < minionWave[0].transform.childCount; j++)
            {
                if(minionWave[0].transform.GetChild(j).GetComponent<Health>().isDeath)
                {
                    SelectManeger.Instance.sahadakiHerolar[i].GetComponent<Movement>().allEnemies.Remove(minionWave[0].transform.GetChild(j).GetComponent<Unit>());
                }
            }
        }
    }
    public void SahadakiMinyonlarEkle()
    {
        for (int i = 0; i < minionWave[turSayisi-1].transform.childCount; i++)
        {
            sahadakiMinyonlar.Add(minionWave[turSayisi-1].transform.GetChild(i).GetComponent<Unit>());
        }
    }
    public void SahadakiMinyonlarSil(Unit unit)
    {
        sahadakiMinyonlar.Remove(unit);
    }

    public void SetEnemiesMinionsFirst()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            for (int j = 0; j < minionWave[0].transform.childCount; j++)
            {
                SelectManeger.Instance.sahadakiHerolar[i].GetComponent<Movement>().allEnemies.Add(minionWave[0].transform.GetChild(j).GetComponent<Unit>());
            }

            
        }
    }
    public void InGameToPreparatoryChangeTag()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            SelectManeger.Instance.sahadakiHerolar[i].tag = "Char";
        }
        gameStage = GameStage.preparatory;
    }
    public void PreparatoryToInGameChangeTag()
    {
        for (int i = 0; i < SelectManeger.Instance.sahadakiHerolar.Count; i++)
        {
            SelectManeger.Instance.sahadakiHerolar[i].tag = "Oyunda";
        }
        gameStage = GameStage.inGame;
    }
    // IEnumerator RoundCalculater()
    // {
    //     yield return new WaitForSeconds(Time)
    // }
    public void ChangeImage()
    {
        var childCount = imageList[roundPhaseCount].transform.childCount;
        activeImage.enabled = false;
        activeImage = imageList[roundPhaseCount].transform.GetChild(turSayisi-1).GetComponent<Image>();
        // activeImage = turImages[_turSayisi];
        activeImage.enabled = true;
        if(turSayisi == childCount)
        {
            turSayisi = 1;
            roundPhaseCount ++;
            activeImage = imageList[roundPhaseCount].transform.GetChild(turSayisi-1).GetComponent<Image>();
        }
    }
}
