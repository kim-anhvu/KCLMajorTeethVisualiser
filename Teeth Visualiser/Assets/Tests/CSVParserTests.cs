using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
  public class CSVParserTests
  {

    List<Patient> patients;
    int expectedSize = 2;
    string path = "Assets/Tests/Resources/test.csv";

    private string tumour = "T1";
    private string nodal = "N0";
    private string gender = "M";
    private int ageCancer = 62;
    private string site = "OP-OTHER";
    private string tumourJawSide = "N/A";
    private string treatment = "RT";
    private string totalRT = "65/30";

    private Patient patient;

    // This patient has invalid data in all teeth radiation columns
    private Patient invalidPatient;

    double[,] expectedLowerLeft = new double[,] { { 0,1,2 }, { 3,4,5 }, { 6,7,8 }, { 9,10,11 }, { 12,13,14 }, { 15,16,17 }, { 18,19,20 }, { 21,22,23 } };
    double[,] expectedLowerRight = new double[,] { { 24,25,26 }, { 27,28,29 }, { 30,31,32 }, { 33,34,35 }, { 36,37,38 }, { 39,40,41 }, { 42,43,44 }, { 45,46,47 } };
    double[,] expectedUpperLeft = new double[,] { { 48,49,50 }, { 51,52,53 }, { 54,55,56 }, { 57,58,59 }, { 60,61,62 }, { 63,64,65 }, { 66,67,68 }, { 69,70,71 } };
    double[,] expectedUpperRight = new double[,] { { 72,73,74 }, { 75,76,77 }, { 78,79,80 }, { 81,82,83 }, { 84,85,86 }, { 87,88,89 }, { 90,91,92 }, { 93,94,95 } };

    double[,] expectedInvalidPatient = new double[,] { { 0,0,0 }, { 0,0,0 }, { 0,0,0 }, { 0,0,0 }, { 0,0,0 }, { 0,0,0 }, { 0,0,0 }, { 0,0,0 } };


    [SetUp]
    public void Init()
    {
      patients = CSVParser.LoadCSV(path);
      patient = patients[0];
      invalidPatient = patients[1];
    }

    [Test]
    public void CheckSize()
    {
      int output = patients.Count;
      Assert.AreEqual(expectedSize, output);
    }

    [Test]
    public void CheckTumor() {
      string output = patient.GetTumourName();
      Assert.AreEqual(tumour, output);
    }

    [Test]
    public void CheckNodal() {
      string output = patient.GetNodalPosition();
      Assert.AreEqual(nodal, output);
    }

    [Test]
    public void CheckGender() {
      string output = patient.GetGender();
      Assert.AreEqual(gender, output);
    }

    [Test]
    public void CheckAgeCancer() {
      int output = patient.GetCancerAge();
      Assert.AreEqual(ageCancer, output);
    }

    [Test]
    public void CheckSite() {
      string output = patient.GetSite();
      Assert.AreEqual(site, output);
    }

    [Test]
    public void CheckTumorJawSide() {
      string output = patient.GetTumourJawSide();
      Assert.AreEqual(tumourJawSide, output);
    }

    [Test]
    public void CheckTreatment() {
      string output = patient.GetTreatment();
      Assert.AreEqual(treatment, output);
    }

    [Test]
    public void CheckTotalRT() {
      string output = patient.GetTotalRT();
      Assert.AreEqual(totalRT, output);
    }

    [Test]
    public void CheckLowerLeft()
    {
      double[,] output = patient.GetLowerLeft();
      // Array has to be reversed due to the way teeth are displayed, front to back
      Assert.AreEqual(CSVParser.reverseLeft(expectedLowerLeft), output);
    }

    [Test]
    public void CheckLowerRight()
    {
      double[,] output = patient.GetLowerRight();
      Assert.AreEqual(expectedLowerRight, output);
    }

    [Test]
    public void CheckUpperLeft()
    {
      double[,] output = patient.GetUpperLeft();
      // Array has to be reversed due to the way teeth are displayed, front to back
      Assert.AreEqual(CSVParser.reverseLeft(expectedUpperLeft), output);
    }

    [Test]
    public void CheckUpperRight()
    {
      double[,] output = patient.GetUpperRight();
      Assert.AreEqual(expectedUpperRight, output);
    }

    /**
    * Testing to see that the incorrect values are parsed as 0
    */

    [Test]
    public void CheckInvalidPatientLowerLeft() {
      double[,] output = invalidPatient.GetLowerLeft();
      Assert.AreEqual(expectedInvalidPatient, output);
    }

    [Test]
    public void CheckInvalidPatientLowerRight() {
      double[,] output = invalidPatient.GetLowerRight();
      Assert.AreEqual(expectedInvalidPatient, output);
    }

    [Test]
    public void CheckInvalidPatientUpperLeft() {
      double[,] output = invalidPatient.GetUpperLeft();
      Assert.AreEqual(expectedInvalidPatient, output);
    }

    [Test]
    public void CheckInvalidPatientUpperRight() {
      double[,] output = invalidPatient.GetUpperRight();
      Assert.AreEqual(expectedInvalidPatient, output);
    }

  }
}
