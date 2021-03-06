﻿using System;
using System.IO;
using DREngine.Editor.Components;
using DREngine.Game.Resources;
using DREngine.Game.VN;
using DREngine.ResourceLoading;
using GameEngine;
using GameEngine.Game;
using GameEngine.Game.Resources;
using Gdk;
using GLib;
using Gtk;
using Action = System.Action;
using Menu = Gtk.Menu;
using MenuItem = Gtk.MenuItem;
using Path = GameEngine.Game.Path;
using Window = Gtk.Window;
using WindowType = Gtk.WindowType;

namespace DREngine.Editor
{
    public class DREditorMainWindow : Window
    {
        private readonly DREditor _editor;
        private EditorLog _log;

        private ResourceView _resourceView;

        public Icons Icons;

        public DREditorMainWindow(DREditor editor) : base(WindowType.Toplevel)
        {
            _editor = editor;
            // Make window is called externally.
        }

        public Action<string, string> OnFileOpened
        {
            get => _resourceView.OnFileOpened;
            set => _resourceView.OnFileOpened = value;
        }

        #region Public Control

        public void MakeWindow(string title, int width, int height)
        {
            _log = new EditorLog();
            Icons = new Icons();


            Title = title;
            Resize(width, height);
            Maximize();
            Resizable = true;

            var mainBox = new VBox();
            var menuBar = MakeMenuBar();
            var projectButtons = MakeProjectActionButtonBar();

            var vertPane = new HPaned();
            vertPane.WideHandle = true; // Separate left and right side
            vertPane.Position = 256; // How wide the left side starts

            _resourceView = new ResourceView(Icons);
            HookupLog(_log);
            HookupResourceView(_resourceView);
            var rightSide = new VPaned();
            rightSide.WideHandle = true;
            Widget emptyBox = new VBox();
            rightSide.Pack1(emptyBox, false, false);
            emptyBox.Show();
            rightSide.Pack2(_log, true, false);
            _log.Show();
            //rightSide.Position = 256;
            vertPane.Pack1(_resourceView, false, false);
            _resourceView.Show();
            vertPane.Pack2(rightSide, true, false);
            rightSide.Show();

            mainBox.PackStart(menuBar, false, true, 4);
            menuBar.Show();
            mainBox.PackStart(projectButtons, false, false, 4);
            projectButtons.Show();
            mainBox.PackStart(vertPane, true, true, 4);
            vertPane.Show();

            mainBox.Show();

            Add(mainBox);
        }

        private void HookupLog(EditorLog log)
        {
            Debug.OnLogDebug += log.PrintDebug;
            Debug.OnLogPrint += log.Print;
            Debug.OnLogWarning += log.PrintWarning;
            Debug.OnLogError += (message, trace) => { log.PrintError($"{message}\n{trace}"); };
            Debug.LogDebug("Editor Log Initialized");
        }

