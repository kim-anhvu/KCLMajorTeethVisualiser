using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
 * Testing the SearchQuery Class
 */

namespace Tests
{
    public class SearchQueryTests
    {
        List<Patient> patients;
        SearchQuery search;
        MockDataLoader mock;

        [SetUp] 
        public void Init()
        { 
            mock = new MockDataLoader();
            search = new SearchQuery();
        }

        [TearDown] 
        public void Dispose()
        {
            patients = null;
            search = null;
        }

        //
        [Test]
        public void NoFiltersReturnsAllPatients()
        {
            patients = mock.GetAllMockData();
            bool expected = true;
            bool output = true;
            foreach (Patient patient in patients)
            {
                if(!search.filter(patient)){
                    output = false;
                }
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //All patients with an age >= SetFromAge should be found
        public void AgeFilterGreaterThanOrEqualFromAge()
        {
            List<int> ages = new List<int>(){50,60,70};
            patients = mock.CreateSimpleAgeMockData(ages);

            //Set the from age in the search query
            search.SetFromAge(50);

            bool expected = true;
            bool output = true;
            foreach (Patient patient in patients)
            {
                if(!search.filter(patient)){
                    output = false;     //All patients should be found
                }
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //All patients with an age < SetFromAge should not be found

        public void AgeFilterLessThanFromAge()
        {
            List<int> ages = new List<int>(){50,60,70};
            patients = mock.CreateSimpleAgeMockData(ages);

            //Set the from age in the search query
            search.SetFromAge(71);

            bool expected = true;
            bool output = true;
            foreach (Patient patient in patients)
            {
                if(search.filter(patient)){
                    output = false;     //All patients should not be found
                }
            }
            Assert.AreEqual(expected, output);
        }

         [Test]
        //All patients with an age > SetToAge should not be found
        public void AgeFilterGreaterThanToAge()
        {
            List<int> ages = new List<int>(){50,60,70};
            patients = mock.CreateSimpleAgeMockData(ages);

            //Set the from age in the search query
            search.SetToAge(49);

            bool expected = true;
            bool output = true;
            foreach (Patient patient in patients)
            {
                if(search.filter(patient)){
                    output = false;     //All patients should not be found
                }
            }
            Assert.AreEqual(expected, output);
        }

        [Test]
        //All patients with an age <= SetToAge should  be found

        public void AgeFilterLessThanOrEqualToAge()
        {
            List<int> ages = new List<int>(){50,60,70};
            patients = mock.CreateSimpleAgeMockData(ages);

            //Set the from age in the search query
            search.SetToAge(70);

            bool expected = true;
            bool output = true;
            foreach (Patient patient in patients)
            {
                if(!search.filter(patient)){
                    output = false;     //All patients should  be found
                }
            }
            Assert.AreEqual(expected, output);
        } 

        [Test]
        //An Empty filter should return true
        public void CheckAllEmptyTest(){
            List<string> emptyfilter = new List<string>();
            emptyfilter.Clear();
            Debug.Log("empty.. " + emptyfilter.Count.Equals(0));
            bool expected = true;
            bool output;

            if(search.CheckAll(emptyfilter)){
                output = true;     //True should be returned
            }else{
                output = false; 
                Debug.Log("set to false");

            }
            Assert.AreEqual(expected, output);

        }
        [Test]
        //A filter with 'ALL' selected should return true
        public void CheckAllSetTest(){
            List<string> filter = new List<string>(){"All"};
            bool expected = true;
            bool output = true;

            if(!search.CheckAll(filter)){
                output = false;     //True should be returned
            }
            Assert.AreEqual(expected, output);

        }

        [Test]
        //A filter which isnt empty and not "All" selected should return false
        public void CheckNonEmptyTest()
        {
            List<string> emptyfilter = new List<string>(){"not", "all"};
            bool expected = false;
            bool output = true;

            if(!search.CheckAll(emptyfilter)){
                output = false;     //false should be returned
            }
            Assert.AreEqual(expected, output);

        }

        [Test]
        //Checks if record exists in the filter
        public void CheckRecordEqual()
        {
            string record = "M";
            List<string> filter = new List<string>(){"M", "F"};
            bool expected = true;
            bool output;

            if(search.CheckRecord(record,filter)){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }

        //Checks if record doesnt exists in the filter
        public void CheckRecordNotEqual()
        {
            string record = "NA";
            List<string> filter = new List<string>(){"M", "F"};
            bool expected = false;
            bool output;

            if(search.CheckRecord(record,filter)){
                output = true;
            }else{
                output = false;
            }
            Assert.AreEqual(expected, output);
        }


    }
}
