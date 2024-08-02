namespace Hamster.Library
{
    public abstract class State
    {
        /// <summary>
        /// �X�e�[�g�̏������t���O
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// ����������
        /// </summary>
        public virtual void OnInitialize()
        {
            Log("OnInitialize");
        }

        /// <summary>
        /// �J�n����
        /// </summary>
        public virtual void OnEnter()
        {
            Log("OnEnter");
        }

        /// <summary>
        /// �I������
        /// </summary>
        public virtual void OnExit()
        {
            Log("OnExit");
        }

        /// <summary>
        /// �Œ�ōX�V���鏈��
        /// </summary>
        public virtual void OnFixedUpdate()
        {
            Log("OnFixedUpdate");
        }

        /// <summary>
        /// �X�V����
        /// </summary>
        public virtual void OnUpdate()
        {
            Log("OnUpdate");
        }

        /// <summary>
        /// �Ō�ɍX�V���鏈��
        /// </summary>
        public virtual void OnLateUpdate()
        {
            Log("OnLateUpdate");
        }

        /// <summary>
        /// �X�e�[�g�̏������t���O���������ς݂ɂ���
        /// </summary>
        public void SetInitialize()
        {
            IsInitialized = true;
        }

        private void Log(string comment)
        {
            UnityEngine.Debug.Log($"[State] this.GetType().Name {comment}");
        }
    }
}