using System;


public class EventManager
{
    static EventManager _instance = null;
    public static EventManager instance
    {
        get
        {
            // ���H�ݭn�ڪ��ɭ� �]���ڹ����Ƥ@�˨S������ �N�ۧڳгy�b��Ť�
            if (_instance == null)
                _instance = new EventManager();
            return _instance;
        }
    }

    public Action<int> AddScore;
    public Action GameOver;
  
   

}
