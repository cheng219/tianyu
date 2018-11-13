///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/4/29
//用途：状态机事件基类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;




namespace fsm
{
    public class Event : System.EventArgs
    {
        static public readonly int UNKNOWN = -1;
        static public readonly int NULL = 0;
        static public readonly int NEXT = 1;
        static public readonly int TRIGGER = 2;
        static public readonly int FINISHED = 3;
        public const int USER_FIELD = 1000;
        // properties
        public int id = UNKNOWN;

        // functions
        public Event(int _id = -1)
        {
            id = _id;
        }
    }
}







