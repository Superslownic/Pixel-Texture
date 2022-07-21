using System;
using UnityEngine;

namespace Pixel.Texture
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AsPaletteAttribute : PropertyAttribute
    {
    }
}