        private void HookupResourceView(ResourceView resourceView)
        {
            resourceView.OnNewFolder += projectDir =>
            {
                if (!AssertProjectLoaded()) return;

                using var dialog = new NewFolderDialog(_editor, this, new ProjectPath(_editor, projectDir));
                if (dialog.RunUntilAccept())
                {
                    var toAdd = dialog.GetTargetDirectory();
                    Directory.CreateDirectory(toAdd);
                    Debug.LogSilent($"New Folder: {toAdd.RelativePath}");
                    resourceView.AddFolder(toAdd.RelativePath, true);
                }
            };

            resourceView.OnNewResource += (projectDir, type) =>
            {
                if (!AssertProjectLoaded()) return;

                if (type == typeof(DRSprite))
                {
                    using var dialog = new NewSpriteDialog(_editor, this, new ProjectPath(_editor, projectDir));
                    if (dialog.RunUntilAccept())
                    {
                        var toAdd = dialog.GetTargetDirectory();
                        Path toCopy = dialog.ImageToCopy;
                        File.Copy(toCopy, toAdd);
                        Debug.LogSilent($"New Sprite: {toAdd.RelativePath}");
                        resourceView.AddFile(toAdd.RelativePath, true);
                    }
                }
                else if (type == typeof(Font))
                {
                    using var dialog = new NewFontDialog(_editor, this, new ProjectPath(_editor, projectDir));
                    if (dialog.RunUntilAccept())
                    {
                        var toAdd = dialog.GetTargetDirectory();
                        Path toCopy = dialog.FontToCopy;
                        File.Copy(toCopy, toAdd);
                        Debug.LogSilent($"New Font: {toAdd.RelativePath}");
                        resourceView.AddFile(toAdd.RelativePath, true);
                    }
                }
                else if (type == typeof(AudioClip))
                {
                    using var dialog = new NewAudioClipDialog(_editor, this, new ProjectPath(_editor, projectDir));
                    if (dialog.RunUntilAccept())
                    {
                        var toAdd = dialog.GetTargetDirectory();
                        Path toCopy = dialog.AudioToCopy;
                        File.Copy(toCopy, toAdd);
                        Debug.LogSilent($"New Audio Clip: {toAdd.RelativePath}");

                        // Load and save extra file...
                        var clip = new AudioClip();
                        clip.Type = dialog.ImportType;
                        clip.Save(toAdd);

                        resourceView.AddFile(toAdd.RelativePath, true);
                    }
                } else if (type == typeof(VNScript))
                {
                    using var dialog = new NewVNScriptDialog(_editor, this, new ProjectPath(_editor, projectDir));
                    if (dialog.RunUntilAccept())
                    {
                        var toAdd = dialog.GetTargetDirectory();
                        VNScript newScript = new VNScript();
                        newScript.Save(toAdd);

                        resourceView.AddFile(toAdd.RelativePath, true);
                    }
                }
                else
                {
                    AlertProblem($"Resource creation not implemented yet: {type}. Sorry!");
                }
            };
        }

        private bool AssertProjectLoaded()
        {
            if (!_editor.ProjectLoaded)
            {
                AlertProblem("Project not loaded.");
                return false;
            }

            return true;
        }


        public void EmptyProject()
        {
            _resourceView.Clear();
        }

        public void LoadProject(ProjectData data, string fullPath, Action<string> onFileLoad = null)
        {
            if (fullPath.EndsWith("project.json"))
                fullPath = fullPath.Substring(0, fullPath.Length - "project.json".Length);

            _resourceView.Clear();
            _resourceView.LoadDirectory(fullPath,
                dir =>
                {
                    // We don't really care about directories, do we?
                },
                file =>
                {
                    var relative = System.IO.Path.GetRelativePath(fullPath, file);
                    onFileLoad?.Invoke(relative);
                }
            );
        }

        /// <summary>
        ///     Set the theme of the window. Must be a valid gtk.css file.
        /// </summary>
        public void SetTheme(string path)
        {
            var failed = false;
            try
            {
                var cssProvider = new CssProvider();
                if (!cssProvider.LoadFromPath(path))
                    failed = true;
                else
                    StyleContext.AddProviderForScreen(Screen.Default, cssProvider, 800);
            }
            catch (GException)
            {
                failed = true;
            }

            if (failed)
                AlertProblem("Failed to load theme",
                    $"Invalid theme path: \"{path}\"\n\nMake sure the target is a valid gtk.css file!");
        }

        public void AlertProblem(string title, string message)
        {
            Debug.LogWarning($"Problem: {message}");
            var popup = new MessageDialog(
                this,
                DialogFlags.DestroyWithParent,
                MessageType.Error,
                ButtonsType.Ok,
                message
            ) {Title = title, WindowPosition = WindowPosition.Center};
            popup.Show();
            popup.Run();
            popup.Dispose();
        }

        public void AlertProblem(string message)
        {
            AlertProblem("Problem!", message);
        }

        #endregion

        #region Widget/UI Construction

        /// <summary>
        ///     File, Edit, View, Build, Help, etc...
        /// </summary>
        private Widget MakeMenuBar()
        {
            var menuBar = new MenuBar();

            var fileMenu = new Menu();

            var file = new MenuItem("File");
            file.Submenu = fileMenu;

            var newProject = new MenuItem("New Project");
            newProject.Activated += (sender, args) => NewProjectPressed();
            fileMenu.Append(newProject);
            var openProject = new MenuItem("Open Project");
            openProject.Activated += (sender, args) => OpenProjectPressed();
            fileMenu.Append(openProject);
            var reloadProject = new MenuItem("Reload Project");
            reloadProject.Activated += (sender, args) => _editor.ReloadCurrentProject();
            fileMenu.Append(reloadProject);

            menuBar.Append(file);
            menuBar.ShowAll();

            return menuBar;
        }

