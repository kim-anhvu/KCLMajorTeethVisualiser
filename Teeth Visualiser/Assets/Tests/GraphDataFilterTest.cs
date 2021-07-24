using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GraphDataFilterTest
    {
        List<Patient> allPatients;
        GraphDataFilter filterer;
        MockDataLoader mock;

        [SetUp] 
        public void Init()
        { 
            mock = new MockDataLoader();
            allPatients = mock.GetAllMockData();
            filterer = new GraphDataFilter(allPatients);
        }

        [TearDown] 
        public void Dispose()
        {
            allPatients = null;
            filterer = null;
        }


        //Tumour filter works as expected
        [Test]
        public void FilterCorrectlyTumour()
        {
            List<string> filterList = new List<string>{"T1", "T3"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetTumourName()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByTumour(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Nodal filter works as expected
        [Test]
        public void FilterCorrectlyNodal()
        {
            List<string> filterList = new List<string>{"N0", "N3"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetNodalPosition()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByNodal(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //TotalRT filter works as expected
        [Test]
        public void FilterCorrectlyTotalRT()
        {
            List<string> filterList = new List<string>{"65/30", "55/20"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetTotalRT()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByTotalRT(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Side filter works as expected
        [Test]
        public void FilterCorrectlySide()
        {
            List<string> filterList = new List<string>{"N/A", "R"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetTumourJawSide()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterBySide(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Site filter works as expected
        [Test]
        public void FilterCorrectlySite()
        {
            List<string> filterList = new List<string>{"OP - OTHER", "TONSIL"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetSite()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterBySite(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Gender filter works as expected
        [Test]
        public void FilterCorrectlyGender()
        {
            List<string> filterList = new List<string>{"M"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetGender()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByGender(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Treatment filter works as expected
        [Test]
        public void FilterCorrectlyTreatment()
        {
            List<string> filterList = new List<string>{"CRT", "RT"};
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(filterList.Contains(patient.GetTreatment()))
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByTreatment(filterList);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }

        //Age filter works as expected
        [Test]
        public void FilterCorrectlyAge()
        {
            int ageMin = 50;
            int ageMax = 59;
            List<Patient> expected = new List<Patient>();
            foreach(Patient patient in allPatients)
            {
                if(patient.GetCancerAge() >= ageMin && patient.GetCancerAge() <= ageMax)
                {
                    expected.Add(patient);
                }
            }
            filterer.FilterByAge(ageMin, ageMax);
            List<Patient> outcome = filterer.GetPatients();
            Assert.AreEqual(outcome, expected);
        }  

        //GetRadiationList, given a List of Patients who all have the same radiation, returns a list of tuples, with the same amount of tuples as patients, each of those tuples having the same radiation value
        [Test]
        public void GetRadiationListReturnsGivenAllSameReturnsAppropriateData()
        {
            List<Patient> patientsWith5004dot5 = mock.CreateSimpleRadiationMockData(5004.5); //all patients have 5004.5 radiation across all their teeth (for all min, mean, and max)
            List<int> sampleValidTeethList = new List<int>{0, 11, 22, 31};
            List<Tuple<Patient, double>> outcome = filterer.GetRadiationList(patientsWith5004dot5, 0, sampleValidTeethList);

            //Sizes of number of patients given, and number of patient-radiation tuples produced, should be the same
            Assert.AreEqual(patientsWith5004dot5.Count, outcome.Count);
            
            foreach(Tuple<Patient, double> patientRadiation in outcome)
            {
                //All of the patientRadiation pair Radiations should be equal to 5004.5 
                Assert.AreEqual(patientRadiation.Item2, 5004.5);
            }
        }

        //GetRadiationList, given an invalid list of teethNumbers (and arbitrary list of patients), returns a list of tuples, with the same amount of tuples as patients, however radiation being 0 for all tuples
        [Test]
        public void GetRadiationListReturnsGivenInvalidTeethRangesReturnsAppropriateData()
        {
            List<int> invalidTeethList = new List<int>{100354, 352253, 35233, -342253};
            List<Tuple<Patient, double>> outcome = filterer.GetRadiationList(allPatients, 0, invalidTeethList);

            //Sizes of number of patients given, and number of patient-radiation tuples produced, should be the same
            Assert.AreEqual(allPatients.Count, outcome.Count);
            
            foreach(Tuple<Patient, double> patientRadiation in outcome)
            {
                //All of the patientRadiation pair Radiations should be equal to 0 
                Assert.AreEqual(patientRadiation.Item2, 0);
            }
        }

        //Passing List<string> with first entry as "All" keeps all patients (does not filter anything out)
        [Test]
        public void ListWithAllKeepsEverything()
        {
            List<string> allList = new List<string>();
            allList.Add("All");

            filterer.FilterByTumour(allList);
            filterer.FilterByNodal(allList);
            filterer.FilterByTotalRT(allList);
            filterer.FilterBySide(allList);
            filterer.FilterBySite(allList);
            filterer.FilterByGender(allList);
            filterer.FilterByTreatment(allList);

            List<Patient> filteredData = filterer.GetPatients();

            //Nothing should be removed by these filters
            Assert.AreEqual(allPatients, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - tumour filter
        [Test]
        public void EmptyListKeepsNothingTumour()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterByTumour(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - Nodal filter
        [Test]
        public void EmptyListKeepsNothingNodal()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterByNodal(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - TotalRT filter
        [Test]
        public void EmptyListKeepsNothingTotalRT()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterByTotalRT(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - Side filter
        [Test]
        public void EmptyListKeepsNothingSide()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterBySide(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - Site filter
        [Test]
        public void EmptyListKeepsNothingSite()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterBySite(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - Gender filter
        [Test]
        public void EmptyListKeepsNothingGender()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterByGender(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing empty List<string> keeps no patients (everything filtered out) - Treatment filter
        [Test]
        public void EmptyListKeepsNothingTreatment()
        {
            List<string> nothingList = new List<string>();
            filterer.FilterByTreatment(nothingList);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Passing a maxAge less than minAge keeps no patients (everything filtered out) - Age filter
        //There are patients with ages equal to max and min
        [Test]
        public void MaxMoreThanMinListKeepsNothingAge()
        {
            filterer.FilterByAge(87, 50);
            List<Patient> filteredData = filterer.GetPatients();
            //Nothing should be kept by these filters
            List<Patient> emptyList = new List<Patient>();
            Assert.AreEqual(emptyList, filteredData);
        }

        //Getter for patient list returns unchanged patient list, when no filters performed.
        [Test]
        public void GetterAfterNoFiltersPerformed()
        {
            Assert.AreEqual(allPatients, filterer.GetPatients());
        }
    }
}
