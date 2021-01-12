﻿using System;
using System.Diagnostics;
using DREngine.Editor;
using DREngine.Game;
using NDesk.Options;

namespace DREngine
{
    /// <summary>
    ///     The main class. This will run either the game or the editor.
    /// </summary>
    public static class Program
    {
        #region Program Utils

        private static void SetRootDirectory()
        {
            // Set the root directory
            var thisFile = new StackTrace(true).GetFrame(0)?.GetFileName()?.Replace('\\', '/');
            if (thisFile != null)
            {
                var lastDir = thisFile.LastIndexOf("/", StringComparison.Ordinal);
                if (lastDir != -1) thisFile = thisFile.Substring(0, lastDir);
            }

            RootDirectory = thisFile;
        }

        #endregion

        [STAThread]
        private static void Main(string[] args)
        {
            SetRootDirectory();

            // Parse args
            var useGame = false;
            var debugEditor = false;
            string projectPath = null;
            string editorPipeReadHandle = null;
            string editorPipeWriteHandle = null;
            string startScene = null;
            var opts = new OptionSet
            {
                {
                    "g|game:", v =>
                    {
                        useGame = true;
                        projectPath = v;
                    }
                },
                {
                    "readpipe:", v =>
                    {
                        editorPipeReadHandle = v;
                        debugEditor = true;
                    }
                },
                {
                    "writepipe:", v =>
                    {
                        editorPipeWriteHandle = v;
                        debugEditor = true;
                    }
                },
                {
                    "scene:", v =>
                    {
                        startScene = v;
                    }
                },
                {
                    "sceneedit:", v =>
                    {
                        startScene = v;
                    }
                }
            };
            opts.Parse(args);

            // Minor parsing
            if (debugEditor && (editorPipeReadHandle == null || editorPipeWriteHandle == null))
            {
                Console.Error.WriteLine(
                    "If you're using editor piping at all, you must specify both editor write pipe AND editor read pipe. Sorry!");
                return;
            }

            // Open
            if (useGame)
            {
                using (var game = new DRGame(projectPath))
                {
                    if (debugEditor)
                    {
                        game.InitializeEditorHookup(editorPipeReadHandle,
                            editorPipeWriteHandle);
                    }
                    if (startScene != null)
                    {
                        game.InitializeEditorSceneTool(startScene);
                    }
                    game.Run();
                }                
            }
            else
            {
                StartEditor();
            }
        }

        #region Public Accessors

        public static readonly Version Version = new Version("InDev", 0, 1, 0);

        public static string RootDirectory { get; private set; }

        #endregion

        #region Main functions

        private static void StartEditor()
        {
            using (var editor = new DREditor())
            {
                editor.Run();
            }
        }

        #endregion
    }
}