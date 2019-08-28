using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.XR.SDK.TestsTools;

namespace XRSDK.EditModeTests
{
    public class ValidateManifest
    {
        [Test]
        public void ValidateUnitySubsystemManifest()
        {
            Assert.DoesNotThrow(() => SubsystemManifest.GetSubsystemManifest(SubsystemManifest.LoadManifestFromAssetDatabase()), "UnitySubsystemManifest was invalid.");
        }
    }

    //Test the tests! Checks that the json parser will return expected values given various json inputs
    public class XrSdkTestUtilitiesTests
    {
        private const string kValidUnitySubsystemManifestResourcePath =
            "TestSetup/ValidUnitySubsystemsManifest";

        private const string kInvalidUnitySubsystemManifestResourcePath =
            "TestSetup/InvalidUnitySubsystemsManifest";

        private const string kTrickyUnitySubsystemManifestResourcePath =
            "TestSetup/TrickyUnitySubsystemsManifest";

        private const string kNonExistantUnitySubsystemManifestPath =
            "TestSetup/IDontExistUnitySubsystemsManifest";

        [Test]
        public void ValidManifestReturnsExpectedValues()
        {
            var manifest = SubsystemManifest.GetSubsystemManifest(SubsystemManifest.LoadManifestFromResources(kValidUnitySubsystemManifestResourcePath));
            Assert.AreEqual("Name",manifest.name);
            Assert.AreEqual("Version", manifest.version);
            Assert.AreEqual("LibraryName", manifest.libraryName);
            Assert.AreEqual("Display", manifest.displays[0].id);
            Assert.AreEqual("Input", manifest.inputs[0].id);
        }

        [Test]
        public void InvalidManifestThrowsJsonParseErrorInSubsystemListParseError()
        {
            Assert.Throws<System.ArgumentException> (() => SubsystemManifest.GetSubsystemManifest(SubsystemManifest.LoadManifestFromResources(kInvalidUnitySubsystemManifestResourcePath)));
        }

        [Test]
        public void NonExistantSubsystemManifestThrowsFileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() =>
                SubsystemManifest.LoadManifestFromResources(kNonExistantUnitySubsystemManifestPath));
            Assert.Throws<FileNotFoundException>(() =>
                SubsystemManifest.LoadManifestFromAssetDatabase(kNonExistantUnitySubsystemManifestPath));
            Assert.Throws<ArgumentNullException>(() =>
                SubsystemManifest.GetSubsystemManifest(null));
        }

        [Test]
        public void TrickySubsystemManifestReturnsExpectedValues()
        {
            var manifest = SubsystemManifest.GetSubsystemManifest(SubsystemManifest.LoadManifestFromResources(kTrickyUnitySubsystemManifestResourcePath));
            Assert.AreEqual("Name", manifest.name);
            Assert.AreEqual("Version", manifest.version);
            Assert.AreEqual("LibraryName", manifest.libraryName);
            Assert.AreEqual("Display0", manifest.displays[0].id);
            Assert.AreEqual("Display1", manifest.displays[1].id);
            Assert.AreEqual("Input0", manifest.inputs[0].id);
            Assert.AreEqual("Input1", manifest.inputs[1].id);
            Assert.AreEqual("Input2", manifest.inputs[2].id);
            Assert.AreEqual("Experience0", manifest.experiences[0].id);
            Assert.AreEqual("Experience1", manifest.experiences[1].id);
            Assert.AreEqual("Experience2", manifest.experiences[2].id);
            Assert.AreEqual("Experience3", manifest.experiences[3].id);
            Assert.AreEqual("Experience4", manifest.experiences[4].id);
        }
    }

}
