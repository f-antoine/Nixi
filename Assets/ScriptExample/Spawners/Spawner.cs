using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Spawners
{
    public sealed class Spawner : MonoBehaviourInjectable
    {
        // Tools for simplify debug
        public IEnumerable<Mob> GetEnumerable(GameObjectLevel? gameObjectLevel)
        {
            if (!gameObjectLevel.HasValue)
                return mobEnumerable;
            else if (gameObjectLevel.Value == GameObjectLevel.Children)
                return mobEnumerableChildren;
            else
                return mobEnumerableParent;
        }

        // Tools for simplify debug
        public List<Mob> GetList(GameObjectLevel? gameObjectLevel)
        {
            if (!gameObjectLevel.HasValue)
                return mobList;
            else if (gameObjectLevel.Value == GameObjectLevel.Children)
                return mobListChildren;
            else
                return mobListParent;
        }

        // Tools for simplify debug
        public Mob[] GetArray(GameObjectLevel? gameObjectLevel)
        {
            if (!gameObjectLevel.HasValue)
                return mobArray;
            else if (gameObjectLevel.Value == GameObjectLevel.Children)
                return mobArrayChildren;
            else
                return mobArrayParent;
        }

        // To simplify, in real world we should use NixInjectTestMock and pass prefab by drag & drop
        [Component]
        private Mob mobPrefab;
        public Mob MobPrefab => mobPrefab;

        #region Current
        [ComponentsFromParents]
        private List<Mob> mobListParent;
        public List<Mob> MobListParent => mobListParent;

        [ComponentsFromParents]
        private IEnumerable<Mob> mobEnumerableParent;
        public IEnumerable<Mob> MobEnumerableParent => mobEnumerableParent;

        [ComponentsFromParents]
        private Mob[] mobArrayParent;
        public Mob[] MobArrayParent => mobArrayParent;
        #endregion Current

        #region Current
        [Components]
        private List<Mob> mobList;
        public List<Mob> MobList => mobList;

        [Components]
        private IEnumerable<Mob> mobEnumerable;
        public IEnumerable<Mob> MobEnumerable => mobEnumerable;

        [Components]
        private Mob[] mobArray;
        public Mob[] MobArray => mobArray;
        #endregion Current

        #region Children
        [ComponentsFromChildren]
        private List<Mob> mobListChildren;
        public List<Mob> MobListChildren => mobListChildren;

        [ComponentsFromChildren]
        private IEnumerable<Mob> mobEnumerableChildren;
        public IEnumerable<Mob> MobEnumerableChildren => mobEnumerableChildren;

        [ComponentsFromChildren]
        private Mob[] mobArrayChildren;
        public Mob[] MobArrayChildren => mobArrayChildren;
        #endregion Children

        public void SpawnOnParent()
        {
            Mob newMob = Instantiate(mobPrefab);
            mobListParent.Add(newMob);
        }

        public void SpawnOnCurrent()
        {
            Mob newMob = Instantiate(mobPrefab);
            mobList.Add(newMob);
        }

        public void SpawnOnChild()
        {
            Mob newMob = Instantiate(mobPrefab);
            mobListChildren.Add(newMob);
        }
    }
}