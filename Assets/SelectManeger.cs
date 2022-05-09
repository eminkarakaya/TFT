using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManeger : MonoBehaviour
{
    public  bool satilmaYerindeMi;
    RaycastHit hit;
    RaycastHit charHit;
    // public int playerMaskInt = 7;
    [SerializeField] private GameObject selectedObject;
    bool basildiMi;
    // public LayerMask mask;
    // int bitmask;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            basildiMi = true;
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay();
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
            // bitmask = mask = 1<<6;
            Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.parent.position = new Vector3(worldPosition.x,.25f,worldPosition.z);
            //hit = CastRay();
            selectedObject.GetComponent<Collider>().enabled = false;
            
            if(Input.GetMouseButtonUp(0))
            {   
                SellHero();
                if(selectedObject == null)
                {
                    return;
                }
                
                if(!hit.collider.CompareTag("Ground"))
                {
                    selectedObject.transform.parent.position = selectedComponent.hangiZemin.transform.position;
                    selectedObject.GetComponent<Collider>().enabled = true;
                    selectedObject = null;
                    basildiMi = false;
                    // bitmask = mask = 1<<7;
                    return;
                }
                else
                {
                    GridController hitGridController  = hit.collider.GetComponent<GridController>();
                    GameObject temp = selectedObject.GetComponent<KarakterYerlestirme>().hangiZemin;
                    if(hit.collider.GetComponent<GridController>().uzerindekiChar != null)
                    {
                        selectedComponent.hangiZemin = hit.collider.gameObject;
                        selectedObject.transform.parent.position = hit.collider.transform.position;
                        hitGridController.uzerindekiChar.GetComponent<KarakterYerlestirme>().hangiZemin = temp;
                        hitGridController.uzerindekiChar.transform.parent.position = temp.transform.position;
                        Debug.Log("yerlestirildi");
                        hitGridController.uzerindekiChar = selectedObject;
                        temp = hitGridController.uzerindekiChar;
                    }
                    else
                    {
                        Debug.Log(hit.collider.name);
                        selectedComponent.hangiZemin.GetComponent<GridController>().uzerindekiChar = null;
                        selectedObject.transform.parent.position = hit.collider.gameObject.transform.position;
                        selectedComponent.hangiZemin = hit.collider.gameObject;
                        Debug.Log("yerlestirildi");
                        hit.collider.GetComponent<GridController>().uzerindekiChar = selectedObject;
                    }
                selectedObject.GetComponent<Collider>().enabled = true;
                selectedObject = null;
                basildiMi = false;
                // bitmask = mask = 1<<7;
                }
            }
        }
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
    private RaycastHit CastRay()
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
        Physics.Raycast(worldMousePosNear,worldMousePosFar - worldMousePosNear, out hit ,Mathf.Infinity);

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
