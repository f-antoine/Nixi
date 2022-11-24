using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using UnityEngine.UI;

namespace ScriptExample.Geometrics.Inheritance
{
    public sealed class InheritedRectTransformSquareWithImage : AbstractRectTransformSquare
    {
        [NixInjectComponent]
        private Image currentBackgroundImage;
    }
}