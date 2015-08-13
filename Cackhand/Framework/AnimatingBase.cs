using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Framework
{
    public abstract class AnimatingBase : IState
    {
        private IList<IEnumerator> activeCoroutines;

        public AnimatingBase()
        {
            activeCoroutines = new List<IEnumerator>();
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
