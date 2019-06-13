using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.Management;

using XRGestureSubsystem = UnityEngine.XR.InteractionSubsystems.XRGestureSubsystem;
using XRGestureSubsystemDescriptor = UnityEngine.XR.InteractionSubsystems.XRGestureSubsystemDescriptor;

namespace UnityEngine.XR.WindowsMR
{
#if UNITY_EDITOR
    /// <summary>Generic interface used to access plugin specific settings by the loader.</summary>
    public interface IWindowsMRPackageSettings
    {
        /// <summary>Get the active build target settings.</summary>
        /// <returns>WindowsMRSettings</returns>
        WindowsMRSettings GetActiveBuildTargetSettings();
    }
#endif

    class WindowsMRLoader : XRLoaderHelper
    {
        private static List<XRDisplaySubsystemDescriptor> s_DisplaySubsystemDescriptors = new List<XRDisplaySubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();
        private static List<XRExperienceSubsystemDescriptor> s_ExperienceSubsystemDescriptors = new List<XRExperienceSubsystemDescriptor>();
        private static List<XRGestureSubsystemDescriptor> s_GestureSubsystemDescriptors = new List<XRGestureSubsystemDescriptor>();

        public XRDisplaySubsystem displaySubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRDisplaySubsystem>();
            }
        }

        public XRInputSubsystem inputSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRInputSubsystem>();
            }
        }

        public XRExperienceSubsystem experiencePointSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRExperienceSubsystem>();
            }
        }

        public XRGestureSubsystem gestureSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRGestureSubsystem>();
            }
        }

        public override bool Initialize()
        {
            WindowsMRSettings settings = GetSettings();
            if (settings != null)
            {
                Native.UserDefinedSettings uds;
                uds.depthBufferType = (ushort)settings.DepthBufferFormat;
                uds.sharedDepthBuffer = (ushort)(settings.UseSharedDepthBuffer ? 1 : 0);

                Native.SetUserDefinedSettings(uds);
            }

            Native.UnitySubsystemErrorCode code = Native.CreateHolographicSession();
            if (code != Native.UnitySubsystemErrorCode.kUnitySubsystemErrorCodeSuccess)
                return false;

            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(s_DisplaySubsystemDescriptors, "Windows Mixed Reality Display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "Windows Mixed Reality Input");
            CreateSubsystem<XRExperienceSubsystemDescriptor, XRExperienceSubsystem>(s_ExperienceSubsystemDescriptors, "Windows Mixed Reality Experience");
            CreateSubsystem<XRGestureSubsystemDescriptor, XRGestureSubsystem>(s_GestureSubsystemDescriptors, "Windows Mixed Reality Gesture");

            return displaySubsystem != null && inputSubsystem != null;
        }

        public override bool Start()
        {
            Native.UnitySubsystemErrorCode code = Native.StartHolographicSession();
            if (code != Native.UnitySubsystemErrorCode.kUnitySubsystemErrorCodeSuccess)
                return false;

            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();
            StartSubsystem<XRExperienceSubsystem>();
            StartSubsystem<XRGestureSubsystem>();
            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();
            StopSubsystem<XRExperienceSubsystem>();
            StopSubsystem<XRGestureSubsystem>();

            Native.StopHolographicSession();
            return true;
        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRExperienceSubsystem>();
            DestroySubsystem<XRGestureSubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRDisplaySubsystem>();

            Native.DestroyHolographicSession();
            return true;
        }


        public WindowsMRSettings GetSettings()
        {
            WindowsMRSettings settings = null;
#if UNITY_EDITOR
            UnityEngine.Object obj = null;
            UnityEditor.EditorBuildSettings.TryGetConfigObject<UnityEngine.Object>(Constants.k_SettingsKey, out obj);
            if (obj == null || !(obj is IWindowsMRPackageSettings))
                return null;

            IWindowsMRPackageSettings packageSettings = (IWindowsMRPackageSettings)obj;

            settings =  packageSettings.GetActiveBuildTargetSettings();
#else
            settings = WindowsMRSettings.s_Settings;
#endif
            return settings;
        }
    }

}
