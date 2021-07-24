using UnityEngine;
using NUnit.Framework;

namespace Tests{

    /**
     * Testing the movement of the teeth
     */
    public class MoveTeethLogicTest 
    {
        
        [Test]
        public void TestPositiveUpdateRotMin()
        {
            float multiplier = 12345f;
            for (int i = 0; i < 10; i++)
            {
                float min = Random.value * multiplier;
                float rot = min + 1f;
                Assert.AreEqual(rot, MoveTeethLogic.UpdateRotMin(rot, min));
            }
        }

        [Test]
        public void TestNegativeUpdateRotMin()
        {
            float multiplier = 12345f;
            for (int i = 0; i < 10; i++)
            {
                float min = Random.value * multiplier;
                float rot = min - 1f;
                Assert.AreEqual(min, MoveTeethLogic.UpdateRotMin(rot, min));
            }
        }

        [Test]
        public void TestPositiveUpdateRotMax()
        {
            float multiplier = 12345f;
            for (int i = 0; i < 10; i++)
            {
                float max = Random.value * multiplier;
                float rot = max - 1f;
                Assert.AreEqual(rot, MoveTeethLogic.UpdateRotMax(rot, max));
            }
        }

        [Test]
        public void TestNegativeUpdateRotMax()
        {
            float multiplier = 12345f;
            for (int i = 0; i < 10; i++)
            {
                float max = Random.value * multiplier;
                float rot = max + 1f;
                Assert.AreEqual(max, MoveTeethLogic.UpdateRotMax(rot, max));
            }
        }
    }
}