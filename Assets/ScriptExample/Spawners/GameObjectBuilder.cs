using UnityEngine;

namespace ScriptExample.Spawners
{
    public sealed class GameObjectBuilder : IGameObjectBuilder
    {
        public T Instantiate<T>(T prefab)
            where T : Object
        {
            return Object.Instantiate(prefab);
        }

        public T Instantiate<T>(T prefab, Transform parentTransform)
            where T : Object
        {
            return Object.Instantiate(prefab, parentTransform);
        }
    }
}