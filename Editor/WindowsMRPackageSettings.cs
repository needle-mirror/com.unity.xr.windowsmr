using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Management;

using UnityEditor;

using Unity.XR.WindowsMR;

namespace Unity.XR.WindowsMR.Editor
{
    [System.Serializable]
    [XRConfigurationData("Windows Mixed Reality", Constants.k_SettingsKey)]
    public class WindowsMRPackageSettings : ScriptableObject, ISerializationCallbackReceiver, IWindowsMRPackageSettings
    {
        [SerializeField]
        List<BuildTargetGroup> Keys = new List<BuildTargetGroup>();
        [SerializeField]
        List<WindowsMRSettings> Values = new List<WindowsMRSettings>();
        [SerializeField]
        List<WindowsMRBuildSettings> BuildValues = new List<WindowsMRBuildSettings>();

        Dictionary<BuildTargetGroup, WindowsMRSettings> Settings = new Dictionary<BuildTargetGroup, WindowsMRSettings>();
        Dictionary<BuildTargetGroup, WindowsMRBuildSettings> BuildSettings = new Dictionary<BuildTargetGroup, WindowsMRBuildSettings>();

        public WindowsMRSettings GetActiveBuildTargetSettings()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(buildTarget);
            return GetSettingsForBuildTargetGroup(group);
        }

        public WindowsMRBuildSettings GetBuildSettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            WindowsMRBuildSettings ret = null;
            BuildSettings.TryGetValue(buildTargetGroup, out ret);
            if (ret == null)
            {
                ret = ScriptableObject.CreateInstance<WindowsMRBuildSettings>() as WindowsMRBuildSettings;
                if (BuildSettings.ContainsKey(buildTargetGroup))
                {
                    Debug.LogWarning("We think you have a settings object for the current build target but we can't find it. We are recreating the settings instance.");
                    BuildSettings[buildTargetGroup] = ret;
                }
                else
                {
                    BuildSettings.Add(buildTargetGroup, ret);
                }
                AssetDatabase.AddObjectToAsset(ret, this);
            }
            return ret;
        }

        public WindowsMRSettings GetSettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            WindowsMRSettings ret = null;
            Settings.TryGetValue(buildTargetGroup, out ret);
            if (ret == null)
            {
                ret = ScriptableObject.CreateInstance<WindowsMRSettings>() as WindowsMRSettings;
                if (Settings.ContainsKey(buildTargetGroup))
                {
                    Debug.LogWarning("We think you have a settings object for the current build target but we can't find it. We are recreating the settings instance.");
                    Settings[buildTargetGroup] = ret;
                }
                else
                {
                    Settings.Add(buildTargetGroup, ret);
                }
                AssetDatabase.AddObjectToAsset(ret, this);
            }
            return ret;
        }

        public void OnBeforeSerialize()
        {
            Keys.Clear();
            Values.Clear();
            BuildValues.Clear();

            foreach (var kv in Settings)
            {
                Keys.Add(kv.Key);
                Values.Add(kv.Value);
            }

            foreach (var kv in BuildSettings)
            {
                BuildValues.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Settings = new Dictionary<BuildTargetGroup, WindowsMRSettings>();
            for (int i = 0; i < Math.Min(Keys.Count, Values.Count); i++)
            {
                Settings.Add(Keys[i], Values[i]);
            }

            BuildSettings = new Dictionary<BuildTargetGroup, WindowsMRBuildSettings>();
            for (int i = 0; i < Math.Min(Keys.Count, BuildValues.Count); i++)
            {
                BuildSettings.Add(Keys[i], BuildValues[i]);
            }
        }
    }

}
