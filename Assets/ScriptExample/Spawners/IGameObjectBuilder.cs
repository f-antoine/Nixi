using UnityEngine;

namespace ScriptExample.Spawners
{
    public interface IGameObjectBuilder
    {
        T Instantiate<T>(T prefab) where T : Object;
    }
}