using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class CameraNavigatoLogicTest
    {
        /**
        * Testing the Camemera Navigator Class
        */
        [Test]
        public void TestPositiveIsClick()
        {
            float multiplier = 12345;
            for (int i =0; i< 12345; i++)
            {
                float lastTime = Random.value * multiplier;
                float currentTime = lastTime + 0.24f * Random.value;
                Assert.AreEqual(true,
                    CameraNavigatorLogic.IsClick(lastTime, currentTime));
            }
        }

        [Test]
        public void TestNegativeIsClick()
        {
            float multiplier = 12345;
            for (int i = 0; i < 12345; i++)
            {
                float lastTime = Random.value * multiplier;
                float currentTime = lastTime + 0.25f +1f * Random.value;
                Assert.AreEqual(false,
                    CameraNavigatorLogic.IsClick(lastTime, currentTime));
            }
        }
    }
}