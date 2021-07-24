using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GraphAxisSorterTest
    {
        List<Tuple<Patient, double>> patientsRadiationPairs; 
        GraphAxisSorter sorter;
        MockDataLoader mock;

        [SetUp] 
        public void Init()
        { 
            mock = new MockDataLoader();
            sorter = new GraphAxisSorter();
            //To test the grouping, this small list will be sufficient - all the attribute values of the first 3 patients have been considerted
            List<Patient> abbreviatedPatientList = new List<Patient>{mock.GetAllMockData()[0], mock.GetAllMockData()[1], mock.GetAllMockData()[2]};

            GraphDataFilter filterer = new GraphDataFilter(abbreviatedPatientList);
            //Contains all of the patients from the mock data object, not filtered in any way, of the first tooth for all of the patients.
            patientsRadiationPairs = filterer.GetRadiationList(abbreviatedPatientList, 1, new List<int>{0});
        }

        [TearDown] 
        public void Dispose()
        {
            patientsRadiationPairs = null;
            sorter = null;
        }

        //Set xVariable to "Patient", and check that the returned List of Tuple<string, double> starts with the string of "0", ends with the string of the length of the patientsRadiationPairs;
        //Also, each tuple value matches the patientRadiationPairs values
        [Test]
        public void SortIntoPatientsSuccessful()
        {
            sorter.SetXVariable("Patient");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("0", outcome[0].Item1);
            int lastIndex = patientsRadiationPairs.Count - 1;
            Assert.AreEqual(lastIndex.ToString(), outcome[lastIndex].Item1);
            for(int i = 0; i <= lastIndex; i ++)
            {
                Assert.AreEqual(patientsRadiationPairs[i].Item2, outcome[i].Item2);
            }
        }

        //Set xVariable to "Gender", and check that all the genders of the patients are the same as the genders in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each gender matches up, in the order they are supposed to;
        [Test]
        public void SortIntoGendersSuccessful()
        {
            sorter.SetXVariable("Gender");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("M", outcome[0].Item1);
            Assert.AreEqual("F", outcome[1].Item1);

            Assert.AreEqual(2, outcome.Count);

            Assert.AreEqual(patientsRadiationPairs[0].Item2, outcome[0].Item2);
            Assert.AreEqual((patientsRadiationPairs[1].Item2 + patientsRadiationPairs[2].Item2) / 2, outcome[1].Item2);
        }

        //Set xVariable to "Site", and check that all the sites of the patients are the same as the sites in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each site matches up, in the order they are supposed to.
        [Test]
        public void SortIntoSitesSuccessful()
        {
            sorter.SetXVariable("Site");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("BOT", outcome[0].Item1);
            Assert.AreEqual("TONSIL", outcome[1].Item1);
            Assert.AreEqual("OP-OTHER", outcome[2].Item1);
            
            Assert.AreEqual(3, outcome.Count);

            Assert.AreEqual(patientsRadiationPairs[2].Item2, outcome[0].Item2);
            Assert.AreEqual(patientsRadiationPairs[1].Item2, outcome[1].Item2);
            Assert.AreEqual(patientsRadiationPairs[0].Item2, outcome[2].Item2);
        }

        //Set xVariable to "Treatment", and check that all the treatments of the patients are the same as the treatments in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each treatment matches up, in the order they are supposed to.
        [Test]
        public void SortIntoTreatmentsSuccessful()
        {
            sorter.SetXVariable("Treatment");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("CRT", outcome[0].Item1);
            Assert.AreEqual("RT", outcome[1].Item1);
            
            Assert.AreEqual(2, outcome.Count);

            Assert.AreEqual((patientsRadiationPairs[0].Item2 + patientsRadiationPairs[2].Item2) / 2, outcome[0].Item2);
            Assert.AreEqual(patientsRadiationPairs[1].Item2, outcome[1].Item2);
        }

        //Set xVariable to "Tumour", and check that all the tumours of the patients are the same as the tumours in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each tumour matches up, in the order they are supposed to.
        [Test]
        public void SortIntoTumoursSuccessful()
        {
            sorter.SetXVariable("Tumour");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("T1", outcome[0].Item1);

            Assert.AreEqual(1, outcome.Count);

            Assert.AreEqual((patientsRadiationPairs[0].Item2 + patientsRadiationPairs[1].Item2 + patientsRadiationPairs[2].Item2) / 3, outcome[0].Item2);
        }

        //Set xVariable to "Jaw Side", and check that all the sides of the patients are the same as the sides in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each side matches up, in the order they are supposed to.
        [Test]
        public void SortIntoSideSuccessful()
        {
            sorter.SetXVariable("Jaw Side");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("LEFT", outcome[0].Item1);
            Assert.AreEqual("N/A", outcome[1].Item1);

            Assert.AreEqual(2, outcome.Count);

            Assert.AreEqual((patientsRadiationPairs[1].Item2 + patientsRadiationPairs[2].Item2) / 2, outcome[0].Item2);
            Assert.AreEqual(patientsRadiationPairs[0].Item2, outcome[1].Item2);
        }

        //Set xVariable to "Total RT", and check that all the totalRTs of the patients are the same as the TotalRTs in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each totalRT matches up, in the order they are supposed to.
        [Test]
        public void SortIntoTotalRTSuccessful()
        {
            sorter.SetXVariable("Total RT");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("65/30", outcome[0].Item1);

            Assert.AreEqual(1, outcome.Count);

            Assert.AreEqual((patientsRadiationPairs[0].Item2 + patientsRadiationPairs[1].Item2 + patientsRadiationPairs[2].Item2) / 3, outcome[0].Item2);
        }

        //Set xVariable to "Nodal", and check that all the nodals of the patients are the same as the nodals in the returned List of Tuple<string, double>;
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each nodal matches up, in the order they are supposed to.
        [Test]
        public void SortIntoNodalsSuccessful()
        {
            sorter.SetXVariable("Nodal");
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("N0", outcome[0].Item1);
            Assert.AreEqual("N1", outcome[1].Item1);

            Assert.AreEqual(2, outcome.Count);

            Assert.AreEqual((patientsRadiationPairs[0].Item2 + patientsRadiationPairs[1].Item2) / 2, outcome[0].Item2);
            Assert.AreEqual(patientsRadiationPairs[2].Item2, outcome[1].Item2);
        }

        //Set xVariable to "Age", and check that all the ages of the patients are the same as the ages in the returned List of Tuple<string, double>;
        //Set the age range to 10, and check that all the patients are in the correct age range, and any age ranges which have no patients are not present
        //Check that the size of the outcome is exactly as the number of things for which there should be values;
        //Also, the radiation values given to each nodal matches up, in the order they are supposed to.
        [Test]
        public void SortIntoAgesSuccessful()
        {
            sorter.SetXVariable("Age");
            sorter.SetAgeRange(10);
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual("50 - 59", outcome[0].Item1);
            Assert.AreEqual("70 - 79", outcome[1].Item1);
            Assert.AreEqual("80 - 89", outcome[2].Item1);

            Assert.AreEqual(3, outcome.Count);

            Assert.AreEqual(patientsRadiationPairs[1].Item2, outcome[0].Item2);
            Assert.AreEqual(patientsRadiationPairs[2].Item2, outcome[1].Item2);
            Assert.AreEqual(patientsRadiationPairs[0].Item2, outcome[2].Item2);
        }


        //After setting the xVariable so that it corresponds to none of the sorting methods, return an empty List of Tuple<string, double>
        [Test]
        public void ReturnEmptyListIfXVariableNotProperlySet()
        {
            sorter.SetXVariable("Random");
            List<Tuple<string, double>> empty = new List<Tuple<string, double>>();
            List<Tuple<string, double>> outcome = sorter.SortIntoXVariables(patientsRadiationPairs);
            Assert.AreEqual(empty, outcome);
        }

        //Getter for XVariable - returns XVariable unchanged after it was set to its default - "Patients"
        [Test]
        public void GetXVariableSuccessfully()
        {
            Assert.AreEqual("Patient", sorter.GetXvariable());
        }

        //Getter for ageRange - returns ageRange unchanged after it was set to its default - 1
        [Test]
        public void GetAgeRangeSuccessfully()
        {
            Assert.AreEqual(1, sorter.GetAgeRange());
        }

        //Setter for XVariable - successfully updates
        [Test]
        public void SetXVariableSuccessfully()
        {
            sorter.SetXVariable("testing");
            Assert.AreEqual("testing", sorter.GetXvariable());
        }

        //Setter for ageRange - successfully updates
        [Test]
        public void SetAgeRangeSuccessfully()
        {
            sorter.SetAgeRange(10);
            Assert.AreEqual(10, sorter.GetAgeRange());
        }
    }
}
