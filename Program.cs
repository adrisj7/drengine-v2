﻿using System;
using System.Collections.Generic;
using NDesk.Options;
using System.Diagnostics;


namespace DREngine
{
    /// <summary>
    /// The main class. This will run either the game or the editor.
    /// </summary>
    public static class Program
    {
#region Public Accessors

        public static string RootDirectory { get; private set; }

#endregion

#region Main functions

        private static void StartGame(string projectPath) {
            using (var game = new DRGame(projectPath)) {
                game.Run();
            }
        }

        private static void StartEditor() {
            using (var editor = new DREditor()) {
                editor.Run();
            }
        }

#endregion

#region Program Utils

        private static void SetRootDirectory() {
            // Set the root directory
            string thisFile = new StackTrace(true).GetFrame(0).GetFileName();
            int lastDir = thisFile.LastIndexOf("/");
            if (lastDir != -1)
            {
                thisFile = thisFile.Substring(0, lastDir);
            }

            RootDirectory = thisFile;
        }

#endregion

        [STAThread]
        static void Main(string[] args)
        {
            SetRootDirectory();

            // Parse args
            bool useGame = false;
            string projectPath = null;
            var opts = new OptionSet() {
                {
                    "g|game", v => {
                        useGame = true;
                        projectPath = v;
                    }
                }
            };
            opts.Parse (args);
            // Run
            if (useGame) {
                StartGame(projectPath);
            } else {
                StartEditor();
            }
        }
    }
}