        /// <summary>
        ///     Contains the run button and other useful buttons for the project.
        /// </summary>
        private Widget MakeProjectActionButtonBar()
        {
            Box b = new HBox();
            b.HeightRequest = 16;

            // Make the action buttons
            var newProj = NewButton("New Empty Project", Icons.New, NewProjectPressed);
            var open = NewButton("Open Project", Icons.Open, OpenProjectPressed);
            Separator s1 = new HSeparator();
            var export = NewButton("Export Project", Icons.Export, ExportProjectPressed);
            Separator s2 = new HSeparator();
            Button run = new RunProjectButton(_editor); //NewButton("Run Project", Icons.Play, RunProjectPressed);
            s2.Hexpand = true;
            b.PackStart(newProj, false, false, 0);
            newProj.Show();
            b.PackStart(open, false, false, 0);
            open.Show();
            b.PackStart(s1, false, false, 10);
            s1.Show();
            b.PackStart(export, false, false, 0);
            export.Show();
            b.PackStart(s2, false, false, 10);
            s2.Show();
            b.PackStart(run, false, false, 0);
            run.Show();

            return b;
        }

        private void NewProjectPressed()
        {
            if (!EnsureNobodyDirty()) return;

            using var dialog = new NewProjectDialog(_editor, this);

            if (dialog.RunUntilAccept())
            {
                // Create project. No problems found.
                _editor.ResourceWindowManager.ForceCloseAllWindows();

                // Create path
                var folderToCreate = dialog.GetTargetPath();
                Directory.CreateDirectory(folderToCreate);

                var projectPath = folderToCreate + "/project.json";

                var newProject = new ProjectData();
                newProject.Name = dialog.ProjectTitle;
                newProject.Author = dialog.Author;

                // Create json.txt
                ProjectData.WriteToFile(projectPath, newProject);

                // Create icon if we specified one.
                if (dialog.IconPath != null) File.Copy(dialog.IconPath, folderToCreate + "/icon.png");

                // We did it! Now Load.
                _editor.LoadProject(projectPath);
            }
        }

        private void OpenProjectPressed()
        {
            if (!EnsureNobodyDirty()) return;

            using var chooser = new FileChooserDialog("Open Project", this, FileChooserAction.Open,
                "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
            chooser.Filter = new FileFilter {Name = "DR Project File"};
            chooser.Filter.AddPattern("project.json");

            chooser.SetCurrentFolder(new EnginePath("projects"));

            if ((ResponseType) chooser.Run() == ResponseType.Accept)
            {
                _editor.ResourceWindowManager.ForceCloseAllWindows();

                _editor.LoadProject(chooser.Filename);
            }
        }

        private void ExportProjectPressed()
        {
            AlertProblem("This feature is not implemented yet. Sorry!");
        }

        private Button NewButton(string name, Pixbuf icon, Action onPress = null)
        {
            var result = new Button();
            result.TooltipText = name;

            result.Pressed += (sender, args) => { onPress?.Invoke(); };
            //file.Label = "F";
            if (icon == null)
                result.Label = name;
            else
                result.Image = new Image(icon);

            return result;
        }

        private bool EnsureNobodyDirty()
        {
            if (_editor.ResourceWindowManager.AnyWindowDirty())
            {
                var message = "Some open resources have unsaved changes, Load anyway and discard changes?";

                return AreYouSureDialog.Run(this, "Unsaved Changes", message, "Load and Discard Changes");
            }

            return true;
        }

        #endregion

        #region Garbage stuff

        public DREditorMainWindow(IntPtr raw) : base(raw)
        {
            Debug.LogError("Invalid constructor.");
        }

        public DREditorMainWindow() : base("Invalid")
        {
            Debug.LogError("Invalid constructor.");
        }

        #endregion
    }
}