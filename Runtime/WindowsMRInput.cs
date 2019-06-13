using System;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.Experimental.XR;

namespace UnityEngine.XR.WindowsMR
{
    /// <summary>Extension methods for XRInputSubsystem specific to WindowsMR</summary>
    public static class WindowsMRInput
    {
        public static NativeTypes.SpatialLocatability GetSpatialLocatability(this XRInputSubsystem input)
        {
            return Native.GetSpatialLocatability();
        }
    }
}
