namespace Hamster.Library
{
    public abstract class State
    {
        /// <summary>
        /// ステートの初期化フラグ
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public virtual void OnInitialize()
        {
            Log("OnInitialize");
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        public virtual void OnEnter()
        {
            Log("OnEnter");
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void OnExit()
        {
            Log("OnExit");
        }

        /// <summary>
        /// 固定で更新する処理
        /// </summary>
        public virtual void OnFixedUpdate()
        {
            Log("OnFixedUpdate");
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public virtual void OnUpdate()
        {
            Log("OnUpdate");
        }

        /// <summary>
        /// 最後に更新する処理
        /// </summary>
        public virtual void OnLateUpdate()
        {
            Log("OnLateUpdate");
        }

        /// <summary>
        /// ステートの初期化フラグを初期化済みにする
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