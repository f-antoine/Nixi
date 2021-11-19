using Nixi.Injections;
using Nixi.Injections.Attributes;
using UnityEngine;

namespace ScriptExample.Geometrics.Inheritance
{
    /// <summary>
    /// Bouton personalisé pour utilisation UI
    /// </summary>
    public abstract class AbstractRectTransformSquare : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        protected RectTransform buttonRectangle;
    }
}