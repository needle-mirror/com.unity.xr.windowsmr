using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace Unity.XR.SDK.TestsTools
{
    [Flags]
    public enum XRDevices
    {
        OCULUS = 1,
        ML = 2,
        WINDOWSMR = 4,
    };

    public class SubsystemManifest
    {
        public struct SubsystemManifestData
        {
            public string name;
            public string version;
            public string libraryName;
            public List<SubsystemEntry> displays;
            public List<SubsystemEntry> inputs;
            public List<SubsystemEntry> experiences;
        }

        public class SubsystemEntry
        {
            public string id;
        }

        public static TextAsset LoadManifestFromResources(string path)
        {
            var manifestText = Resources.Load<TextAsset>(path);
            if (!manifestText)
            {
                throw new FileNotFoundException(path);
            }

            return manifestText;
        }

        public static TextAsset LoadManifestFromAssetDatabase(string path = "default path")
        {
#if UNITY_EDITOR
            if (path == "default path")
            {
                var allpaths = AssetDatabase.GetAllAssetPaths();
                foreach (var targetPath in allpaths)
                {
                    if (targetPath.Contains("Packages/com.unity.xr") &&
                        targetPath.Contains("UnitySubsystemsManifest.json") &&
                        targetPath.Contains("Runtime"))
                    {
                        if (!targetPath.Contains("Tests"))
                        {
                            path = targetPath;
                        }
                    }

                }
            }

            var manifestText = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            if (!manifestText)
            {
                throw new FileNotFoundException(path);
            }
            return manifestText;
#else
            return null;
#endif
        }

        public static SubsystemManifestData GetSubsystemManifest(TextAsset manifestText)
        {

            if (!manifestText)
            {
                throw new System.ArgumentNullException();
            }

            var textData = manifestText.text;
            var manifest = JsonUtility.FromJson<SubsystemManifestData>(textData);

            manifest.displays = ParseList<SubsystemEntry>(textData, "displays");
            manifest.inputs = ParseList<SubsystemEntry>(textData, "inputs");
            manifest.experiences = ParseList<SubsystemEntry>(textData, "experience");

            return manifest;
        }

        internal static List<T> ParseList<T>(string json, string key) where T : SubsystemEntry
        {
            string minified = new Regex("[\\t\\r\\n]").Replace(json, "");
            var regex = new Regex(key + "\"\\s*:\\s*\\[(.*?)\\]");
            MatchCollection matches = regex.Matches(minified);
            if (matches.Count == 0)
                return new List<T>();

            string match = matches[0].Groups[1].Value;    // Group 0 is full match, group 1 is capture group
            if (match.Length == 0)                        // Found empty list {}
                return new List<T>();

            var items = new Regex("([{|}].*?[{|}]|[^[{|}],\\s]+)(?=\\s*.|\\s*$)");
            MatchCollection itemMatches = items.Matches(match);

            var itemMatch = matches[0].Groups[1].Value;    // Group 0 is full match, group 1 is capture group
            if (itemMatch.Length == 0)
                return new List<T>();

            var list = new List<T>();
            for (int i = 0; i < itemMatches.Count; ++i)
            {
                var entry = JsonUtility.FromJson<T>(itemMatches[i].ToString());
                list.Add(entry);
            }

            return list;
        }

        private static Dictionary<string, string> ParseDictionary(string json, string key)
        {
            string minified = new Regex("[\"\\s]").Replace(json, "");
            var regex = new Regex(key + ":{(.*?)}");
            MatchCollection matches = regex.Matches(minified);
            if (matches.Count == 0)
                return new Dictionary<string, string>();

            string match = matches[0].Groups[1].Value;    // Group 0 is full match, group 1 is capture group
            if (match.Length == 0)                        // Found empty dictionary {}
                return new Dictionary<string, string>();

            string[] keyValuePairs = match.Split(',');
            return keyValuePairs.Select(kvp => kvp.Split(':')).ToDictionary(k => k[0], v => v[1]);
        }
    }

    [AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RequireXRDevice : NUnitAttribute, IApplyToTest
    {
        XRDevices m_deviceFlags;

        private Dictionary<XRDevices, string> deviceList = new Dictionary<XRDevices, string>()
        {
            {XRDevices.OCULUS, "OCULUS"},
            {XRDevices.ML, "ML"},
            {XRDevices.WINDOWSMR, "WINDOWSMR"}
        };

        public RequireXRDevice(XRDevices deviceFlags)
        {
            this.m_deviceFlags = deviceFlags;
        }

        public void ApplyToTest(Test test)
        {
            bool deviceIsConnected = false;

            foreach (KeyValuePair<XRDevices, string> entry in deviceList)
            {
                if ((m_deviceFlags & entry.Key) != 0)
                {
                    var envVariable = string.Format("{0}_DEVICE_CONNECTED", entry.Value);
                    if (IsTestDeviceConnected(envVariable))
                    {
                        deviceIsConnected = true;
                    }
                }
            }

            if (!deviceIsConnected)
            {
                test.RunState = RunState.Skipped;
                test.Properties.Add("_SKIPREASON", "No valid XR Device connected, Skipping Test!" );
            }
        }

        public static bool IsTestDeviceConnected(string envVariable)
        {
#if (PLATFORM_LUMIN || PLATFORM_ANDROID) && !UNITY_EDITOR 
            return true;
#else
            return !String.IsNullOrEmpty(Environment.GetEnvironmentVariable(envVariable));
#endif // PLATFORM_LUMIN || PLATFORM_ANDROID && !UNITY_EDITOR 
        }
    }
}