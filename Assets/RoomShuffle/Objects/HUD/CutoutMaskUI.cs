using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

//Taken from https://unitycodemonkey.com/video.php?v=XJJl19N2KFM
public class CutoutMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            var mat = new Material(base.materialForRendering);
            mat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return mat;
        }
    }
}