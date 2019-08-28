using System;
using System.IO;

using UnityEngine;
using UnityEngine.Scripting;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WINRT
using System.Runtime.InteropServices;
#endif

namespace UnityEngine.XR.WindowsMRInternals
{
    [Preserve]
    internal class WindowsMRInternal
    {
        static WindowsMRInternal()
        {
            string pluginFolderPathBase = Path.GetFullPath(Path.Combine("Packages", "com.unity.xr.windowsmr.metro"));
            pluginFolderPathBase = Path.Combine(pluginFolderPathBase, "Plugins");

            string pluginFolderPath_x64 = Path.Combine(pluginFolderPathBase, "x64");
            string pluginFolderPath_x86_64 = Path.Combine(pluginFolderPathBase, "x86_64");
            UnityWindowsMR_EmulationLibs_SetPluginFolderPaths(pluginFolderPath_x86_64, pluginFolderPath_x64);
        }

        internal static void Init()
        {
        }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WINRT
        [DllImport("WindowsMRXRSDK")]
        static extern void UnityWindowsMR_EmulationLibs_SetPluginFolderPaths(
            [MarshalAs(UnmanagedType.LPWStr)] string pluginFolderPath_x86_64,
            [MarshalAs(UnmanagedType.LPWStr)] string pluginFolderPath_x64);
#else
        static void UnityWindowsMR_EmulationLibs_SetPluginFolderPaths(string pluginFolderPath_x86_64, string pluginFolderPath_x64)
        {
        }
#endif
    }
}
