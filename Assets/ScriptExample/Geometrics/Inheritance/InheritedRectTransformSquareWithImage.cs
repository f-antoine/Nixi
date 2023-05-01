using Nixi.Injections;
using UnityEngine.UI;

namespace ScriptExample.Geometrics.Inheritance
{
    public sealed class InheritedRectTransformSquareWithImage : AbstractRectTransformSquare
    {
        [Component]
        private Image currentBackgroundImage;
    }
}