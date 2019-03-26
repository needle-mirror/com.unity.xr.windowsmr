using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEditor;

using UnityEditor.XR.Management;

namespace Unity.XR.WindowsMR.Editor
{
    class WindowsMRXRSDKPackageInitialization : XRPackageInitializationBase
    {
        public string PackageName { get { return "Windows MR XR SDK"; } }
        public string LoaderFullTypeName { get { return "Unity.XR.WindowsMR.WindowsMRLoader"; } }
        public string LoaderTypeName { get { return "WindowsMRLoader"; } }
        public string SettingsFullTypeName { get{ return "Unity.XR.WindowsMR.Editor.WindowsMRPackageSettings"; } }
        public string SettingsTypeName { get{ return "WindowsMRPackageSettings"; } }
        public string PackageInitKey { get { return "Windows MR Package Initialization"; } }

        public bool PopulateSettingsOnInitialization(ScriptableObject obj)
        {
            WindowsMRPackageSettings packageSettings = obj as WindowsMRPackageSettings;
            if (packageSettings != null)
            {
                var settings = packageSettings.GetSettingsForBuildTargetGroup(BuildTargetGroup.WSA);
                if (settings != null)
                {
                    switch (PlayerSettings.VRWindowsMixedReality.depthBufferFormat)
                    {
                    case PlayerSettings.VRWindowsMixedReality.DepthBufferFormat.DepthBufferFormat16Bit:
                        settings.DepthBufferFormat = WindowsMRSettings.DepthBufferOption.DepthBuffer16Bit;
                        break;
                    case PlayerSettings.VRWindowsMixedReality.DepthBufferFormat.DepthBufferFormat24Bit:
                        settings.DepthBufferFormat = WindowsMRSettings.DepthBufferOption.DepthBuffer24Bit;
                        break;
                    }

                    settings.UseSharedDepthBuffer = PlayerSettings.VRWindowsMixedReality.depthBufferSharingEnabled;
                    return true;
                }
            }
            return false;
        }

    }
}
