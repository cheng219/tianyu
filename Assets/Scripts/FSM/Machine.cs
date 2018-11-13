///////////////////////////////////////////////////////////////////////////
//状态机
//作者：吴江
//日期：2015/4/21
//描述：一些状态机的基本功能。所有游戏逻辑基于这个类派生
///////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace fsm
{
    /// <summary>
    /// 状态机 by吴江
    /// </summary>
    public class Machine : State
    {
        /// <summary>
        /// 状态机运行状态的枚举 
        /// </summary>
        public enum MachineState
        {
            Running,
            Paused,
            Stopping,
            Stopped
        }
        public delegate void OnEventHandler();


        public bool showDebugInfo = true;
        public bool logDebugInfo = false;

        /// <summary>
        /// 状态机启动事件 by吴江
        /// </summary>
        public OnEventHandler onStart;

        /// <summary>
        /// 状态机停止事件 by吴江
        /// </summary>
        public OnEventHandler onStop;

        /// <summary>
        /// 当前状态机运行状态
        /// </summary>
        /// <returns></returns>
        protected MachineState machineState = MachineState.Stopped;
        /// <summary>
        /// 当前状态机运行状态
        /// </summary>
        /// <returns></returns>
        public MachineState GetMachineState()
        {
            return machineState;
        }

        /// <summary>
        /// 状态机初始状态，特殊需求下可以填塞内容，但一般作为一个跳转的起点，没有其他意义。
        /// startState跟initState是不同的，流程为 startState --transition--> initState 
        /// by吴江
        /// </summary>
        protected State startState = new State("fsm_start"); 
        /// <summary>
        /// 等待处理的【跳转信息】队列 by吴江
        /// </summary>
        protected List<Event>[] eventBuffer = new List<Event>[2] { new List<Event>(), new List<Event>() };
        protected int curEventBufferIdx = 0;
        protected int nextEventBufferIdx = 1;
        protected bool isUpdating = false;
        protected List<Transition> validTransitions = new List<Transition>();



        public Machine() : base("fsm_state_machine")
        {

        }

        /// <summary>
        /// 重启状态机 by吴江
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }


        /// <summary>
        /// 启动状态机 by吴江
        /// </summary>
        public void Start()
        {
            if (machineState == MachineState.Running ||
                 machineState == MachineState.Paused)
                return;//如果已经启动，返回。

            machineState = MachineState.Running;
            if (onStart != null)
                onStart();

            Event nullEvent = new Event(Event.NULL);
            if (mode == State.Mode.Exclusive)
            {
                if (initState != null)
                {
                    EnterStates(nullEvent, initState, startState);
                }
                else
                {
                    Debug.LogError("FSM error: can't find initial state in " + name);
                }
            }
            else
            { // if ( _toEnter.mode == State.Mode.Parallel )
                for (int i = 0; i < children.Count; ++i)
                {
                    EnterStates(nullEvent, children[i], startState);
                }
            }
        }


        /// <summary>
        /// 终止状态机 by吴江
        /// </summary>
        public void Stop()
        {
            if (machineState == MachineState.Stopped)
                return;

            if (isUpdating)
            {
                machineState = MachineState.Stopping;
            }
            else
            {
                ProcessStop();
            }
        }

        /// <summary>
        /// 状态机停止的工作 by吴江
        /// </summary>
        protected void ProcessStop()
        {
            eventBuffer[0].Clear();
            eventBuffer[1].Clear();
            ClearCurrentStatesRecursively();

            if (onStop != null)
                onStop();

            machineState = MachineState.Stopped;
        }

        /// <summary>
        /// 状态机的更新函数
        /// NOTE:并非一定要放在unity进程的update中，某些特定的情况，可以手动指定更新  by吴江
        /// </summary>
        public void Update()
        {
            if (machineState == MachineState.Paused ||
                 machineState == MachineState.Stopped)
                return;

            isUpdating = true;

            if (machineState != MachineState.Stopping)
            {
                int tmp = curEventBufferIdx;
                curEventBufferIdx = nextEventBufferIdx;
                nextEventBufferIdx = tmp;

                bool doStop = false;
                List<Event> eventList = eventBuffer[curEventBufferIdx];
                for (int i = 0; i < eventList.Count; ++i)
                {
                    if (HandleEvent(eventList[i]))
                    {
                        doStop = true;
                        break;
                    }
                }
                eventList.Clear();
                if (doStop)
                {
                    Stop();
                }
                else
                {
                    OnAction();
                }
            }
            isUpdating = false;

            if (machineState == MachineState.Stopping)
            {
                ProcessStop();
            }
        }


        public void Pause() { machineState = MachineState.Paused; }
        public void Resume() { machineState = MachineState.Running; }

         /// <summary>
         /// 状态跳转处理 by吴江
         /// </summary>
         /// <param name="_event"></param>
         /// <returns></returns>
        protected bool HandleEvent(Event _event)
        {
            OnEvent(_event);

            //更新跳转列表
            validTransitions.Clear();
            TestTransitions(ref validTransitions, _event);

            //跳转
            ExitStates(_event, validTransitions); // invoke State.OnExit
            ExecTransitions(_event, validTransitions); // invoke Transition.OnTransition
            EnterStates(_event, validTransitions); // invoke State.OnEnter


            //如果为最终状态，则需要停止状态机
            if (_event.id == Event.FINISHED)
            {
                bool canStop = true;

                for (int i = 0; i < currentStates.Count; ++i)
                {

                    if ((currentStates[i] is FinalState) == false)
                    {
                        canStop = false;
                        break;
                    }
                }
                if (canStop)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通知状态机跳转
        /// </summary>
        /// <param name="_eventID"></param>
        public void Send(int _eventID) { Send(new Event(_eventID)); }
        /// <summary>
        /// 通知状态机跳转
        /// </summary>
        public void Send(Event _event)
        {
            if (machineState == MachineState.Stopped)
                return;
            eventBuffer[nextEventBufferIdx].Add(_event);
        }


        /// <summary>
        /// 根据跳转列表执行进入状态 by吴江
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_transitionList"></param>
        protected void EnterStates(Event _event, List<Transition> _transitionList)
        {
            for (int i = 0; i < _transitionList.Count; ++i)
            {
                Transition transition = _transitionList[i];
                State targetState = transition.target;
                if (targetState == null)
                    targetState = transition.source;

                if (targetState.parent != null)
                    targetState.parent.EnterStates(_event, targetState, transition.source);
            }
        }


        /// <summary>
        /// 根据跳转列表执行退出状态 by吴江
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_transitionList"></param>
        protected void ExitStates(Event _event, List<Transition> _transitionList)
        {
            for (int i = 0; i < _transitionList.Count; ++i)
            {
                Transition transition = _transitionList[i];

                if (transition.source.parent != null)
                    transition.source.parent.ExitStates(_event, transition.target, transition.source);
            }
        }



        protected void ExecTransitions(Event _event, List<Transition> _transitionList)
        {
            for (int i = 0; i < _transitionList.Count; ++i)
            {
                Transition transition = _transitionList[i];
                transition.OnTransition(_event);
            }
        }


        public void ShowDebugGUI(string _name, GUIStyle _textStyle)
        {
            GUILayout.Label("State Machine (" + _name + ")");
            showDebugInfo = GUILayout.Toggle(showDebugInfo, "Show States");
            logDebugInfo = GUILayout.Toggle(logDebugInfo, "Log States");
            if (showDebugInfo)
            {
                ShowDebugInfo(0, true, _textStyle);
            }
        }
    }
}

















