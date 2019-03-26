using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.Management;

using UnityEngine;
using UnityEngine.XR.Management;

using Unity.XR.WindowsMR;

namespace Unity.XR.WindowsMR.Editor
{
    public class WindowsMRBuildProcessor : XRBuildHelper<WindowsMRSettings>
    {
        public override string BuildSettingsKey { get { return Constants.k_SettingsKey; } }

        private WindowsMRPackageSettings PackageSettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            UnityEngine.Object settingsObj = null;
            EditorBuildSettings.TryGetConfigObject(BuildSettingsKey, out settingsObj);
            WindowsMRPackageSettings settings = settingsObj as WindowsMRPackageSettings;

            if (settings == null)
            {
                var assets = AssetDatabase.FindAssets("t:WindowsMRPackageSettings");
                if (assets.Length == 1)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[0]);
                    settings = AssetDatabase.LoadAssetAtPath(path, typeof(WindowsMRPackageSettings)) as WindowsMRPackageSettings;
                    if (settings != null)
                    {
                        EditorBuildSettings.AddConfigObject(BuildSettingsKey, settings, true);
                    }

                }
            }

            return settings;
        }

        public WindowsMRBuildSettings BuildSettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            WindowsMRPackageSettings settings = PackageSettingsForBuildTargetGroup(buildTargetGroup);

            if (settings != null)
            {
                WindowsMRBuildSettings targetSettings = settings.GetBuildSettingsForBuildTargetGroup(buildTargetGroup);
                return targetSettings;
            }

            return null;
        }

        public override UnityEngine.Object SettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            WindowsMRPackageSettings settings = PackageSettingsForBuildTargetGroup(buildTargetGroup);

            if (settings != null)
            {
                WindowsMRSettings targetSettings = settings.GetSettingsForBuildTargetGroup(buildTargetGroup);
                return targetSettings;
            }

            return null;
        }

        const string k_ForcePrimaryWindowHolographic = "force-primary-window-holographic";

        public override void OnPostprocessBuild(BuildReport report)
        {
            base.OnPostprocessBuild(report);
            WindowsMRBuildSettings buildSettings = BuildSettingsForBuildTargetGroup(report.summary.platformGroup);

            if (buildSettings == null)
                return;

            string bootConfigPath = report.summary.outputPath;

            if (report.summary.platformGroup == BuildTargetGroup.WSA)
            {
                // Boot Config data path is highly specific to the platform being built.
                bootConfigPath = Path.Combine(bootConfigPath, PlayerSettings.productName);
                bootConfigPath = Path.Combine(bootConfigPath, "Data");
                bootConfigPath = Path.Combine(bootConfigPath, "boot.config");

                using (StreamWriter sw = File.AppendText(bootConfigPath))
                {
                    sw.WriteLine(String.Format("{0}={1}", k_ForcePrimaryWindowHolographic, buildSettings.UsePrimaryWindowForDisplay ? 1 : 0));
                }
            }
        }
    }
}
