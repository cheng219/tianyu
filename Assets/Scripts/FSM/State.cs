///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/4/29
//用途：状态机基类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace fsm
{
    /// <summary>
    /// 状态机基类 by吴江
    /// </summary>
    [System.Serializable]
    public class State
    {
        public enum Mode
        {
            Exclusive,
            Parallel,
        }
        public delegate void TransitionEventHandler(State _from, State _to, Event _event);
        public delegate void OnEventHandler(State _curState, Event _event);
        public delegate void OnActionHandler(State _curState);

        /// <summary>
        /// 状态名
        /// </summary>
        public string name = "";
        /// <summary>
        /// 父状态（机）
        /// </summary>
        protected State parent_ = null;
        /// <summary>
        /// 父状态（机）
        /// </summary>
        public State parent
        {
            set
            {
                if (parent_ != value)
                {
                    State oldParent = parent_;
                    // check if it is parent layer or child
                    while (parent_ != null)
                    {
                        if (parent_ == this)
                        {
                            Debug.LogWarning("can't add self or child as parent");
                            return;
                        }
                        parent_ = parent_.parent;
                    }
                    if (oldParent != null)
                    {

                        if (oldParent.initState == this)

                            oldParent.initState = null;

                        oldParent.children.Remove(this);

                    }
                    if (value != null)
                    {
                        value.children.Add(this);
                        // if this is first child 
                        if (value.children.Count == 1)
                            value.initState = this;
                    }
                    parent_ = value;
                }
            }
            get { return parent_; }
        }

        /// <summary>
        /// 所属状态机 by吴江
        /// </summary>
        protected Machine machine_ = null;
        /// <summary>
        /// 所属状态机 by吴江
        /// </summary>
        public Machine machine
        {
            get
            {
                if (machine_ != null)
                    return machine_;
                State last = this;
                State root = parent;
                while (root != null)
                {
                    last = root;
                    root = root.parent;
                }
                machine_ = last as Machine; // 有可能为空
                return machine_;
            }
        }

        /// <summary>
        /// 启动状态，只在状态机类型为State.Exclusive的情况下才生效 by吴江
        /// </summary>
        protected State initState_ = null;
        /// <summary>
        /// 启动状态，只在状态机类型为State.Exclusive的情况下才生效 by吴江
        /// </summary>
        public State initState
        {
            get { return initState_; }
            set
            {
                if (initState_ != value)
                {
                    if (value != null && children.IndexOf(value) == -1)
                    {
                        Debug.LogError("FSM error: You must use child state as initial state.");
                        initState_ = null;
                        return;
                    }
                    initState_ = value;
                }
            }
        }

        /// <summary>
        /// 状态机类型。 Exclusive为具备初始状态的状态机，状态机开启，自动进入initState
        /// Parallel则不会在启动时进入某一个状态循环，而是遍历每个状态后跳出。（一般用于预加载）
        /// 两者都支持外部手动指定进入某一个状态 by吴江
        /// </summary>
        public Mode mode = Mode.Exclusive;
        public List<Transition> transitionList = new List<Transition>();
        /// <summary>
        /// 当前状态列表 by吴江
        /// </summary>
        protected List<State> currentStates = new List<State>();
        /// <summary>
        /// 子状态列表 by吴江
        /// </summary>
        protected List<State> children = new List<State>();



        /// <summary>
        /// 进入时的事件 by吴江
        /// </summary>
        public TransitionEventHandler onEnter;
        /// <summary>
        /// 退出时的事件 by吴江
        /// </summary>
        public TransitionEventHandler onExit;
        /// <summary>
        /// 状态切换时的事件，仅当本身为一个状态机时 by吴江
        /// </summary>
        public OnEventHandler onEvent;
        /// <summary>
        /// 在状态机Update()时候调用的更新事件 by吴江
        /// </summary>
        public OnActionHandler onAction;



        // public System.EventHandler<Event> onEvent;  // void EventHandler ( object _sender, Event _event )
        // public System.EventHandler onAction;        // void ActionHandler ( object _sender )

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_parent"></param>
        public State(string _name, State _parent = null)
        {
            name = _name;
            parent = _parent;
        }

        public State()
        {
        }

        public void ClearCurrentStatesRecursively()
        {
            currentStates.Clear();
            for (int i = 0; i < children.Count; ++i)
            {
                children[i].ClearCurrentStatesRecursively();
            }
        }

        /// <summary>
        /// 添加一个跳转关系 by吴江
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_targetState"></param>
        /// <returns></returns>
        public T Add<T>(State _targetState) where T : Transition, new()
        {
            T newTranstion = new T();
            newTranstion.source = this;
            newTranstion.target = _targetState;
            transitionList.Add(newTranstion);
            return newTranstion;
        }



        /// <summary>
        /// 添加一个跳转关系 by吴江
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_targetState"></param>
        /// <returns></returns>
        public T Add<T>(State _targetState, int _id) where T : EventTransition, new()
        {
            T newTranstion = Add<T>(_targetState);
            newTranstion.eventID = _id;
            return newTranstion;
        }

        /// <summary>
        /// 更新  by吴江
        /// </summary>
        public void OnAction()
        {
            if (onAction != null)
            {
                onAction(this);
            }
            for (int i = 0; i < currentStates.Count; ++i)
            {
                currentStates[i].OnAction();

            }
        }

        /// <summary>
        /// 状态跳转 by吴江
        /// </summary>
        /// <param name="_event"></param>
        public void OnEvent(Event _event)
        {
            if (onEvent != null)
            {
                onEvent(this, _event);
            }
            for (int i = 0; i < currentStates.Count; ++i)
            {
                currentStates[i].OnEvent(_event);
            }
        }

        /// <summary>
        /// 检测冲突跳转，获取出当前所有同样需要跳转的子状态 by吴江
        /// </summary>
        /// <param name="_validTransitions"></param>
        /// <param name="_event"></param>
        public void TestTransitions(ref List<Transition> _validTransitions, Event _event)
        {
            for (int i = 0; i < currentStates.Count; ++i)
            {
                State activeChild = currentStates[i];
                // NOTE: if parent transition triggerred, the child should always execute onExit transition
                bool hasTranstion = false;
                for (int j = 0; j < activeChild.transitionList.Count; ++j)
                {
                    Transition transition = activeChild.transitionList[j];
                    if (transition.TestEvent(_event))
                    {
                        _validTransitions.Add(transition);
                        hasTranstion = true;
                        break;
                    }
                }
                if (!hasTranstion)
                {
                    activeChild.TestTransitions(ref _validTransitions, _event);
                }
            }
        }

        /// <summary>
        /// 进入某一个状态（机），并根据该状态（机）的模式进行相应的处理 by吴江
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_toEnter"></param>
        /// <param name="_toExit"></param>
        public void EnterStates(Event _event, State _toEnter, State _toExit)
        {
            currentStates.Add(_toEnter);
            if (machine != null && machine.logDebugInfo)
                Debug.Log("FSM Debug: Enter State - " + _toEnter.name + " at " + Time.time);
            _toEnter.OnEnter(_toExit, _toEnter, _event);

            if (_toEnter.children.Count != 0)
            {
                if (_toEnter.mode == State.Mode.Exclusive)//如果为指定初始化模式
                {
                    if (_toEnter.initState != null)
                    {
                        _toEnter.EnterStates(_event, _toEnter.initState, _toExit);
                    }
                    else
                    {
                        Debug.LogError("FSM error: can't find initial state in " + _toEnter.name);
                    }
                }
                else
                { // if ( _toEnter.mode == State.Mode.Parallel )  如果为遍历模式
                    for (int i = 0; i < _toEnter.children.Count; ++i)
                    {
                        _toEnter.EnterStates(_event, _toEnter.children[i], _toExit);
                    }
                }
            }
        }


        /// <summary>
        /// 退出某一个状态（机） by吴江
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_toEnter"></param>
        /// <param name="_toExit"></param>
        public void ExitStates(Event _event, State _toEnter, State _toExit)
        {
            _toExit.ExitAllStates(_event, _toEnter);
            if (machine != null && machine.logDebugInfo)
                Debug.Log("FSM Debug: Exit State - " + _toExit.name + " at " + Time.time);
            _toExit.OnExit(_toExit, _toEnter, _event);
            currentStates.Remove(_toExit);
        }



        /// <summary>
        /// 退出所有状态（机） by吴江
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_toEnter"></param>
        /// <param name="_toExit"></param>
        protected void ExitAllStates(Event _event, State _toEnter)
        {
            for (int i = 0; i < currentStates.Count; ++i)
            {
                State activeChild = currentStates[i];
                activeChild.ExitAllStates(_event, _toEnter);
                activeChild.OnExit(activeChild, _toEnter, _event);
                if (machine != null && machine.logDebugInfo)
                    Debug.Log("FSM Debug: Exit State - " + activeChild.name + " at " + Time.time);
            }
            currentStates.Clear();
        }


        /// <summary>
        /// 进入本状态 by吴江
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_event"></param>
        public void OnEnter(State _from, State _to, Event _event)
        {
            if (onEnter != null)
            {
                onEnter(_from, _to, _event);
            }
        }

        /// <summary>
        /// 退出本状态 by吴江
        /// </summary>
        /// <param name="_from"></param>
        /// <param name="_to"></param>
        /// <param name="_event"></param>
        public void OnExit(State _from, State _to, Event _event)
        {
            if (onExit != null)
            {
                onExit(_from, _to, _event);
            }
        }


        /// <summary>
        /// 所包含的状态总数 by吴江
        /// </summary>
        /// <returns></returns>
        public int TotalStates()
        {
            int count = 1;
            for (int i = 0; i < children.Count; ++i)
            {
                count += children[i].TotalStates();
            }
            return count;
        }


        public void ShowDebugInfo(int _level, bool _active, GUIStyle _textStyle)
        {
            _textStyle.normal.textColor = _active ? Color.green : new Color(0.5f, 0.5f, 0.5f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label(new string('\t', _level) + name, _textStyle, new GUILayoutOption[] { });
            GUILayout.EndHorizontal();

            for (int i = 0; i < children.Count; ++i)
            {
                State s = children[i];
                s.ShowDebugInfo(_level + 1, currentStates.IndexOf(s) != -1, _textStyle);
            }
        }
    }




    /// <summary>
    /// 最终状态
    /// </summary>
    [System.Serializable]
    public class FinalState : State
    {

        public FinalState(string _name, State _parent = null)
            : base(_name, _parent)
        {
            onEnter += OnFinished;
        }


        void OnFinished(State _from, State _to, Event _event)
        {
            Machine stateMachine = machine;
            if (stateMachine != null)
            {
                stateMachine.Send(Event.FINISHED);
            }
        }
    }
}


















