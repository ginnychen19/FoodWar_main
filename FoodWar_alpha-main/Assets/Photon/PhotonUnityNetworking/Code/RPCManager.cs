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
    
    // 添加一個RPC 如果重複就堆疊 不重複就添加列表元素
    public void AddRPC(string rpcName)
    {
        bool isNoRPC = false;
        // 掃描目前的列表
        for (int i = 0; i < RPCs.Count; i++)
        {
            // 可口可樂
            // 可
            // 口
            // 可
            // 樂
            // 如果遇到列表中有的東西
            if (RPCs[i].rpcName == rpcName)
            {
                // 修改該物件的times
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
       
        
        // 如果掃完迴圈都沒遇到相同的
        // 直接新增一個並且設為數量1
    }
}
public struct RPCStuff
{
    public string rpcName;
    public int times;
}
