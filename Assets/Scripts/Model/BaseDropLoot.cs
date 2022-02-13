using UnityEngine;


namespace Old_Code
{
    public abstract class BaseDropLoot : BaseObjectScene
    {
        private ParticleHelper _markEffects;

        protected virtual void Start()
        {
            _markEffects = GetComponentInChildren<ParticleHelper>();
            _markEffects.Play();
            _markEffects.FreezeRotation = true;
        }
    }
}
