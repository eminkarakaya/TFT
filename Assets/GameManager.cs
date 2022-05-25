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
public class GameManager : Singleton<GameManager>
{
    public List<Unit> minionsOnGround;
    public List<Unit> livingHeros;
    public Transform poolPos;
    public List<GameObject> minionWave;
    public static event System.Action InGameToPreparatory;
    public static event System.Action PreparatoryToInGame;
    public List<GameObject> imageList; 
    public Image activeImage;
    //public List<Image> turImages;
    private int _roundStageCount;
    public int roundStageCount{
        get => _roundStageCount;
        set{
            imageList[_roundStageCount].SetActive(false);
            _roundStageCount = value;
            imageList[_roundStageCount].SetActive(true);
        }
    }
    public Text roundText;
    [SerializeField] private int _roundCount;
    public int roundCount{
        get => _roundCount;
        set{
            _roundCount = value;
        }
    }
    public GameStage gameStage;
    public Slider roundTimeSlider;
    [SerializeField] private float _roundTime;
    public float roundTime{
        get => _roundTime;
        set{
            _roundTime = value;
            roundTimeSlider.value = _roundTime;
        }
    }
    float roundTimeTemp;
    public List<Hero> heroPool;
    
    void Start()
    {
        PreparatoryToInGame += PlaceMinion;
        PreparatoryToInGame += OpenProperties;
        InGameToPreparatory += CloseProperties;
        Health.OnDeath += CheckEnemiesMinions;
        InGameToPreparatory += NewMinionWave;
        PreparatoryToInGame += SetEnemiesMinionsFirst;
        InGameToPreparatory += ChangeImage;
        InGameToPreparatory += InGameToPreparatoryChangeTag;
        PreparatoryToInGame += PreparatoryToInGameChangeTag;
        roundCount = 1;
        roundTimeSlider.maxValue = roundTime;
        roundStageCount = 1;
        roundTimeTemp = roundTime;
        gameStage = GameStage.preparatory;
        activeImage = imageList[roundStageCount].transform.GetChild(roundCount-1).GetComponent<Image>();
    }
    void Update()
    {
        RoundCalculater();
    }
    public void RoundCalculater()
    {
        roundTime -= Time.deltaTime;
        if(roundTime <= 0)
        {
            if(gameStage == GameStage.inGame)
            {
                InGameToPreparatory.Invoke();
                roundCount ++;
            }
            else if(gameStage == GameStage.preparatory)
            {
                PreparatoryToInGame.Invoke();
            }
            roundTime = roundTimeTemp;
        }
    }
    public void NewMinionWave()
    {
        minionWave[0].SetActive(false);
        minionWave.RemoveAt(0);
        SetEnemiesMinionsFirst();
        minionWave[0].SetActive(true);
    }
    public void CloseProperties()
    {    
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            var qw = SelectManager.Instance.HerosOnField[i];
            SelectManager.Instance.HerosOnField[i].gameObject.SetActive(true);
            qw.GetComponent<Hero>().attack.enabled = false;
            qw.GetComponent<Hero>(). movement.enabled = false;
            qw.GetComponent<Hero>(). agent.enabled = false;
            qw.transform.position = new Vector3(qw.transform.parent.position.x,qw.transform.parent.position.y+.5f,qw.transform.parent.position.z);
            qw.transform.parent.position = SelectManager.Instance.HerosOnField[i].GetComponent<HeroPlacement>().whichFloor.transform.position;
        }
    }
    public void OpenProperties()
    {
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            livingHeros.Add(SelectManager.Instance.HerosOnField[i]);
            SelectManager.Instance.HerosOnField[i].GetComponent<Movement>().enabled = true;
            SelectManager.Instance.HerosOnField[i].GetComponent<Attack>().enabled = true;
            SelectManager.Instance.HerosOnField[i].GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    public void CheckEnemiesMinions()
    {
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            for (int j = 0; j < minionWave[0].transform.childCount; j++)
            {
                if(minionWave[0].transform.GetChild(j).GetComponent<Health>().isDeath)
                {
                    SelectManager.Instance.HerosOnField[i].GetComponent<Movement>().allEnemies.Remove(minionWave[0].transform.GetChild(j).GetComponent<Unit>());
                }
            }
        }
    }
    public void PlaceMinion()
    {
        for (int i = 0; i < minionWave[roundCount-1].transform.childCount; i++)
        {
            minionsOnGround.Add(minionWave[roundCount-1].transform.GetChild(i).GetComponent<Unit>());
        }
    }

    public void SetEnemiesMinionsFirst()
    {
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            for (int j = 0; j < minionWave[0].transform.childCount; j++)
            {
                SelectManager.Instance.HerosOnField[i].GetComponent<Movement>().allEnemies.Add(minionWave[0].transform.GetChild(j).GetComponent<Unit>());
            }
        }
    }
    public void InGameToPreparatoryChangeTag()
    {
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            SelectManager.Instance.HerosOnField[i].tag = "Char";
        }
        gameStage = GameStage.preparatory;
    }
    public void PreparatoryToInGameChangeTag()
    {
        for (int i = 0; i < SelectManager.Instance.HerosOnField.Count; i++)
        {
            SelectManager.Instance.HerosOnField[i].tag = "Oyunda";
        }
        gameStage = GameStage.inGame;
    }
    public void ChangeImage()
    {
        var childCount = imageList[roundStageCount].transform.childCount;
        activeImage.enabled = false;
        activeImage = imageList[roundStageCount].transform.GetChild(roundCount-1).GetComponent<Image>();
        // activeImage = turImages[_roundCount];
        activeImage.enabled = true;
        roundText.text = roundStageCount + " / " + roundCount; 
        if(roundCount == childCount)
        {
            roundCount = 1;
            roundStageCount ++;
            activeImage = imageList[roundStageCount].transform.GetChild(roundCount-1).GetComponent<Image>();
        }
    }
}
