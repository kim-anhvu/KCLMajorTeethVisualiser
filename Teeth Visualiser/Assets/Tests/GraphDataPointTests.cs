using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/*
 * Testing The GraphDataPoint Class
 */

namespace Tests
{
    public class GraphDataPointTests
    {
        GraphDataPoint point;

        [SetUp] 
        public void Init()
        { 
            point = new GraphDataPoint();
        }

        [TearDown] 
        public void Dispose()
        {
            point = null;
        }

        [Test]
        //Adding one data point will increment list
        public void AddDataPointAppendsToList(){
            double valueToAdd = 1;
            point.AddDataPoint(valueToAdd);
            bool expected = true;
            bool output;
            if(point.GetGraphDataPoints().Count > 0){
                output = true;
            }else{
                output = false;
            }

            Assert.AreEqual(expected, output);
        }


        [Test]
        //Appending multiple values
        public void AddDataPointRangeAppendsAllToList(){
            List<double> valuesToAdd = new List<double>(){3,4,5,6};
            point.AddDataPointRange(valuesToAdd);
            bool expected = true;
            bool output;
            if(point.GetGraphDataPoints().Count == valuesToAdd.Count){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }
        
        [Test]
        //Appending multiple values results in correct maximum
        public void AddingDataPointsSetsCorrectMaximum(){
            List<double> valuesToAdd = new List<double>(){3,6,4,5};
            point.AddDataPointRange(valuesToAdd);
            bool expected = true;
            bool output;
            if(point.GetMax() == 6){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //Appending multiple values results in correct min
        public void AddingDataPointsSetsCorrectMin(){
            List<double> valuesToAdd = new List<double>(){3,6,4,5};
            point.AddDataPointRange(valuesToAdd);
            bool expected = true;
            bool output;
            if(point.GetMin() == 3){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //Clear sets the datapoint list to empty.
        public void ClearEmptiesDataPoint(){
            List<double> valuesToAdd = new List<double>(){3,6,4,5};
            point.AddDataPointRange(valuesToAdd);
            point.Clear();
            bool expected = true;
            bool output;
            if(point.GetGraphDataPoints().Count == 0){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //Indexing GraphDataPoint above the hihest index returns a 0 
        public void GetDataPointExceedingBounds(){
            List<double> valuesToAdd = new List<double>(){3,6,4,5};
            point.AddDataPointRange(valuesToAdd);
            bool expected = true;
            bool output;
            if(point.GetPoint(valuesToAdd.Count) == 0){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //Indexing GraphDataPoint < 0 returns 0  
        public void GetDataPointPreceedingBounds(){
            List<double> valuesToAdd = new List<double>(){3,6,4,5};
            point.AddDataPointRange(valuesToAdd);
            bool expected = true;
            bool output;
            if(point.GetPoint(-1) == 0){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

    }
}
