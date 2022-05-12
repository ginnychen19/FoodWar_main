using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RPCManager : MonoBehaviour
{
    static RPCManager mInstance;

    public static RPCManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = (new GameObject("RPCManager")).AddComponent<RPCManager>();
            return mInstance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    float startTime;
    public int rpcTimes = 0;
    private void Update()
    {

        if (Time.time - startTime > 1f)
        {
            startTime = Time.time;
            string say = ""+rpcTimes+"\n";
            for (int i = 0; i < RPCs.Count; i++)
            {
                say += RPCs[i].times + " " + RPCs[i].rpcName + "\n";
                
            }
            Debug.Log(say);
            RPCs.Clear();
            rpcTimes = 0;
        }
    }
    public List<RPCStuff> RPCs = new List<RPCStuff>(); 
    
    // �K�[�@��RPC �p�G���ƴN���| �����ƴN�K�[�C����
    public void AddRPC(string rpcName)
    {
        bool isNoRPC = false;
        // ���y�ثe���C��
        for (int i = 0; i < RPCs.Count; i++)
        {
            // �i�f�i��
            // �i
            // �f
            // �i
            // ��
            // �p�G�J��C�������F��
            if (RPCs[i].rpcName == rpcName)
            {
                // �ק�Ӫ���times
                RPCStuff temp = RPCs[i];
                temp.times++;
                RPCs[i] = temp;
                isNoRPC = true;
                break;
            }
        }
        
        if (isNoRPC == false)
        {
            RPCStuff rpc = new RPCStuff();
            rpc.rpcName = rpcName;
            rpc.times = 1;
            RPCs.Add(rpc);
        }
       
        
        // �p�G�����j�鳣�S�J��ۦP��
        // �����s�W�@�ӨåB�]���ƶq1
    }
}
public struct RPCStuff
{
    public string rpcName;
    public int times;
}
