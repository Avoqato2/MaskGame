using System;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace PSX
{
    [Serializable]
    [VolumeComponentMenu("PSX/Pixelation")]
    public class Pixelation : VolumeComponent, IPostProcessComponent
    {
        public IntParameter widthPixelation = new IntParameter(320, true);
        public IntParameter heightPixelation = new IntParameter(180, true);
        public FloatParameter colorPrecision = new FloatParameter(32.0f, true);

        public bool IsActive() => AnyPropertiesIsOverridden();
    }
}