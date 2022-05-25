using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : Singleton<SelectManager>
{
    public List<Unit> HerosOnField;
    public TextMesh sahadakiOyuncuSayisiText;
    public List<GridController> sahaIciGridler;
    public  bool isOnSalePlace;
    RaycastHit hit;
    [SerializeField] private GameObject selectedObject;
    bool isPressed;
    public LayerMask mask;
     
    private void Update()
    {
        Select();
    }
    public void Select()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isPressed = true;
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
        if(selectedObject != null && isPressed)
        {
            hit = CastRayChar();
            HeroPlacement selectedComponent = selectedObject.GetComponent<HeroPlacement>();
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
                    HerosInsertionCancel(selectedComponent);
                }
                else
                {
                    GridController hitGridController  = hit.collider.GetComponent<GridController>();

                    // uzerınde car olan grıde car koyarken
                    if(hit.collider.GetComponent<GridController>().heroOnGround != null)
                    {
                       DoluYereHeroKoy(selectedComponent,hitGridController);
                    }
                    else
                    {
                        if(GameManager.Instance.gameStage == GameStage.inGame && !hitGridController.isBackup)
                        {
                            selectedObject.transform.parent.position = selectedComponent.whichFloor.transform.position;
                            selectedObject = null;
                            isPressed = false;
                            return;
                        }
                        //sahaya adam koydugumuz yer
                        SahaIcıBosYereHeroKoy(selectedComponent);
                    }
                selectedObject = null;
                isPressed = false;
                }
            }
        }
    }
    public void HerosInsertionCancel(HeroPlacement selectedComponent)
    {
        selectedObject.transform.parent.position = selectedComponent.whichFloor.transform.position;
        selectedObject = null;
        isPressed = false;
        return;
    }
    public void DoluYereHeroKoy(HeroPlacement selectedComponent , GridController hitGridController)
    {
        GameObject tempFloor = hit.collider.gameObject;
        GameObject tempHero = tempFloor.GetComponent<GridController>().heroOnGround;

        selectedObject.transform.parent.position = hitGridController.heroOnGround.GetComponent<HeroPlacement>().whichFloor.transform.position;
        hitGridController.heroOnGround.transform.parent.position = selectedComponent.whichFloor.transform.position;

        hitGridController.heroOnGround.GetComponent<HeroPlacement>().whichFloor = selectedComponent.whichFloor;
        hitGridController.heroOnGround = selectedObject;


        selectedComponent.whichFloor.GetComponent<GridController>().heroOnGround = tempHero; 
        selectedComponent.whichFloor = tempFloor;



    
        GetCountOnField();
    }
    public void SahaIcıBosYereHeroKoy(HeroPlacement selectedComponent)
    {
        if(!hit.collider.GetComponent<GridController>().isBackup)
        {
            if(GetCountOnField() == HeroPanel.Instance.level && selectedComponent.whichFloor.GetComponent<GridController>().isBackup)
            {
                selectedObject.transform.parent.position = selectedComponent.whichFloor.transform.position;
                selectedObject = null;
                GetCountOnField();
                return;
            }
        }
        selectedComponent.whichFloor.GetComponent<GridController>().heroOnGround = null;
        selectedComponent.whichFloor = hit.collider.gameObject;
        selectedObject.transform.parent.position = hit.collider.gameObject.transform.position;
        hit.collider.GetComponent<GridController>().heroOnGround = selectedObject;
        GetCountOnField();
    }
    public void True()
    {
        isOnSalePlace = true;
    }
    public void False()
    {
        isOnSalePlace = false;
    }
    public void SellHero()
    {
        if(selectedObject != null && isOnSalePlace)
        {
            Debug.Log("satıldı");
            selectedObject.GetComponent<HeroPlacement>().whichFloor.GetComponent<GridController>().heroOnGround = null;
            selectedObject.GetComponent<HeroPlacement>().whichFloor = null;
            HeroPanel.Instance.gold += selectedObject.GetComponent<Hero>().heroData.cost;
            GameManager.Instance.heroPool.Add(selectedObject.GetComponent<Hero>());
            selectedObject.SetActive(false);
            selectedObject = null;
        }
    }
    public int GetCountOnField()
    {
        var herosOnFieldTemp = 0;
        List<Unit> unitsOnField = new List<Unit>();
        for (int i = 0; i < sahaIciGridler.Count; i++)
        {
            if(sahaIciGridler[i].heroOnGround != null)
            {
                // if(!sahadakiHerolar.Contains(sahaIciGridler[i].heroOnGround.GetComponent<Unit>()))
                // {
                    unitsOnField.Add(sahaIciGridler[i].heroOnGround.GetComponent<Unit>());
                // }
                herosOnFieldTemp++;
            }
        }
        HerosOnField = unitsOnField;
        sahadakiOyuncuSayisiText.text = herosOnFieldTemp.ToString()+ "/" + HeroPanel.Instance.level;
        return herosOnFieldTemp;
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
