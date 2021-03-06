using System.IO;
using DREngine.ResourceLoading;
using Gtk;

namespace DREngine.Editor.Components
{
    public abstract class NewItemDialog : EasyDialog
    {
        private readonly ProjectPath _parentDirectory;

        // ReSharper disable once UnassignedField.Global
        public new string Name;

        //public Path NewFolderPath => FailureString == null? (_folderParent + _input.Value) : null;

        public NewItemDialog(DREditor editor, Window parent, ProjectPath parentDirectory, string title = "New Folder") :
            base(editor, parent, title)
        {
            _parentDirectory = parentDirectory;
        }

        protected abstract string ItemName { get; }

        protected override void OnModified()
        {
            if (GetTargetDirectory() == null)
            {
                SetFailure("No import file set.");
                return;
            }

            var displayPath = GetTargetDirectory().GetShortName();
            if (Directory.Exists(GetTargetDirectory()) || File.Exists(GetTargetDirectory()))
                SetFailure($"File already exists here: {displayPath}");
            else if (Name == null || Name.Trim() == "")
                SetFailure($"{ItemName} Name can't be empty!");
            else
                SetPostText($"New {ItemName}: {displayPath}");
        }

        protected override bool CheckForFailuresPreSubmit()
        {
            var path = GetTargetDirectory();
            if (!Directory.GetParent(path).Exists)
            {
                SetFailure($"Parent Directory doesn't exist at {Directory.GetParent(path).FullName}!");
                return false;
            }

            if (Directory.Exists(path))
            {
                SetFailure($"{ItemName} already exists at {path}.");
                return false;
            }

            return true;
        }

        public virtual ProjectPath GetTargetDirectory()
        {
            if (Name == null) return null;
            return (ProjectPath) (_parentDirectory + "/" + Name);
        }
    }
}