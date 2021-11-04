using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ScriptExample.Flowers
{
    public sealed class Bouquet : MonoBehaviourInjectable
    {
		// First flower, used as reference for all component root instance tests
		[NixInjectComponent]
		public DualFlower ReferentialFlower;

		[NixInjectComponentFromChildren("ChildFlower")]
		public DualFlower ChildFlower;

		[NixInjectComponentFromParent("ParentFlower")]
		public DualFlower ParentFlower;

		[NixInjectRootComponent("RootIsolatedFlower")]
		public DualFlower RootIsolatedFlower;

		[NixInjectRootComponent("RootIsolatedFlower", "SubRootIsolatedFlower")]
		public DualFlower SubRootIsolatedFlower;

		[NixInjectRootComponent("RootIsolatedFlower", "SubRootIsolatedFlower")]
		public Image Image;

		[NixInjectComponents]
		public List<DualFlower> FlowersRemaining;
	}
}