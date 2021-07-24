using NUnit.Framework;
using UnityEngine;

namespace Tests{

    /**
    * Testing the Logic of the ThirdPersonCamera
    */
    public class ThirdPersonCameraLogicTest
    {
        [Test]
        public void TestFullHDScreenWidthNoSplitScreen()
        {
            Assert.LessOrEqual(
                ThirdPersonLogic.GetScreenSizeRatio(1080f, false), 8f);
        }

        [Test]
        public void TestMacBookProScreenWidthNoSplitScreen()
        {
            Assert.LessOrEqual(
                ThirdPersonLogic.GetScreenSizeRatio(1600f, false), 13f);
        }

        [Test]
        public void TestFullHDScreenWidthSplitScreen()
        {
            Assert.LessOrEqual(
                ThirdPersonLogic.GetScreenSizeRatio(1080f, true), 2f);
        }

        [Test]
        public void TestMacBookProScreenWidthSplitScreen()
        {
            Assert.LessOrEqual(
                ThirdPersonLogic.GetScreenSizeRatio(1600f, true), 4.5);
        }

        [Test]
        public void TestUpdateCurrentYInBounds()
        {
            for(float f=-88.99f; f<88.99; f += 0.01f)
            {
                Assert.AreEqual(f, ThirdPersonLogic.UpdateCurrentY(f));
            }
        }

        [Test]
        public void TestUpdateCurrentYUnderLowerBound()
        {
            float min = -89f;
            for(float f = -180f; f < min; f += 0.01f)
            {
                Assert.AreEqual(min, ThirdPersonLogic.UpdateCurrentY(f));
            }
        }

        [Test]
        public void TestUpdateCurrentYOverUpperBound()
        {
            float max = 89f;
            for (float f = 180f; f > max; f -= 0.01f)
            {
                Assert.AreEqual(max, ThirdPersonLogic.UpdateCurrentY(f));
            }
        }

        [Test]
        public void TestPositiveUpdateMinDist()
        {
            float multiplier = 12345f;
            for(int i = 0; i < 10; i++)
            {
                float min = Random.value * multiplier ;
                float cur = min + 1f;
                Assert.AreEqual(cur, ThirdPersonLogic.UpdateMinDist(cur, min));
            }
        }

        [Test]
        public void TestNegativeUpdateMinDist()
        {
            float multiplier = 12345f;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                float min = Random.value * multiplier;
                float cur = min - 1f;
                Assert.AreEqual(min, ThirdPersonLogic.UpdateMinDist(cur, min));
            }
        }

        [Test]
        public void TestPositiveUpdateMaxDist()
        {
            float multiplier = 12345f;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                float max = Random.value * multiplier;
                float cur = max - 1f;
                Assert.AreEqual(cur, ThirdPersonLogic.UpdateMaxDist(cur, max));
            }
        }

        [Test]
        public void TestNegativeUpdateMaxDist()
        {
            float multiplier = 12345f;
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                float max = Random.value * multiplier;
                float cur = max + 1f;
                Assert.AreEqual(max, ThirdPersonLogic.UpdateMaxDist(cur, max));
            }
        }

    }
}
