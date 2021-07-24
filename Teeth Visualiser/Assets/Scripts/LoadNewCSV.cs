using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/**
* The LoadNewCSV class is used to copy a CSV file a user has selected
* into a folder called TeethVisualiser in the user home directory so
* that every time the application is loaded, the user will not have to reload
* the CSV files manually as it wil load all files in this directory automatically.
*/
public static class LoadNewCSV {

    /**
    * Initially called when a user has selected a CSV file.
    */
    public static void Load(string path)
    {
      bool validCSV = ValidateCSV(path);
      if(validCSV) {
        string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
        ? Environment.GetEnvironmentVariable("HOME")
        : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        string csvDirectoryNew = "/TeethVisualiser/";

        string name = GenerateString();

        CopyCSVToDirectory(path, homePath, csvDirectoryNew, name);
      }
    }

    /**
    * Validates the CSV file by checking if there are 104 columns
    */
    private static bool ValidateCSV(string path) {
      int columns = 104;
      List<String[]> patientData = CSVParser.ReadCSVFile(path);
      if(patientData[0].Length == columns)
        return true;
      else
        return false;
    }

    /**
    * Generates a random string to add onto the end of the file name
    * to avoid conflicts with two CSV files having the same name
    */
    private static string GenerateString() {
      string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
      string name = "";
      int length = 10;
      System.Random random = new System.Random();
      for(int i =0; i<length; i++) {
        int num = random.Next(0, chars.Length);
        name += chars[num];
      }
      name += ".csv";
      return name;
    }

    /**
    * Sets a new name for the CSV file
    * Copies the CSV file to the TeethVisualiser folder in the home directory
    * Appends the newly loaded patients to the current ones in the application
    */
    private static void CopyCSVToDirectory(string path, string homePath, string csvDirectoryNew, string name)
    {
      string[] pathSplit = path.Split('\\');
      string originalName = pathSplit[pathSplit.Length - 1];
      originalName = originalName.Remove(originalName.Length-4);
      originalName += name;
      File.Copy(path, homePath + csvDirectoryNew + name);
      DataLoader.AppendPatients(CSVParser.LoadCSV(path));
    }
}
