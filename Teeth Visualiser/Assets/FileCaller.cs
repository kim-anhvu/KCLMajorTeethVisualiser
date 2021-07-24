using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Include these namespaces to use BinaryFormatter
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace GracesGames.SimpleFileBrowser.Scripts {
	// Class to use the FileBrowser script
	// Able to load files paths
	public class FileCaller : MonoBehaviour {

		// Use the file browser prefab
		public GameObject FileBrowserPrefab;

		// Define a file extension
		private string[] FileExtensions;

        public Button addCSVButton;



		public bool PortraitMode;

		// Find the input field, label objects and add a onValueChanged listener to the input field
		private void Start() {
            if(GameObject.Find("FileBrowserUI") == null)
            {
                addCSVButton.interactable = true;
            }else{
                addCSVButton.interactable = false;
            }
		}



		// Open the file browser for loading
		public void OpenFileBrowser() {
      FileExtensions = new string[]{"csv"};
			OpenFileBrowser(FileBrowserMode.Load);
		}

		// Open a file browser to load files
		private void OpenFileBrowser(FileBrowserMode fileBrowserMode) {
			// Create the file browser and name it
			GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
			fileBrowserObject.name = "FileBrowser";

			// Set the mode to save or load
			FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
			fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);
			fileBrowserScript.OpenFilePanel(FileExtensions);
			// Subscribe to OnFileSelect event (call LoadFileUsingPath using path)
			fileBrowserScript.OnFileSelect += LoadFileUsingPath;
            GameObject fb = GameObject.Find("FileBrowserUI");
			fb.transform.localScale = new Vector3(0.5f,0.5f,1f);


		}


		// Loads a file using a path
		private void LoadFileUsingPath(string path) {
      LoadNewCSV.Load(path);
		}
	}
}
