using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Flowers
{
    public sealed class Bouquet : MonoBehaviourInjectable
    {
		// First flower, used as reference for all component root instance tests
		[NixInjectComponent]
		public DualFlower ReferentialFlower;

		[NixInjectComponentFromMethod("ChildFlower", GameObjectMethod.GetComponentsInChildren)]
		public DualFlower ChildFlower;

		[NixInjectComponentFromMethod("ParentFlower", GameObjectMethod.GetComponentsInParent)]
		public DualFlower ParentFlower;

		[NixInjectRootComponent("RootIsolatedFlower")]
		public DualFlower RootIsolatedFlower;

		[NixInjectRootComponent("RootIsolatedFlower", "SubRootIsolatedFlower")]
		public DualFlower SubRootIsolatedFlower;

		[NixInjectComponentList]
		public List<DualFlower> FlowersRemaining;
	}
}