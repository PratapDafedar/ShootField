using System;

namespace SocketBase
{
    public class ThreadJob
    {
        private bool m_IsDone = false;
        private object m_Handle = new object();
        private System.Threading.Thread m_Thread = null;

        private int deltaCallTime;

        System.Action threadMethod;

        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_IsDone;
                }
                return tmp;
            }
            set
            {
                lock (m_Handle)
                {
                    m_IsDone = value;
                }
            }
        }

        public ThreadJob(System.Action _threadMethod, int _deltaCallTime = 200)
        {
            this.threadMethod = _threadMethod;
            this.deltaCallTime = _deltaCallTime;
            m_Thread = new System.Threading.Thread(Run);
            m_Thread.Start();
        }

        public virtual void Abort()
        {
            m_Thread.Abort();
        }

        protected virtual void ThreadFunction()
        {
            if (threadMethod != null)
            {
                while (true)
                {
                    threadMethod();
                    System.Threading.Thread.Sleep(deltaCallTime);
                }
            }
        }

        protected virtual void OnFinished() 
        { 

        }

        private void Run()
        {
            ThreadFunction();

            IsDone = true;

            OnFinished();
        }
    }
}