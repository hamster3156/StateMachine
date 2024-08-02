using System.Collections.Generic;
using UnityEngine;

namespace Hamster.Library
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        // �X�e�[�g�̑J�ڏ�����ǉ����鎫��
        private Dictionary<State, List<State>> transitionDictionary = new Dictionary<State, List<State>>();

        // �����X�e�[�g���m����̑J�ڏ�����ǉ����郊�X�g
        private List<State> equalTransitions = new List<State>();

        /// <summary>
        /// �ŏ��̃X�e�[�g�ɏ���������
        /// </summary>
        public void InitializeState<TState>(TState initializeState) where TState : State
        {
            CurrentState = initializeState;
            InitializeState();
        }

        /// <summary>
        /// ���݃X�e�[�g�̏������������s��
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
        /// �J�ڏ������������z��ɒǉ�����B�J�ڏ������ǉ��ς݂̏ꍇ���O��Ԃ��������Ȃ��B
        /// </summary>
        public void AddTransition<TState>(TState fromState, TState toState) where TState : State
        {
            // fromState�̃L�[�����݂��Ȃ��ꍇ�������ēo�^����
            if (!transitionDictionary.ContainsKey(fromState))
            {
                transitionDictionary.Add(fromState, new List<State>());
            }

            // �����X�e�[�g���m�̑J�ڏ��������X�g�ɓo�^����Ă��邩�m�F���A�J�ڏ��������ɑ��݂���ꍇ�̓��O��Ԃ��ď����𒆒f����B
            if (equalTransitions.Count > 0)
            {
                foreach (var checkToState in equalTransitions)
                {
                    if (checkToState == toState)
                    {
                        Debug.Log($"{fromState}����{toState}�̑J�ڏ����͊��ɒǉ�����Ă܂�");
                        return;
                    }
                }
            }

            // fromState�̃L�[��toState�̑J�ڏ��������ɑ��݂���ꍇ�̓��O��Ԃ��ď����𒆒f����B
            if (transitionDictionary[fromState].Contains(toState))
            {
                Debug.Log($"{fromState}����{toState}�̑J�ڏ����͊��ɒǉ�����Ă܂�");
                return;
            }

            // �����X�e�[�g���m�̑J�ڏ��������f���Ă���ǉ�����
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
        /// �Œ�t���[�����s�����
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        /// <summary>
        /// ���t���[�����s�����
        /// </summary>
        public void Update()
        {
            CurrentState?.OnUpdate();
        }

        /// <summary>
        /// ���t���[���̍Ō�Ɏ��s�����
        /// </summary>
        public void LateUpdate()
        {
            CurrentState?.OnLateUpdate();
        }

        /// <summary>
        /// �X�e�[�g�J�ڂ��s���B�����J�ڏ�����������Ȃ���΃��O���o���B
        /// </summary>
        public void TransitionTo<TState>(TState fromState, TState toState) where TState : State
        {
            if (!transitionDictionary.ContainsKey(fromState))
            {
                Debug.LogWarning($"�L�[����������Ă��܂���BAddTransition���\�b�h�ő�������{fromState.GetType()}�̑J�ڏ�����ǉ����Ă��������B");
                return;
            }

            // �����X�e�[�g���m�̑J�ڏ������ǉ�����Ă���ꍇ�m�F����
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

            // �X�e�[�g�̑J�ڏ������ǉ�����Ă��邩�m�F����
            if (transitionDictionary[fromState].Contains(toState))
            {
                TransitionTo(toState);
                InitializeState();
                return;
            }

            Debug.LogWarning($"{fromState}����{toState}�̑J�ڏ�����������܂���ł���");
        }

        /// <summary>
        ///�C�ӂ̃X�e�[�g�ɑJ�ڂ���
        /// </summary>
        public void AnyTransitionTo<TState>(TState anyState) where TState : State
        {
            TransitionTo(anyState);
            InitializeState();
        }

        /// <summary>
        /// ���݃X�e�[�g�̏I���������s���A���̃X�e�[�g�ɕύX����
        /// /// </summary>
        public void TransitionTo<TState>(TState nextState) where TState : State
        {
            CurrentState?.OnEnter();
            CurrentState?.OnExit();
            CurrentState = nextState;
        }
    }
}
