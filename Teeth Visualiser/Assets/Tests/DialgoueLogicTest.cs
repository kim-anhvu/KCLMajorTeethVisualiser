using NUnit.Framework;
using UnityEngine;

namespace Tests 
{   
    /**
     * Testing the Dialgoue (Tutorial) class logic
     */
    public class DialgoueLogicTest 
    {

        [Test]
        public void TestIsRunningCoroutinesEmptyArray()
        {
            bool[] cr_status = { };
            Assert.AreEqual(false, DialogueLogic.IsRunningCoroutines(cr_status));
        }

        [Test]
        public void TestIsRunningCoroutinesAllFalse()
        {
            bool[] cr_status = new bool[12345];
            for(int i = 0; i< 12345; i++)
            {
                cr_status[i] = false;
            }
            Assert.AreEqual(false, DialogueLogic.IsRunningCoroutines(cr_status));
        }

        [Test]
        public void TestIsRunningCoroutinesAllTrue()
        {
            bool[] cr_status = new bool[12345];
            for (int i = 0; i < 12345; i++)
            {
                cr_status[i] = true;
            }
            Assert.AreEqual(true, DialogueLogic.IsRunningCoroutines(cr_status));
        }

        [Test]
        public void TestIsRunningCoroutinesOneTrue()
        {
            bool[] cr_status = new bool[12345];
            for (int i = 0; i < 12345; i++)
            {
                cr_status[i] = false;
            }
            int pos = (int)Random.value * 12345 - 1;
            if (pos < 0) pos = 0;
            cr_status[pos] = true;
            Assert.AreEqual(true, DialogueLogic.IsRunningCoroutines(cr_status));
        }

        [Test]
        public void TestIsRunningCoroutinesOneFalse()
        {
            bool[] cr_status = new bool[12345];
            for (int i = 0; i < 12345; i++)
            {
                cr_status[i] = true;
            }
            int pos = (int)Random.value * 12345 - 1;
            if (pos < 0) pos = 0;
            cr_status[pos] = false;
            Assert.AreEqual(true, DialogueLogic.IsRunningCoroutines(cr_status));
        }
    }
}