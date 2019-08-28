using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.TestTools;
using UnityEngine.XR;
using Unity.XR.SDK.TestsTools;


namespace XR.SDK.PlayModeTests
{
    [TestFixture]
    public class SubsystemRegistration
    {
        private SubsystemManifest.SubsystemManifestData manifest;

        [SetUp]
        public void Setup()
        {
#if UNITY_EDITOR
            manifest = SubsystemManifest.GetSubsystemManifest(SubsystemManifest.LoadManifestFromAssetDatabase());
#endif
        }

        [Test]
        public void DisplaySubsystemsRegistered()
        {
            if (manifest.displays == null || manifest.displays.Count == 0)
            {
                Assert.Fail("No display subsystems found in the subsystems manifest");
                return;
            }

            foreach (SubsystemManifest.SubsystemEntry displayEntry in manifest.displays)
            {
                Assert.That(SubsystemDescriptorRegistered<XRDisplaySubsystemDescriptor>(displayEntry.id), SubsystemRegisterationAssertMessage(displayEntry.id));
            }

        }

        [Test]
        public void InputSubsystemsRegistered()
        {
            if (manifest.inputs == null || manifest.inputs.Count == 0)
            {
                Assert.Fail("No input subsystems found in the subsystems manifest");
                return;
            }

            foreach (SubsystemManifest.SubsystemEntry inputEntry in manifest.inputs)
            {
                Assert.That(SubsystemDescriptorRegistered<XRInputSubsystemDescriptor>(inputEntry.id),
                    SubsystemRegisterationAssertMessage(inputEntry.id));
            }
        }

        bool SubsystemDescriptorRegistered<T>(string id) where T : ISubsystemDescriptor
        {
            List<T> descriptors = new List<T>();

            SubsystemManager.GetSubsystemDescriptors<T>(descriptors);

            foreach (T descriptor in descriptors)
            {
                if (descriptor.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        string SubsystemRegisterationAssertMessage(string entryId)
        {
            return String.Format("{0} is listed in the UnitySubsystemManifest but has not been registered at runtime.",
                entryId);
        }
    }
}
