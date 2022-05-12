using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquenceManager : MonoBehaviour
{
    public static SquenceManager instance;

  
    List<GameObject> _sequences;
    [SerializeField] GameObject playerUI;
    [SerializeField] Button nextButtom;
    [SerializeField] Button backButtom;
    [SerializeField] InputField nameInput;
    [SerializeField] Canvas ca;
    [SerializeField] Button addMatButton;
    [SerializeField] Button startCookButtom;
    [SerializeField] Text gunBombCount;
    [SerializeField] Text potionCount;
    [SerializeField] Text matCount;
    [SerializeField] GameObject[] signs;
    [SerializeField] TMPro.TMP_Text matAmount;

    [SerializeField] GameObject[] checks;
    [SerializeField] GameObject[] noFins;
    [Range(0, 2)]
    public int gunBombC;
    [Range(0, 1)]
    public int potionC;
    [Range(0, 1)]
    public int matC;

    GameObject currenSequence;
    public int sequenceIndex;
    public bool canMove;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
      
    }

    private void Start()
    {
        _sequences = Sequence.instance.sequences;
        sequenceIndex = 0;
        for (int i = 0; i < signs.Length; i++)
        {
            signs[i].SetActive(false);
        }
        currenSequence = _sequences[sequenceIndex];
        nextButtom.onClick.RemoveAllListeners();
        nextButtom.onClick.AddListener(delegate { NextSequence(sequenceIndex); });
        backButtom.onClick.RemoveAllListeners();
        backButtom.onClick.AddListener(delegate { BackSequece(sequenceIndex); });
    }
    public void NextSequence(int index)
    {
        
        if (currenSequence.activeSelf)
        {
            sequenceIndex += 1;
            currenSequence.gameObject.SetActive(false);
            _sequences[sequenceIndex].gameObject.SetActive(true);
            currenSequence = _sequences[sequenceIndex];
            Debug.LogError(sequenceIndex);
            if (sequenceIndex > 0 && sequenceIndex != 4 && sequenceIndex != 5 && sequenceIndex != 6)
            {
                backButtom.gameObject.SetActive(true);
            }
            if (sequenceIndex == 2)
            {
                SaveName();
            }
            if (sequenceIndex == 3)
            {

                nextButtom.gameObject.SetActive(false);
                backButtom.gameObject.SetActive(false);
                playerUI.SetActive(true);
                canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                StartCoroutine(WaitForNextSequence(1f));
                
            }
            if (sequenceIndex == 5)
            {
                StartCoroutine(WaitForNextSequence(2f));
            }
            if (sequenceIndex == 6)
            {
                canMove = false;
                Cursor.lockState = CursorLockMode.None;
                nextButtom.gameObject.SetActive(true);
                MoveButtom(nextButtom, new Vector3(530f, -490f, 0));
                
            }
            if (sequenceIndex == 7)
            {
                MoveButtom(backButtom, new Vector3(310, -490, 0));
            }
            if (sequenceIndex == 8)
            {
                nextButtom.gameObject.SetActive(false);
                backButtom.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                canMove = true;
            }
            if (sequenceIndex == 9)
            {
                backButtom.gameObject.SetActive(false);
               
                StartCoroutine(WaitForNextSequence(1f));
                
                

            }
            if (sequenceIndex == 10)
            {
                nextButtom.gameObject.SetActive(true);
                backButtom.gameObject.SetActive(false);
                MoveButtom(nextButtom, new Vector3(850, 50, 0));
                
                InventoryManager.instance.AddItemToInventoryInCurrentSlot(ItemManager.instance.GetMaterialById(98), 1, CookUIForTur.instance.ingredients[0]);
                InventoryManager.instance.AddItemToInventoryInCurrentSlot(ItemManager.instance.GetMaterialById(99), 1, CookUIForTur.instance.ingredients[1]);
            }
            if (sequenceIndex == 11)
            {
                nextButtom.gameObject.SetActive(false);
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(2f));

            }
            if (sequenceIndex == 12)
            {
                backButtom.gameObject.SetActive(false);
                ca.sortingOrder = -1;
                addMatButton.onClick.RemoveAllListeners();
                addMatButton.onClick.AddListener(delegate { AddMaterial(); });
                
            }
            if (sequenceIndex == 13)
            {
                ca.sortingOrder = 1;
                backButtom.gameObject.SetActive(false);
                matAmount.text = "4/5";
                StartCoroutine(WaitForNextSequence(2f));
            }
            if (sequenceIndex == 14)
            {
                backButtom.gameObject.SetActive(false);
                ca.sortingOrder = -1;
                startCookButtom.onClick.RemoveAllListeners();
                startCookButtom.onClick.AddListener(delegate { StartCook(); });
                
            }
            if (sequenceIndex == 15)
            {
                backButtom.gameObject.SetActive(false);
                CookUIForTur.instance.gameObject.SetActive(false);
            }
            if (sequenceIndex == 16)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 17)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 18)
            {
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(2f));

                
            }
            if (sequenceIndex == 19)
            {
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(true);
                canMove = false;
                Cursor.lockState = CursorLockMode.None;


                
            }
            if (sequenceIndex == 20)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 21)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 22)
            {
                backButtom.gameObject.SetActive(false);
                ca.sortingOrder = 1;
                MoveButtom(nextButtom, new Vector3(700, -360, 0));
            }
            if (sequenceIndex == 23)
            {
                MoveButtom(nextButtom, new Vector3(700, -360, 0));
                MoveButtom(backButtom, new Vector3(450, -360, 0));
            }
            if (sequenceIndex == 24)
            {
               MoveButtom(nextButtom, new Vector3(120, -450, 0));
               MoveButtom(backButtom, new Vector3(-120, -450, 0));
            }
            if (sequenceIndex == 25)
            {
                nextButtom.gameObject.SetActive(false);
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 26)
            {
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(2f));
            }
            if (sequenceIndex == 27)
            {
                backButtom.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                canMove = true;
            }
            if (sequenceIndex == 28)
            {
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                canMove = false;
                MoveButtom(nextButtom, new Vector3(850, 50, 0));
            }
            if (sequenceIndex == 29)
            {
                backButtom.gameObject.SetActive(false);
                MoveButtom(nextButtom, new Vector3(530f, -490f, 0));
            }
            if (sequenceIndex == 30)
            {
                MoveButtom(backButtom, new Vector3(350, -490, 0));
            }
            if (sequenceIndex == 31)
            {
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(true);
                MoveButtom(nextButtom, new Vector3(850, 50, 0));

            }
            if (sequenceIndex == 32)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 33)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 34)
            {
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(false);
                for (int i = 0; i < signs.Length; i++)
                {
                    signs[i].SetActive(true);
                }
                canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (sequenceIndex == 35)
            {
                for (int i = 0; i < signs.Length; i++)
                {
                    signs[i].SetActive(false);
                }
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(true);
                MoveButtom(nextButtom, new Vector3(850, 50, 0));
                Cursor.lockState = CursorLockMode.None;
                canMove = false;
            }
            if (sequenceIndex == 36)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 37)
            {
                backButtom.gameObject.SetActive(false);
                MoveButtom(nextButtom, new Vector3(0, -450, 0));
                
            }
            if (sequenceIndex == 38)
            {
                MoveButtom(nextButtom, new Vector3(100, -450, 0));
                MoveButtom(backButtom, new Vector3(-120, -450, 0));
            }
            if (sequenceIndex == 39)
            {
                backButtom.gameObject.SetActive(false);
                nextButtom.gameObject.SetActive(false);
                canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (sequenceIndex == 40)
            {
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(1f));
            }
            if (sequenceIndex == 41)
            {
                backButtom.gameObject.SetActive(false);
                
            }
            if (sequenceIndex == 42)
            {
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(3f));
            }
            if (sequenceIndex == 43)
            {
                backButtom.gameObject.SetActive(false);
            }
            if (sequenceIndex == 44)
            {
                backButtom.gameObject.SetActive(false);
                HotBarForTur.instance.WeaponUse();
                StartCoroutine(WaitForNextSequence(1f));
            }
            if (sequenceIndex == 45)
            {
                backButtom.gameObject.SetActive(false);
                StartCoroutine(WaitForNextSequence(2f));
            }
            if (sequenceIndex == 46)
            {
                backButtom.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                canMove = false;
            }
          

            



        }
       
        
    }

    public void GunBombHit()
    {
        if(gunBombC < 3)
        {
            gunBombC++;
            gunBombCount.text = string.Format("({0}/2)", gunBombC);
        }
        if (gunBombC > 2)
        {
            gunBombC--;
            gunBombCount.text = string.Format("({0}/2)", gunBombC);
        }
        if (gunBombC == 2)
        {
            noFins[0].gameObject.SetActive(false);
            checks[0].gameObject.SetActive(true);
        }
    }
    public void PotionHit()
    {
        if (potionC < 2)
        {
            potionC++;
            potionCount.text = string.Format("({0}/1)", potionC);
        }
        if (potionC > 1)
        {
            potionC--;
            potionCount.text = string.Format("({0}/1)", potionC);
        }
        if (potionC == 1)
        {
            noFins[1].gameObject.SetActive(false);
            checks[1].gameObject.SetActive(true);
        }
    }
    public void GetBox()
    {
        if (matC < 2)
        {
            matC++;
            matCount.text = string.Format("({0}/1)", matC);
        }
        if (potionC > 1)
        {
            matC--;
            matCount.text = string.Format("({0}/1)", matC);
        }
        if (matC == 1)
        {
            noFins[2].gameObject.SetActive(false);
            checks[2].gameObject.SetActive(true);
        }
    }
    bool isDone;
    private void CheckThree()
    {
        if (matC + gunBombC + potionC >= 4 && !isDone)
        {
            NextSequence(sequenceIndex);
            isDone = true;
            
        }
    }



    private void StartCook()
    {
        NextSequence(sequenceIndex);
        TurCooker.instance.progressBar.gameObject.SetActive(true);
        StartCoroutine(WaitForNextSequence(5f));
        StartCoroutine(CookerTimer());
    }
    private IEnumerator CookerTimer()
    {
        float startTime = Time.time;
        while (Time.time < startTime + 10)
        {
            TurCooker.instance.bar.fillAmount = Mathf.Abs((((startTime + 10) - Time.time) / 10) - 1);
            yield return null;
        }
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        TurCooker.instance.progressBar.gameObject.SetActive(false);
        TurCooker.instance.resultDish.SetActive(true);
        NextSequence(sequenceIndex);
        
        
    }
    private void AddMaterial()
    {
        InventoryManager.instance.AddItemToInventoryInCurrentSlot(ItemManager.instance.GetMaterialById(97), 1, CookUIForTur.instance.ingredients[2]);
        startCookButtom.interactable = true;
        NextSequence(sequenceIndex);
    }
    private void BackSequece(int index)
    {
        if (currenSequence.activeSelf)
        {
            sequenceIndex -= 1;
            currenSequence.SetActive(false);
            _sequences[sequenceIndex].gameObject.SetActive(true);
            currenSequence = _sequences[sequenceIndex];
        }
        if (sequenceIndex > 0)
        {
            backButtom.gameObject.SetActive(true);
        }
        else
        {
            backButtom.gameObject.SetActive(false);
        }
        if (sequenceIndex == 6)
        {
            backButtom.gameObject.SetActive(false);
        }
        if (sequenceIndex == 22)
        {
            backButtom.gameObject.SetActive(false);
            MoveButtom(nextButtom, new Vector3(700, -360, 0));
        }
        if (sequenceIndex == 23)
        {
            MoveButtom(nextButtom, new Vector3(700, -360, 0));
            MoveButtom(backButtom, new Vector3(450, -360, 0));
        }
        if (sequenceIndex == 29)
        {
            backButtom.gameObject.SetActive(false);
        }
        if (sequenceIndex == 37)
        {
            backButtom.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        Sequence1();
        CheckThree();
    }
    private void SaveName()
    {
        if (nameInput.text != "" && sequenceIndex == 2)
        {
            Debug.LogError("Save!");
            SaveManager.instance.nowData.playerName = nameInput.text;
            SaveManager.instance.SaveGame();
        }
    }

   

    private IEnumerator WaitForNextSequence(float time)
    {
       
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            yield return null;
        }
       
            NextSequence(sequenceIndex);
       
      








    }
    
     
    private void Sequence1()
    {
        if (_sequences[1].activeSelf && nameInput.text != "")
        {
            nextButtom.interactable = true;

        }
        else if (_sequences[1].activeSelf)
        {
            nextButtom.interactable = false;
        }
        else
        {
            nextButtom.interactable = true;
        }
    }
    private void MoveButtom(Button targetButtom, Vector3 targetPos)
    {
        targetButtom.transform.localPosition = targetPos;
    }

    public void LeaveGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}
