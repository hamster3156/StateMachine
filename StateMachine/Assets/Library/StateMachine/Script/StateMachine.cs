using System.Collections.Generic;
using UnityEngine;

namespace Hamster.Library
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        // ステートの遷移条件を追加する辞書
        private Dictionary<State, List<State>> transitionDictionary = new Dictionary<State, List<State>>();

        // 同じステート同士からの遷移条件を追加するリスト
        private List<State> equalTransitions = new List<State>();

        /// <summary>
        /// 最初のステートに初期化する
        /// </summary>
        public void InitializeState<TState>(TState initializeState) where TState : State
        {
            CurrentState = initializeState;
            InitializeState();
        }

        /// <summary>
        /// 現在ステートの初期化処理を行う
        /// </summary>
        public void InitializeState()
        {
            if (!CurrentState.IsInitialized)
            {
                CurrentState?.OnInitialize();
                CurrentState?.SetInitialize();
            }
        }

        /// <summary>
        /// 遷移条件を辞書か配列に追加する。遷移条件が追加済みの場合ログを返し何もしない。
        /// </summary>
        public void AddTransition<TState>(TState fromState, TState toState) where TState : State
        {
            // fromStateのキーが存在しない場合生成して登録する
            if (!transitionDictionary.ContainsKey(fromState))
            {
                transitionDictionary.Add(fromState, new List<State>());
            }

            // 同じステート同士の遷移条件がリストに登録されているか確認し、遷移条件が既に存在する場合はログを返して処理を中断する。
            if (equalTransitions.Count > 0)
            {
                foreach (var checkToState in equalTransitions)
                {
                    if (checkToState == toState)
                    {
                        Debug.Log($"{fromState}から{toState}の遷移条件は既に追加されてます");
                        return;
                    }
                }
            }

            // fromStateのキーにtoStateの遷移条件が既に存在する場合はログを返して処理を中断する。
            if (transitionDictionary[fromState].Contains(toState))
            {
                Debug.Log($"{fromState}から{toState}の遷移条件は既に追加されてます");
                return;
            }

            // 同じステート同士の遷移条件か判断してから追加する
            if (EqualityComparer<TState>.Default.Equals(fromState, toState))
            {
                equalTransitions.Add(toState);
            }
            else
            {
                transitionDictionary[fromState].Add(toState);
            }
        }

        /// <summary>
        /// 固定フレーム実行される
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        /// <summary>
        /// 毎フレーム実行される
        /// </summary>
        public void Update()
        {
            CurrentState?.OnUpdate();
        }

        /// <summary>
        /// 毎フレームの最後に実行される
        /// </summary>
        public void LateUpdate()
        {
            CurrentState?.OnLateUpdate();
        }

        /// <summary>
        /// ステート遷移を行う。もし遷移条件が見つからなければログを出す。
        /// </summary>
        public void TransitionTo<TState>(TState fromState, TState toState) where TState : State
        {
            if (!transitionDictionary.ContainsKey(fromState))
            {
                Debug.LogWarning($"キーが生成されていません。AddTransitionメソッドで第一引数が{fromState.GetType()}の遷移条件を追加してください。");
                return;
            }

            // 同じステート同士の遷移条件が追加されている場合確認する
            if (equalTransitions.Count > 0)
            {
                foreach (var checkToState in equalTransitions)
                {
                    if (checkToState == toState)
                    {
                        TransitionTo(toState);
                        InitializeState();
                        return;
                    }
                }
            }

            // ステートの遷移条件が追加されているか確認する
            if (transitionDictionary[fromState].Contains(toState))
            {
                TransitionTo(toState);
                InitializeState();
                return;
            }

            Debug.LogWarning($"{fromState}から{toState}の遷移条件が見つかりませんでした");
        }

        /// <summary>
        ///任意のステートに遷移する
        /// </summary>
        public void AnyTransitionTo<TState>(TState anyState) where TState : State
        {
            TransitionTo(anyState);
            InitializeState();
        }

        /// <summary>
        /// 現在ステートの終了処理を行い、次のステートに変更する
        /// /// </summary>
        public void TransitionTo<TState>(TState nextState) where TState : State
        {
            CurrentState?.OnEnter();
            CurrentState?.OnExit();
            CurrentState = nextState;
        }
    }
}
