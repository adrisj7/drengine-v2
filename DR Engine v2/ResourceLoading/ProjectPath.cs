﻿using DREngine.Editor;
using DREngine.Game;
using GameEngine.Game;

namespace DREngine.ResourceLoading
{
    public class ProjectPath : Path
    {
        private readonly DREditor _editor;
        private readonly DRGame _game;

        public ProjectPath(DRGame game, string path) : base(ParseRelativePath(path))
        {
            _game = game;
            _editor = null;
        }

        public ProjectPath(DREditor editor, string path) : base(ParseRelativePath(path))
        {
            _game = null;
            _editor = editor;
        }

        private static string ParseRelativePath(string path)
        {
            if (path.StartsWith("/")) path = path.Substring(1);
            return path;
        }

        public override string ToString()
        {
            return _game != null
                ? _game.GameData.GetFullProjectPath(RelativePath)
                : _editor.ProjectData.GetFullProjectPath(RelativePath);
        }

        public override string GetShortName()
        {
            return ProjectResourceConverter.RESOURCE_PATH_PREFIX + RelativePath;
        }

        protected override Path CreateNew(string relativePath)
        {
            if (_game != null)
                return new ProjectPath(_game, relativePath);
            return new ProjectPath(_editor, relativePath);
        }
    }
}