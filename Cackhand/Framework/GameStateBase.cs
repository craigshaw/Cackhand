using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Framework
{
    internal abstract class GameStateBase : IState
    {
        private IList<IEnumerator> activeCoroutines;
        private readonly IStateManager stateManager;

        public GameStateBase(IStateManager stateManager)
        {
            if (stateManager == null)
                throw new ArgumentNullException("stateManager");

            this.stateManager = stateManager;
            activeCoroutines = new List<IEnumerator>();
        }

        protected IStateManager StateManager { get { return stateManager; } }

        protected void RegisterNextState(IState nextState)
        {
            stateManager.RegisterNextState(nextState);
        }

        protected void StartCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null)
                throw new ArgumentNullException("coroutine");

            activeCoroutines.Add(coroutine);
        }

        protected void RunCoroutines()
        {
            List<IEnumerator> completedCoroutines = new List<IEnumerator>();

            foreach (var coroutine in activeCoroutines)
            {
                if (!coroutine.MoveNext())
                    completedCoroutines.Add(coroutine);
            }

            foreach (var coroutine in completedCoroutines)
                activeCoroutines.Remove(coroutine);
        }

        public abstract void Initialise();

        public virtual void ProcessFrame()
        {
            RunCoroutines();
        }
    }
}
