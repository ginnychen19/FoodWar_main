using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurCooker : MonoBehaviour
{
    public static TurCooker instance;


    [SerializeField] GameObject sight;
    [SerializeField] GameObject pushE;
    public GameObject resultDish;
    public Image bar;
    public Canvas progressBar;
    public bool canPick;
    
    [SerializeField] CookUIForTur cookUIForTur;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        sight.SetActive(false);
        pushE.SetActive(false);
        resultDish.SetActive(false);
        progressBar.gameObject.SetActive(false);
        cookUIForTur.gameObject.SetActive(false);
      
    }
    private void Update()
    {
        if (SquenceManager.instance.sequenceIndex == 4 && !sight.activeSelf)
        {
            sight.SetActive(true);
        }
        if (SquenceManager.instance.sequenceIndex == 8)
        {
            OpenCooker();
           
               
            
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SquenceManager.instance.sequenceIndex == 4)
        {
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
            sight.SetActive(false);
        }
        if (other.CompareTag("Player") && SquenceManager.instance.sequenceIndex == 17)
        {
            canPick = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && SquenceManager.instance.sequenceIndex == 17)
        {
            canPick = false;
        }
    }

    private void FixedUpdate()
    {
        if (SquenceManager.instance.sequenceIndex == 8)
        {
            PlayerNear();
        }
    }
    private void OpenCooker()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen && SquenceManager.instance.sequenceIndex == 8)
        {

            cookUIForTur.gameObject.SetActive(true);
            cookUIForTur.SetCookerUIBillboard(this.gameObject.transform.position);

            SquenceManager.instance.canMove = false;
            SquenceManager.instance.NextSequence(SquenceManager.instance.sequenceIndex);
            pushE.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
       
    }
    bool canOpen;
    private void PlayerNear()
    {
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 8f);
        for (int i = 0; i < cols.Length; i++)
        {
           
            if (cols[i].gameObject.CompareTag("Player"))
            {
                pushE.SetActive(true);
                canOpen = true;
               
            }
            else
            {
                pushE.SetActive(false);
                canOpen = false;
                
               
            }
        }
    }
}
