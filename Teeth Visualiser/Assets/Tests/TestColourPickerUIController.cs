using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System;

namespace Tests
{
    /**
     * Testing the ColourPickerUI Controller Class
     */

    public class TestColourPickerUIController
    {

        ColourPickerUIController colourPickerController;
        List<Color> colours;
        double[] topTeeth;
        double[] bottomTeeth;

        [SetUp]
        public void Init()
        {
            colourPickerController = new ColourPickerUIController();
        }

        [TearDown]
        public void Dispose()
        {
            colourPickerController = null;
        }

        private void resetColourPickerUIController()
        {
            colourPickerController = new ColourPickerUIController();
        }

        public void setUpColourPickerControllerWithData()
        {
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);
            colourPickerController.setSelectedTeethColours(colours);

            topTeeth = new double[16] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
            bottomTeeth = new double[16] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
            colourPickerController.setInputAndOutputRadiation(1, 13);
            colourPickerController.setAverageMeanRadiation(bottomTeeth, topTeeth);
        }

        public void setUpSecondSetOfColoursNoOverLap() {
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);
            colourPickerController.setSelectedTeethColours(colours);
            colourPickerController.setInputAndOutputRadiation(14, 16);

        }

        public void setUpSecondSetOfColoursOverLap()
        {
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);
            colourPickerController.setSelectedTeethColours(colours);
            colourPickerController.setInputAndOutputRadiation(12, 16);

        }

        [Test]
        public void CreateColourMapGivesStateWithMockData()
        {
            setUpColourPickerControllerWithData();
            colourPickerController.CreateColourMaps();
            List<KeyValuePair<float, Color>> RadColourMapToTest = colourPickerController.getRadColourMap();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
                //Debug.Log("Test " + RadColourMapToTest[i].Value + "Mine " + myRadColourMap[i].Key);
            }
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                Assert.AreEqual(myRadColourMap[i].Key, RadColourMapToTest[i].Key);

            }
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 1);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, myRadColourMap.Count);
        }

        [Test]
        public void CreateColourMapGivesStateWithMockDataTwoMapsNoOverLap()
        {
            setUpColourPickerControllerWithData();
            colourPickerController.CreateColourMaps();
            List<KeyValuePair<float, Color>> RadColourMapToTest = colourPickerController.getRadColourMap();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
                //Debug.Log("Test " + RadColourMapToTest[i].Value + "Mine " + myRadColourMap[i].Key);
            }
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                Assert.AreEqual(myRadColourMap[i].Key, RadColourMapToTest[i].Key);

            }
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 1);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, myRadColourMap.Count);
            setUpSecondSetOfColoursNoOverLap();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 2);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, 26);

        }

        [Test]
        public void CreateColourMapGivesStateWithMockDataTwoMapsOverLap()
        {
            setUpColourPickerControllerWithData();
            colourPickerController.CreateColourMaps();
            List<KeyValuePair<float, Color>> RadColourMapToTest = colourPickerController.getRadColourMap();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
                //Debug.Log("Test " + RadColourMapToTest[i].Value + "Mine " + myRadColourMap[i].Key);
            }
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                Assert.AreEqual(myRadColourMap[i].Key, RadColourMapToTest[i].Key);

            }
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 1);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, myRadColourMap.Count);
            setUpSecondSetOfColoursOverLap();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 1);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, 13);

        }


        [Test]
        public void CreateColourMapThrowsExceptionWithNoData()
        {
            resetColourPickerUIController();
            //colourPickerController.CreateColourMaps();
            var ex = Assert.Throws<Exception>(() => colourPickerController.CreateColourMaps());
            Assert.That(ex.Message, Is.EqualTo("From or To value not set"));
        }

        [Test]
        public void createColourMapThrowsExceptionWithNoColours(){
            resetColourPickerUIController();
            colourPickerController.setInputAndOutputRadiation(1,10);
            var ex = Assert.Throws<Exception>(() => colourPickerController.CreateColourMaps());
            Assert.That(ex.Message, Is.EqualTo("No colours have been selected"));
        }

        [Test]
        public void createColourMapFinishesWithoutRadiationArrays() {
            resetColourPickerUIController();
            colours = new List<Color>();
            colours.Add(Color.red);
            colours.Add(Color.blue);
            colours.Add(Color.green);
            colourPickerController.setSelectedTeethColours(colours);
            colourPickerController.setInputAndOutputRadiation(1, 13);
            colourPickerController.CreateColourMaps();
            List<KeyValuePair<float, Color>> RadColourMapToTest = colourPickerController.getRadColourMap();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
            }
            for (int i = 0; i < RadColourMapToTest.Count; ++i)
            {
                Assert.AreEqual(myRadColourMap[i].Key, RadColourMapToTest[i].Key);

            }
            Assert.AreEqual(colourPickerController.getFromToRadiationAndColourList().Count, 1);
            Assert.AreEqual(colourPickerController.getRadColourMap().Count, myRadColourMap.Count);
        }

        [Test]
        public void paintBottomTeethReturnNullForEmptyRadiationArray(){
            resetColourPickerUIController();
            Assert.AreEqual(colourPickerController.paintBottomTeeth(), null);
        }


        [Test]
        public void paintBottomTeethThrowsExceptionIfRadColourMapIsNotSet()
        {
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            var ex = Assert.Throws<Exception>(() => colourPickerController.paintBottomTeeth());
            Assert.That(ex.Message, Is.EqualTo("Can't paint teeth with empty map"));
        }

        [Test]
        public void paintBottomTeethProducesAsetOfWhiteTeethWithNoToFromList()
        {
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i < 13; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
                //Debug.Log("Test " + RadColourMapToTest[i].Value + "Mine " + myRadColourMap[i].Key);
            }
            List<Tuple<int, Color, string>> coloursForTeeth = new List<Tuple<int, Color, string>>();
            for (int i = 0; i < 16; ++i)
            {
                coloursForTeeth.Add(new Tuple<int, Color, string>(i, Color.white, "bottom"));
            }
            colourPickerController.setRadiationMap(myRadColourMap);
            Assert.AreEqual(colourPickerController.paintBottomTeeth(), coloursForTeeth);
        }

        [Test]
        public void paintBottomTeethProducesCorrectList()
        {
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.paintBottomTeeth().Count, 16);
            setUpSecondSetOfColoursNoOverLap();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.paintBottomTeeth().Count, 16);
        }
    

    [Test]
        public void paintTopTeethReturnNullForEmptyRadiationArray()
        {
            resetColourPickerUIController();
            Assert.AreEqual(colourPickerController.paintTopTeeth(), null);
        }

        [Test]
        public void paintTopTeethThrowsExceptionIfRadColourMapIsNotSet(){
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            var ex = Assert.Throws<Exception>(() => colourPickerController.paintTopTeeth());
            Assert.That(ex.Message, Is.EqualTo("Can't paint teeth with empty map"));
        }

        [Test]
        public void paintTopTeethProducesAsetOfWhiteTeethWithNoToFromList()
        {
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            List<KeyValuePair<float, Color>> myRadColourMap = new List<KeyValuePair<float, Color>>();
            for (int i = 0; i <13; ++i)
            {
                myRadColourMap.Add(new KeyValuePair<float, Color>(i + 1, Color.black));
                //Debug.Log("Test " + RadColourMapToTest[i].Value + "Mine " + myRadColourMap[i].Key);
            }
            List<Tuple<int, Color, string>> coloursForTeeth = new List<Tuple<int, Color, string>>();
            for (int i =0; i < 16; ++i) {
                coloursForTeeth.Add(new Tuple<int, Color, string>(i, Color.white, "top"));
            }
            colourPickerController.setRadiationMap(myRadColourMap);
            Assert.AreEqual(colourPickerController.paintTopTeeth(),coloursForTeeth );
        }

        [Test]
        public void paintTopTeethProducesCorrectList(){
            resetColourPickerUIController();
            setUpColourPickerControllerWithData();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.paintTopTeeth().Count, 16);
            setUpSecondSetOfColoursNoOverLap();
            colourPickerController.CreateColourMaps();
            Assert.AreEqual(colourPickerController.paintTopTeeth().Count, 16);
        }

    }
}