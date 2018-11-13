///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/4/29
//用途：状态机状态转换基类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

namespace fsm
{
    /// <summary>
    /// 状态转换基类 by吴江
    /// </summary>
    [System.Serializable]
    public class Transition
    {
        public delegate bool TestEventHandler(Transition _trans, Event _event);
        public delegate void TransitionHandler(Transition _trans, Event _event);

        /// <summary>
        /// 跳转之前的状态 by吴江
        /// </summary>
        public State source = null;
        /// <summary>
        /// 跳转之后的状态 by吴江
        /// </summary>
        public State target = null;
        public Machine machine
        {
            get
            {
                if (source != null)
                    return source.machine;
                return null;
            }
        }

        public TestEventHandler onTestEvent;
        public TransitionHandler onTransition;

        public virtual bool TestEvent(Event _event)
        {
            if (onTestEvent != null)
                return onTestEvent(this, _event);
            return true;
        }

        public virtual void OnTransition(Event _event)
        {
            if (onTransition != null)
                onTransition(this, _event);
        }
    }



    /// <summary>
    /// 状态机状态转换基类 by吴江
    /// </summary>
    [System.Serializable]
    public class EventTransition : Transition
    {
        public int eventID = -1;
        public EventTransition()
        {
        }

        public EventTransition(int _eventID)
        {
            eventID = _eventID;
        }

        public override bool TestEvent(Event _event)
        {
            return _event.id == eventID;
        }
    }
}



