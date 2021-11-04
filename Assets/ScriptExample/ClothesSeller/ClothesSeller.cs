using Nixi.Injections.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ScriptExample.ClothesSeller
{
    public sealed class ClothesSeller
    {
        [NixInjectComponent]
        public Clothes ClothesOnMe;

        //[NixInjectComponentFromMethod("")]
        //public Clothes[] ClothesOnMe;
    }
}
