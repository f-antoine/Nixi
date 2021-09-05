using Assets.ScriptExample.Controllers;
using UnityEngine;

namespace Assets.Tests.Builders
{
    internal sealed class ControllersBuilder
    {
        private ControllersBuilder()
        {
        }

        internal static ControllersBuilder Create()
        {
            return new ControllersBuilder();
        }

        internal MonsterController BuildMonsterController()
        {
            return new GameObject("MonsterController").AddComponent<MonsterController>();
        }

        internal SorcererController BuildSorcererController()
        {
            return new GameObject("SorcererController").AddComponent<SorcererController>();
        }

        internal AllMightyController BuildAllMightyController()
        {
            return new GameObject("AllMightyController").AddComponent<AllMightyController>();
        }

        internal AllMightyControllerFull BuildAllMightyControllerFull()
        {
            return new GameObject("AllMightyControllerWithChilds").AddComponent<AllMightyControllerFull>();
        }

        internal AllMightyWithChilds BuildAllMightyWithChilds()
        {
            return new GameObject("AllMightyControllerWithChilds").AddComponent<AllMightyWithChilds>();
        }

        internal AllMightyWithSameChilds BuildAllMightyWithSameChilds()
        {
            return new GameObject("AllMightyWithWrongChilds").AddComponent<AllMightyWithSameChilds>();
        }
    }
}
