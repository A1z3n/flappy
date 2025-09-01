using UnityEngine;

namespace Assets.Scripts
{
    public class pausableObject : MonoBehaviour
    {
        protected bool isPaused = true;
        public void Pause()
        {
            isPaused = true;
        }
        public void Resume()
        {
            isPaused = false;
        }

        public virtual void Restart()
        {
            
        }
    }
}
