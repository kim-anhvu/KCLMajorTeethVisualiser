using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

/**
* The CSVParser class is used to load in CSV files which have been added
* previously by the user and then parses all the patients, stores them in
* patient objects and returns all the objects in a list.
*/
public class CSVParser
{

    private List<Patient> patients = new List<Patient>();

    private string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
    ? Environment.GetEnvironmentVariable("HOME")
    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

    private string csvDirectory = "/TeethVisualiser/";

    private static Regex regex = new Regex("[^0-9.]");

    /**
    * Loads all CSV files in a folder called TeethVisualiser in the
    * user home directory. It will create this directory if it does not exist.
    */
    public void Startup()
    {
      System.IO.Directory.CreateDirectory(homePath + csvDirectory);
      FileInfo[] fileInfos = GetAllFilesInDirectory();
      LoadAllPatients(fileInfos);
    }

    /**
    * Retrieves all files in the directory where files are stored
    */
    public FileInfo[] GetAllFilesInDirectory() {
      DirectoryInfo directory = new DirectoryInfo(homePath + csvDirectory);
      FileInfo[] fileInfos = directory.GetFiles("*.csv");
      return fileInfos;
    }

    /**
    * Calls LoadCSV on every file and appends it to the patients list
    * @param fileInfos
    */
    private void LoadAllPatients(FileInfo[] fileInfos) {
      foreach (FileInfo f in fileInfos)
      {
          patients.AddRange(LoadCSV(homePath + csvDirectory + f.Name));
      }
    }

    /**
    * Loads the CSV file at a given path
    * @param path
    */
    public static List<Patient> LoadCSV(string path)
    {
        List<string[]> patientData = ReadCSVFile(path);
        List<Patient> parsedPatients = ParseData(patientData);
        return parsedPatients;
    }

    /**
    * @param path it will read and structure the csv file at this path
    * @return patientData
    */
    public static List<string[]> ReadCSVFile(string path)
    {
      List<string[]> patientData = new List<string[]>();
      using (var reader = new StreamReader(@path))
      {
          while (!reader.EndOfStream)
          {
              var line = reader.ReadLine();
              var values = line.Split(',');
              patientData.Add(values);
          }
      }
      return patientData;
    }

    /*
    * In total there 104 columns
    * 8 for non array values
    * 24 for lower left teeth
    * 24 for lower right teeth
    * 24 for upper left teeth
    * 24 for upper right teeth
    * Given these values, a for loop is run from 8 to 104
    * with data being parsed in intervals depending on the quadrant they are located in.
    *
    * @param patientData
    * @return parsedPatients
    */
    private static List<Patient> ParseData(List<string[]> patientData)
    {
        List<Patient> parsedPatients = new List<Patient>();
        for (int i = 1; i < patientData.Count; i++)
        {
            double[,] lowerLeft = new double[8, 3];
            double[,] lowerRight = new double[8, 3];
            double[,] upperLeft = new double[8, 3];
            double[,] upperRight = new double[8, 3];

            int count = 0;
            for (int x = 8; x < 104; x += 3)
            {
                if(count == 8)
                {
                    count = 0;
                }
                if (x <= 31)
                {
                    lowerLeft[count, 0] = ParseDouble(patientData[i][x]);
                    lowerLeft[count, 1] = ParseDouble(patientData[i][x + 1]);
                    lowerLeft[count, 2] = ParseDouble(patientData[i][x + 2]);
                }


                else if (x <= 55)
                {
                    lowerRight[count, 0] = ParseDouble(patientData[i][x]);
                    lowerRight[count, 1] = ParseDouble(patientData[i][x + 1]);
                    lowerRight[count, 2] = ParseDouble(patientData[i][x + 2]);
                }

                else if (x <= 79)
                {
                    upperLeft[count, 0] = ParseDouble(patientData[i][x]);
                    upperLeft[count, 1] = ParseDouble(patientData[i][x + 1]);
                    upperLeft[count, 2] = ParseDouble(patientData[i][x + 2]);
                }
                else
                {
                    upperRight[count, 0] = ParseDouble(patientData[i][x]); ;
                    upperRight[count, 1] = ParseDouble(patientData[i][x + 1]);
                    upperRight[count, 2] = ParseDouble(patientData[i][x + 2]);
                }
                count++;
            }

            lowerLeft = reverseLeft(lowerLeft);
            upperLeft = reverseLeft(upperLeft);
            Patient p = new Patient(
            patientData[i][0],
            patientData[i][1],
            patientData[i][2],
            int.Parse(patientData[i][3]),
            patientData[i][4],
            patientData[i][5],
            patientData[i][6],
            patientData[i][7],
            lowerLeft,
            lowerRight,
            upperLeft,
            upperRight);
            parsedPatients.Add(p);
        }
        return parsedPatients;
    }

    /**
    * Reverses the values for all teeth in the any left quadrant
    * of the mouth due to the way the teeth are displayed.
    * From front to back, whereas the data is back to front. (Requirements)
    */
    public static double[,] reverseLeft(double[,] left)
    {
        double[] tempArray = new double[8];
        int j = 0;
        for(int i=7; i>-1; --i){
            tempArray[j]=left[i,1];
            j++;
        }

        for (int k=0; k<8;k++){
            left[k, 1] = tempArray[k];
        }
        return left;
    }

    /**
    * Replaces any non numberic value or a '.' with an empty char
    * if the whole string is empty it will return a zero else the value.
    */
    public static double ParseDouble(string str)
    {
      bool valid = CheckValidString(str);
      str = (valid == true ? str : "");

      if(str == "")
        return 0;
      else
        return double.Parse(str);
    }

    /**
    * Checks if the string contains any non numeric value excluding '.'
    * If it does return false, else true
    */
    private static bool CheckValidString(string str) {
      foreach(char c in str) {
        bool isDigit = Char.IsDigit(c);
        if(!(isDigit || c == '.'))
          return false;
      }
      return true;
    }

    public List<Patient> GetPatients(){
        return patients;
    }
}
