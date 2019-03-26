using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Unity.XR.WindowsMR.Tests
{
    internal class ExperienceSubsystemTests : TestBaseSetup
    {
        [UnityTest]
        public IEnumerator TestExperienceSubsystem()
        {
            yield return new WaitForSeconds(1);
            Assert.IsNotNull(m_XrManager);
            XRExperienceSubsystem expSub = XRManager.activeLoader.GetLoadedSubsystem<XRExperienceSubsystem>();
            Assert.IsNotNull(expSub);
        }

        [UnityTest]
        public IEnumerator TestExperienceType()
        {
            yield return new WaitForSeconds(1);
            XRExperienceSubsystem expSub = XRManager.activeLoader.GetLoadedSubsystem<XRExperienceSubsystem>();
            switch(expSub.experienceType)
            {
                case XRExperienceSubsystem.ExperienceType.UnBounded:
                    Assert.Fail("Should not be testing on an unbounded device");
                    break;
                default:
                    break;
            }
        }

        [UnityTest]
        public IEnumerator TestExperienceBoundary()
        {
            yield return new WaitForSeconds(1);
            XRExperienceSubsystem expSub = XRManager.activeLoader.GetLoadedSubsystem<XRExperienceSubsystem>();
            if (expSub.experienceType == XRExperienceSubsystem.ExperienceType.Bounded)
            {
                List<Vector3> boundaryPoints = new List<Vector3>();
                expSub.GetAllBoundaryPoints(boundaryPoints);
                Assert.IsFalse(boundaryPoints.Count == 0);
            }
        }
    }

}
