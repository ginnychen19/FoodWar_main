using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float speed = 60f;
    [SerializeField] ParticleSystem HitFX;
    private void Start()
    {
        Invoke("DetroySelf", 10f);
    }
    private void Update()
    {
        if (canMove)
            Move(Time.deltaTime);
    }
    int nowPoint = 0;
    void Move(float times)
    {
        // �p��o�@�V���ʶZ��
        float mspeed = times * speed;

        while (mspeed > 0f)
        {
            // �p�G�S���U�@���I�N����
            if (nowPoint < 0)
                return;
            // ���ˬd���U�@���I�h��
            float d = Vector3.Distance(this.transform.position, psss[nowPoint]);
            // �@�V���ʪ��Z�����i�j���U���I���Z�� �_�h�|���ʹL�Y
            float dmove = mspeed;
            // �o�@�V�i�H��F�o���I �ҥH�U������n���U�@���I����
            bool needAdd = false;
            if (dmove > d)
            {
                dmove = d;
                needAdd = true;
            }
            this.transform.Translate((psss[nowPoint] - this.transform.position).normalized * dmove, Space.World);
            // ���Ӳ��ʶZ��
            mspeed -= dmove;
            if (needAdd)
                nowPoint--;
        }
    }
    Collider[] cols;
    private void DetroySelf()
    {

      
       
        Instantiate(impactParticle, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    List<Vector3> psss = new List<Vector3>();
    int shooterId = 0;
    bool canMove = false;
    public void LagMove(double lagTime, List<Vector3> poss, int shooterId)
    {
        this.shooterId = shooterId;
        this.psss = poss;
        nowPoint = psss.Count-1;
        //Debug.LogError(psss.Count);
        canMove = true;
        StartCoroutine(ILagMove(lagTime));
    }
    IEnumerator ILagMove(double lagTime)
    {
        yield return null;
        double time = lagTime / (double)10;
        for (int i = 0; i < 10; i++)
        {
            Move((float)time);
        }
    }
    [SerializeField] GameObject impactParticle = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetInstanceID() == shooterId)
            return;
        
        PlayerController pc = other.transform.root.GetComponent<PlayerController>();
        if (pc != null)
        {
            //TODO: fly away
            

        }

        DetroySelf();
    }
    bool isHit;
    private void OnDestroy()
    {
        cols = Physics.OverlapSphere(this.transform.position, 8f);
        isHit = false;
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].transform.root.gameObject.CompareTag("Player") && !isHit)
            {
                cols[i].transform.root.GetComponent<PlayerController>().ApplyStun();
                Instantiate(HitFX, this.gameObject.transform.position, Quaternion.identity);
                isHit = true;
            }
        }
    }


}
