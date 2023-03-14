using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ScriptExample.Flowers
{
    public sealed class Bouquet : MonoBehaviourInjectable
    {
		// First flower, used as reference for all component root instance tests
		[Component]
		public DualFlower ReferentialFlower;

		[ComponentFromChildren("ChildFlower")]
		public DualFlower ChildFlower;

		[ComponentFromParents("ParentFlower")]
		public DualFlower ParentFlower;

		[RootComponent("RootIsolatedFlower")]
		public DualFlower RootIsolatedFlower;

		[RootComponent("RootIsolatedFlower", "SubRootIsolatedFlower")]
		public DualFlower SubRootIsolatedFlower;

		[RootComponent("RootIsolatedFlower", "SubRootIsolatedFlower")]
		public Image Image;

		[Components]
		public List<DualFlower> FlowersRemaining;
	}
}