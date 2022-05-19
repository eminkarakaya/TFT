using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManeger : Singleton<SelectManeger>
{
    public List<Unit> sahadakiHerolar;
    public TextMesh sahadakiOyuncuSayisiText;
    public List<GridController> sahaIciGridler;
    bool sahaFull;
    public  bool satilmaYerindeMi;
    RaycastHit hit;
    RaycastHit charHit;
    // public int playerMaskInt = 7;
    [SerializeField] private GameObject selectedObject;
    bool basildiMi;
    public LayerMask mask;
    // int bitmask;
     
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            basildiMi = true;
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay(mask);
                if(hit.collider != null)
                {
                    
                    if(!hit.collider.CompareTag("Char"))
                    {
                        return;
                    }
                    selectedObject = hit.collider.gameObject;
                }
            }
        }
        if(selectedObject != null && basildiMi)
        {
            
            hit = CastRayChar();
            KarakterYerlestirme selectedComponent = selectedObject.GetComponent<KarakterYerlestirme>();
            Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.parent.position = new Vector3(worldPosition.x,.25f,worldPosition.z);
            
            if(Input.GetMouseButtonUp(0))
            {   
                SellHero();
                if(selectedObject == null)
                {
                    return;
                }
                
                if(!hit.collider.CompareTag("Ground"))
                {
                    HeroKoymaIptali(selectedComponent);
                }
                else
                {
                    GridController hitGridController  = hit.collider.GetComponent<GridController>();
                    GameObject temp = selectedObject.GetComponent<KarakterYerlestirme>().hangiZemin;

                    // uzerınde car olan grıde car koyarken
                    if(hit.collider.GetComponent<GridController>().uzerindekiChar != null)
                    {
                       DoluYereHeroKoy(selectedComponent,hitGridController,temp);
                    }
                    else
                    {
                        if(GameController.Instance.gameStage == GameStage.inGame)
                        {
                            selectedObject.transform.parent.position = selectedComponent.hangiZemin.transform.position;
                            selectedObject = null;
                            basildiMi = false;
                            return;
                        }
                        //sahaya adam koydugumuz yer
                        SahaIcıBosYereHeroKoy(selectedComponent);
                    }
                selectedObject = null;
                basildiMi = false;
                }
            }
        }
    }
    public void HeroKoymaIptali(KarakterYerlestirme selectedComponent)
    {
        selectedObject.transform.parent.position = selectedComponent.hangiZemin.transform.position;
        selectedObject = null;
        basildiMi = false;
        return;
    }
    public void DoluYereHeroKoy(KarakterYerlestirme selectedComponent , GridController hitGridController, GameObject temp)
    {
        selectedComponent.hangiZemin = hit.collider.gameObject;
        selectedObject.transform.parent.position = hit.collider.transform.position;
        hitGridController.uzerindekiChar.GetComponent<KarakterYerlestirme>().hangiZemin = temp;
        hitGridController.uzerindekiChar.transform.parent.position = temp.transform.position;
        hitGridController.uzerindekiChar = selectedObject;
        temp = hitGridController.uzerindekiChar;
        GetSahaIciOyuncuSayisi();
    }
    public void SahaIcıBosYereHeroKoy(KarakterYerlestirme selectedComponent)
    {
        if(!hit.collider.GetComponent<GridController>().yedekMi)
        {
            if(GetSahaIciOyuncuSayisi() == HeroPanel.Instance.level && selectedComponent.hangiZemin.GetComponent<GridController>().yedekMi)
            {
                selectedObject.transform.parent.position = selectedComponent.hangiZemin.transform.position;
                selectedObject = null;
                GetSahaIciOyuncuSayisi();
                return;
            }
        }
        selectedComponent.hangiZemin.GetComponent<GridController>().uzerindekiChar = null;
        selectedComponent.hangiZemin = hit.collider.gameObject;
        selectedObject.transform.parent.position = hit.collider.gameObject.transform.position;
        hit.collider.GetComponent<GridController>().uzerindekiChar = selectedObject;
        GetSahaIciOyuncuSayisi();
    }
    public void True()
    {
        satilmaYerindeMi = true;
    }
    public void False()
    {
        satilmaYerindeMi = false;
    }
    public void SellHero()
    {
        if(selectedObject != null && satilmaYerindeMi)
        {
            Debug.Log("satıldı");
            selectedObject.GetComponent<KarakterYerlestirme>().hangiZemin.GetComponent<GridController>().uzerindekiChar = null;
            selectedObject.GetComponent<KarakterYerlestirme>().hangiZemin = null;
            HeroPanel.Instance.gold += selectedObject.GetComponent<Hero>().heroData.cost;
            GameController.Instance.heroPool.Add(selectedObject.GetComponent<Hero>());
            selectedObject.SetActive(false);
            selectedObject = null;
        }
    }
    public int GetSahaIciOyuncuSayisi()
    {
        var sahadakiOyuncuSayisi = 0;
        List<Unit> sahadakiUnits = new List<Unit>();
        for (int i = 0; i < sahaIciGridler.Count; i++)
        {
            if(sahaIciGridler[i].uzerindekiChar != null)
            {
                if(!sahadakiHerolar.Contains(sahaIciGridler[i].uzerindekiChar.GetComponent<Unit>()))
                {
                    sahadakiUnits.Add(sahaIciGridler[i].uzerindekiChar.GetComponent<Unit>());
                }
                sahadakiOyuncuSayisi++;
            }
        }
        sahadakiHerolar = sahadakiUnits;
        sahadakiOyuncuSayisiText.text = sahadakiOyuncuSayisi.ToString()+ "/" + HeroPanel.Instance.level;
        return sahadakiOyuncuSayisi;
    }
    private RaycastHit CastRay(LayerMask layerMask)
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear,worldMousePosFar - worldMousePosNear, out hit ,Mathf.Infinity, layerMask);

        return hit;
    }
    private RaycastHit CastRayChar()
    {
        Vector3 charPos = selectedObject.transform.parent.position;
        Vector3 dir = Vector3.down;
        RaycastHit hit;
        Physics.Raycast(charPos,dir, out hit ,Mathf.Infinity);

        return hit;
    }
}
