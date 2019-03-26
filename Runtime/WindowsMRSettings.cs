using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Management;

namespace Unity.XR.WindowsMR
{
    public class WindowsMRSettings : ScriptableObject
    {
        public enum DepthBufferOption
        {
            DepthBuffer16Bit,
            DepthBuffer24Bit
        }

        [SerializeField, Tooltip("Set the size of the depth buffer")]
        public DepthBufferOption DepthBufferFormat;

        [SerializeField, Tooltip("Enable depth buffer sharing")]
        public bool UseSharedDepthBuffer;

#if !UNITY_EDITOR
        internal static WindowsMRSettings s_Settings;

        public void Awake()
        {
            s_Settings = this;
        }
#endif
    }
}
