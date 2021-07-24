using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{

    /**
     * Testing the BoxPlotBackendClass
     */
    public class TestBoxPlot
    {
        //Tests With no outlier position less than q1, the min will be the first value
        [Test]
        public void CalculateMinNotSetReturnsFirstValue()
        {
            double expected = 1;
            double output;

            int lastLowOutlierPosistion = -1;
            double valueAfterOutlier = 10;
            double first = 1;
            
            output = BoxPlotBackend.CalculateMin(lastLowOutlierPosistion,valueAfterOutlier,first);
            
            Assert.AreEqual(expected, output);
        }

        //Tests With outlier position less than q1, the min will be the first value after the outlier
        [Test]
        public void CalculateMinSetReturnsFirstNonOutlierValue()
        {
            double expected = 10;
            double output;

            int lastLowOutlierPosistion = 1; //not set to -1
            double valueAfterOutlier = 10;
            double first = 1;
            
            output = BoxPlotBackend.CalculateMin(lastLowOutlierPosistion,valueAfterOutlier,first);
            
            Assert.AreEqual(expected, output);
        }

        //Tests With outlier position > q3, the max will be the first value before the outlier
        [Test]
        public void CalculateMaxSetReturnsFirstNonOutlierBeforeOutlier()
        {
            double expected = 10;
            double output;

            int firstAboveOutlier = 1; //not set to -1
            double valBeforeOutlier = 10;
            double last = 30;
            
            output = BoxPlotBackend.CalculateMax(firstAboveOutlier,valBeforeOutlier,last);
            
            Assert.AreEqual(expected, output);
        }

        //Tests With not outlier position > q3, the max will be the last value 
        [Test]
        public void CalculateMaxNotSetReturnsLast()
        {
            double expected = 30;
            double output;

            int firstAboveOutlier = -1; //not set 
            double valBeforeOutlier = 10;
            double last = 30;
            
            output = BoxPlotBackend.CalculateMax(firstAboveOutlier,valBeforeOutlier,last);
            
            Assert.AreEqual(expected, output);
        }
        
        //Max integration Test
        [Test]
        public void CalculateMaxIntegration()
        {
            List<double> values = new List<double>(){1,2,3,4,5,70}; 
            double expected = 5; //70 is an outlier
            double output;
            
            output = BoxPlotBackend.BoxPlot(values)[4];
            values = null;
            Assert.AreEqual(expected, output);
        }
        //Min integration Test
        [Test]
        public void CalculateMinIntegration()
        {
            List<double> values = new List<double>(){1,30,32,33,34,35}; 
            double expected = 30; 
            double output;
            output = BoxPlotBackend.BoxPlot(values)[0];
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //max integration Test
        [Test]
        public void CalculateMaxIntegrationWithoutOutliers()
        {
            List<double> values = new List<double>(){1,2,3,4,5}; 
            double expected = 5; 
            double output;
            output = BoxPlotBackend.BoxPlot(values)[4];
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //q2 integration Test
        [Test]
        public void CalculateQ2Integration()
        {
            List<double> values = new List<double>(){1,2,3,4,5}; 
            double expected = 3; 
            double output;
            output = BoxPlotBackend.BoxPlot(values)[2];
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //q1 integration Test
        [Test]
        public void CalculateQ1Integration()
        {
            List<double> values = new List<double>(){1,2,3,4,5}; 
            double expected = (2+1)/2.0; 
            double output;
            output = BoxPlotBackend.BoxPlot(values)[1];
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //q3 integration Test
        [Test]
        public void CalculateQ3Integration()
        {
            List<double> values = new List<double>(){1,2,3,4,5}; 
            double expected = (4+3)/2.0; 
            double output;
            output = BoxPlotBackend.BoxPlot(values)[3];
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //Radiation Count less than 5 returns null
        [Test]
        public void UnableToCreateBoxPlotForLessThan5Values()
        {
            List<double> values = new List<double>(){1,2,3,4}; 
            List<double> expected = null; 
            List<double> output;
            output = BoxPlotBackend.BoxPlot(values);
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //Correct Posistion Of Outliers for upper bound
        [Test]
        public void OutlierUpperBoundPosistionFound()
        {
            List<double> values = new List<double>(){1,2,2,2,3,70}; 
            int expected = 5; 
            int output;
            List<double> boxplot = BoxPlotBackend.BoxPlot(values);
            double q1 = boxplot[1];
            double q3 = boxplot[3];
            output = BoxPlotBackend.CalculateOutliersPosistions(q1,q3,values.Count,values)[0];       //first outlier
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //Correct Posistion Of Outliers for lower bound
        [Test]
        public void OutlierUpperLowerPosistionFound()
        {
            List<double> values = new List<double>(){1,71,72,72,72,73}; 
            int expected = 0; 
            int output;
            List<double> boxplot = BoxPlotBackend.BoxPlot(values);
            double q1 = boxplot[1];
            double q3 = boxplot[3];
            output = BoxPlotBackend.CalculateOutliersPosistions(q1,q3,values.Count,values)[0];       //first outlier
            
            values = null;
            Assert.AreEqual(expected, output);
        }

        //Gets Correct Teeth Radiations for >0
        [Test]
        public void AggregatedTeethRadiationListIsCorrectForPostiveValue()
        {
            double rad = 5;
            MockDataLoader mocking = new MockDataLoader();
            Patient patient = mocking.CreateSimpleRadiationMockData(rad)[0];
            List<int> teethSelected = new List<int>(){1,2,3};   //for 3 selected teeth
            List<double> expected = new List<double>(){rad,rad,rad};
            List<double> output = BoxPlotBackend.GetAllTeethRadiations(patient, 0, teethSelected );
            
            Assert.AreEqual(expected, output);
        }

        //Gets Correct Teeth Radiations for =0
        [Test]
        public void AggregatedTeethRadiationWithZerosShouldReturnAnEmptyList()
        {
            double rad = 5;
            MockDataLoader mocking = new MockDataLoader();
            Patient patient = mocking.CreateSimpleRadiationMockData(0)[0];
            List<int> teethSelected = new List<int>(){1,2,3};   //for 3 selected teeth
            int expected = 0;
            int output = BoxPlotBackend.GetAllTeethRadiations(patient, 0, teethSelected ).Count;
            
            Assert.AreEqual(expected, output);
        }

        //Sorts Radiation List Into Genders
    }
}
