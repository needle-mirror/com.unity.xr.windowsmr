using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Experimental.XR;

using UnityEngine.XR.Management;

namespace Unity.XR.WindowsMR
{
#if UNITY_EDITOR
    public interface IWindowsMRPackageSettings
    {
        WindowsMRSettings GetActiveBuildTargetSettings();
    }
#endif

    class WindowsMRLoader : XRLoaderHelper
    {
        private static List<XRSessionSubsystemDescriptor> s_SessionSubsystemDescriptors = new List<XRSessionSubsystemDescriptor>();
        private static List<XRDisplaySubsystemDescriptor> s_DisplaySubsystemDescriptors = new List<XRDisplaySubsystemDescriptor>();
        private static List<XRInputSubsystemDescriptor> s_InputSubsystemDescriptors = new List<XRInputSubsystemDescriptor>();
        private static List<XRReferencePointSubsystemDescriptor> s_ReferencePointSubsystemDescriptors = new List<XRReferencePointSubsystemDescriptor>();
        private static List<XRMeshSubsystemDescriptor> s_MeshSubsystemDescriptors = new List<XRMeshSubsystemDescriptor>();
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

        public XRSessionSubsystem sessionSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRSessionSubsystem>();
            }
        }

        public XRReferencePointSubsystem referencePointSubsystem
        {
            get
            {
                return GetLoadedSubsystem<XRReferencePointSubsystem>();
            }
        }

        public XRMeshSubsystem meshSubsystemDescriptor
        {
            get
            {
                return GetLoadedSubsystem<XRMeshSubsystem>();
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
            CreateSubsystem<XRSessionSubsystemDescriptor, XRSessionSubsystem>(s_SessionSubsystemDescriptors, "Windows Mixed Reality Session");
            if (sessionSubsystem == null)
                return false;

            CreateSubsystem<XRDisplaySubsystemDescriptor, XRDisplaySubsystem>(s_DisplaySubsystemDescriptors, "Windows Mixed Reality Display");
            CreateSubsystem<XRInputSubsystemDescriptor, XRInputSubsystem>(s_InputSubsystemDescriptors, "Windows Mixed Reality Input");
            CreateSubsystem<XRReferencePointSubsystemDescriptor, XRReferencePointSubsystem>(s_ReferencePointSubsystemDescriptors, "Windows Mixed Reality Reference Point");
            CreateSubsystem<XRMeshSubsystemDescriptor, XRMeshSubsystem>(s_MeshSubsystemDescriptors, "Windows Mixed Reality Meshing");
            CreateSubsystem<XRExperienceSubsystemDescriptor, XRExperienceSubsystem>(s_ExperienceSubsystemDescriptors, "Windows Mixed Reality Experience");
            CreateSubsystem<XRGestureSubsystemDescriptor, XRGestureSubsystem>(s_GestureSubsystemDescriptors, "Windows Mixed Reality Gesture");

            return displaySubsystem != null && inputSubsystem != null;
        }

        public override bool Start()
        {
            WindowsMRSettings settings = GetSettings();
            if (settings != null)
            {
                UserDefinedSettings uds;
                uds.depthBufferType = (ushort)settings.DepthBufferFormat;
                uds.sharedDepthBuffer = (ushort)(settings.UseSharedDepthBuffer ? 1 : 0);

                SetUserDefinedSettings(uds);
            }

            StartSubsystem<XRSessionSubsystem>();
            StartSubsystem<XRDisplaySubsystem>();
            StartSubsystem<XRInputSubsystem>();
            StartSubsystem<XRReferencePointSubsystem>();
            StartSubsystem<XRMeshSubsystem>();
            StartSubsystem<XRExperienceSubsystem>();
            StartSubsystem<XRGestureSubsystem>();
            return true;
        }

        public override bool Stop()
        {
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRInputSubsystem>();
            StopSubsystem<XRExperienceSubsystem>();
            StopSubsystem<XRReferencePointSubsystem>();
            StopSubsystem<XRInputSubsystem>();
            StopSubsystem<XRDisplaySubsystem>();
            StopSubsystem<XRSessionSubsystem>();
            StopSubsystem<XRReferencePointSubsystem>();
            StopSubsystem<XRMeshSubsystem>();
            return true;
        }

        public override bool Deinitialize()
        {
            DestroySubsystem<XRExperienceSubsystem>();
            DestroySubsystem<XRReferencePointSubsystem>();
            DestroySubsystem<XRGestureSubsystem>();
            DestroySubsystem<XRInputSubsystem>();
            DestroySubsystem<XRDisplaySubsystem>();
            DestroySubsystem<XRSessionSubsystem>();
            DestroySubsystem<XRReferencePointSubsystem>();
            DestroySubsystem<XRMeshSubsystem>();
            return true;
        }


        [StructLayout(LayoutKind.Sequential)]
        struct UserDefinedSettings
        {
            public ushort depthBufferType;
            public ushort sharedDepthBuffer;
        }

#if UNITY_EDITOR
        [DllImport("Packages/com.unity.xr.windowsmr/Runtime/Plugins/x64/WindowsMRXRSDK.dll", CharSet = CharSet.Auto)]
#else
#if ENABLE_DOTNET
        [DllImport("WindowsMRXRSDK.dll")]
#else
        [DllImport("WindowsMRXRSDK.dll", CharSet = CharSet.Auto)]
#endif
#endif
        static extern void SetUserDefinedSettings(UserDefinedSettings settings);

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
