using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /**
     * Testing the DataModel Class
     */
    public class TestingDataModel
    {
        List<Patient> multiplePatients;
        List<Patient> singlePatient;
        MockDataLoader mockData;
        Patient patient;

        [SetUp]
        public void Init()
        {
            mockData = new MockDataLoader();
            multiplePatients = mockData.GetMultiplePatientsList();
            singlePatient = mockData.GetSinglePatientList();
            patient = singlePatient[0];
        }

        [TearDown]
        public void Dispose()
        {
            mockData = null;
            multiplePatients = null;
            singlePatient = null;
            patient = null;
        }

        [Test]
        public void TestCombineLeftAndRightTeethRaditaionMultiplePatients()
        {
            List<double[]> ExpectedRadiationValues = new List<double[]>();
            double[] ExpectedArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < multiplePatients.Count; ++i)
            {
                ExpectedRadiationValues.Add(new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 });
            }

            Assert.AreEqual(ExpectedRadiationValues, DataModel.CombineRightLeftTeethRadiation(multiplePatients, "bottom"));
            Assert.AreEqual(ExpectedRadiationValues, DataModel.CombineRightLeftTeethRadiation(multiplePatients, "top"));
        }

        [Test]
        public void TestCombineLeftAndRightRadiationSinglePatient()
        {
            List<double[]> ExpectedRadiationValues = new List<double[]>();
            double[] ExpectedArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            ExpectedRadiationValues.Add(new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 });

            Assert.AreEqual(ExpectedRadiationValues, DataModel.CombineRightLeftTeethRadiation(singlePatient, "bottom"));
            Assert.AreEqual(ExpectedRadiationValues, DataModel.CombineRightLeftTeethRadiation(singlePatient, "top"));
        }

        [Test]
        public void TestCombineLeftAndRightRadiationIncorrectString()
        {
            var ex = Assert.Throws<Exception>(() => DataModel.CombineRightLeftTeethRadiation(multiplePatients, ""));
            Assert.That(ex.Message, Is.EqualTo("Incorrect string parsed"));
        }

        [Test]
        public void TestCombineLeftAndRightNoData()
        {
            List<Patient> noPatient = new List<Patient>();
            double[] ExpectedArray = new double[0];
            Assert.AreEqual(ExpectedArray, DataModel.CombineRightLeftTeethRadiation(noPatient, "top"));
            Assert.AreEqual(ExpectedArray, DataModel.CombineRightLeftTeethRadiation(noPatient, "bottom"));
        }


        [Test]
        public void TestCalculateAverageMeanRadiationMultiplePatients()
        {

            List<double[]> InitialRadiationValues = new List<double[]>();
            double[] InitialArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < multiplePatients.Count; ++i)
            {
                InitialRadiationValues.Add(new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 });
            }
            double[] ExpectedArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(ExpectedArray, DataModel.CalculateAverageMeanRadiation(InitialRadiationValues));
        }

        [Test]
        public void TestCalculateAverageMeanRadiationSinglePatient()
        {
            List<double[]> InitialRadiationValues = new List<double[]>();
            double[] InitialArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            InitialRadiationValues.Add(new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 });
            double[] ExpectedArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };
            Assert.AreEqual(ExpectedArray, DataModel.CalculateAverageMeanRadiation(InitialRadiationValues));
        }

        [Test]
        public void TestCalculateAverageMeanRadiationNoData()
        {
            List<double[]> InitialRadiationValues = new List<double[]>();
            double[] ExpectedArray = new double[16] { double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN };
            Assert.AreEqual(ExpectedArray, DataModel.CalculateAverageMeanRadiation(InitialRadiationValues));
        }

        [Test]
        public void TestgetMinMaxMultiplePatients()
        {
            Tuple<double, double> ExpectedMinMax = new Tuple<double, double>(1, 8);
            Assert.AreEqual(ExpectedMinMax, DataModel.getMinMAx(multiplePatients));
        }

        [Test]
        public void TestgetMinMaxSinglePatients()
        {
            Tuple<double, double> ExpectedMinMax = new Tuple<double, double>(1, 8);
            Assert.AreEqual(ExpectedMinMax, DataModel.getMinMAx(singlePatient));
        }

        [Test]
        public void TestgetMinMaxNoPatients()
        {
            Tuple<double, double> ExpectedMinMax = new Tuple<double, double>(Double.NaN, Double.NaN);
            Assert.AreEqual(ExpectedMinMax, DataModel.getMinMAx(new List<Patient>()));
        }

    }
}
