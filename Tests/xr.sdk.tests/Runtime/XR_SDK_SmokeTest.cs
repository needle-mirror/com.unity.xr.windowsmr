using System.Collections;
using NUnit.Framework;
using Unity.XR.SDK.TestsTools;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace XR.SDK.PlayModeTests
{
   public class ProviderSmokeTest
    {
        [RequireXRDevice(XRDevices.OCULUS | XRDevices.WINDOWSMR)]
        [UnityTest]
        public IEnumerator CanDeployAndRunOnDevice()
        {
            yield return new MonoBehaviourTest<XrSdkMonoBehaviourTest>();
        }
    }

    public class XrSdkMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        public bool IsTestFinished { get; private set; }

        void Start()
        {
            var displaySubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
            var inputSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRInputSubsystem>();

            Assert.NotNull(displaySubsystem, "Display subsystem not loaded");
            Assert.NotNull(inputSubsystem, "Input subsystem not loaded");

            Assert.IsTrue(displaySubsystem.running, "Display subsystem was not running");
            Assert.IsTrue(inputSubsystem.running, "Input subsystem was not running");

            IsTestFinished = true;
        }
    }
}
