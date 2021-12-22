using UnityEngine;

namespace ScriptExample.AbstractMapping
{
    public abstract class AbstractComponentBase : MonoBehaviour
    {
        public virtual int ValueToRetrieve => 888;
    }
}