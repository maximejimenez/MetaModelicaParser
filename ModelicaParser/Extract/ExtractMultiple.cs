﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ModelicaParser;
using ModelicaParser.Config;

namespace ModelicaParser.Extract
{
    // extracting multiple AUTOSAR meta-model versions
    class ExtractMultiple
    {
        private MainForm form;
        private string[] releases;          // list of releases to be extracted (their paths)

        #region Extraction

        // creation of the extract multiple which automatically starts the extraction
        public ExtractMultiple(MainForm form)
        {
            this.form = form;

            try
            {
                bool validates = ConfigReader.Read(form.TextBoxNotRelevant1.Text, form);               // configuration for the extract multiple is done based on the config file whose path is provided in the GUI

                if (validates)  // if the XML config file is validated
                {
                    releases = new string[ConfigReader.ExtractMultipleReleases.Count];

                    // TODO : handle full directory or multiple file from the front end compiler
                    for (int i = 0; i < ConfigReader.ExtractMultipleReleases.Count; i++)
                        releases[i] = ConfigReader.ExtractMultipleReleases[i] + "\\Absyn.mo";              // getting all release paths from the config file

                    form.ListAdd("Release paths successfully read.");

                    ExtractModels();            // automated extraction
                }
                else
                    form.ListAdd("Config file not read.");
            }
            catch (Exception exp)
            {
                form.ListAdd(exp.ToString());
            }
        }

        // extracting multiple meta-models
        private void ExtractModels()
        {
            for (int i = 0; i < releases.Length; i++)       // extracted models are created in the same folder as the AUTOSAR models
            {
                string modelPath = releases[i];     // extracted name is the same as the meta-model name, just the extension "eap" is exchanged with "mod"
                string filePath = releases[i].Substring(0, releases[i].Length - 3) + ".xml";

                if (File.Exists(filePath))      // the extraction is not done if the "mod" file with the same name exists
                    form.ListAdd("File " + filePath + " already exists.");
                else
                {
                    MM_Extractor extractor = new MM_Extractor(form);

                    form.ListAdd("Dumping " + modelPath);
                    extractor.ExtractModel(modelPath, filePath);
                }
            }

            form.ListAdd("Done!");
        }

        #endregion
    }
}