using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.WindowsMR
{
    public class WindowsMRBuildSettings : ScriptableObject
    {
        [SerializeField, Tooltip("Enable or disable the use of the primary application window for display when running as a WSA application.")]
        public bool UsePrimaryWindowForDisplay = true;
    }
}
