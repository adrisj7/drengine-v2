﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using DREngine.Game;
using GameEngine;
using GameEngine.Game;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SpriteFontPlus;
using Path = GameEngine.Game.Path;

namespace DREngine
{
    /// <summary>
    /// A path to an "Overridable Default Resource" that is global for all projects, but CAN be overriden.
    /// </summary>
    public class OverridablePath
    {
        public string DefaultResourcePath { get; }
        public string OverrideProjectPath = null;
        [JsonIgnore]
        public bool Overrided => OverrideProjectPath != null;

        public OverridablePath(string defaultResourcePath)
        {
            DefaultResourcePath = defaultResourcePath;
        }

        public string GetFullPath(DRGame game)
        {
            if (Overrided)
            {
                return new ProjectPath(game, OverrideProjectPath);
            }
            return new DefaultResourcePath(DefaultResourcePath);
        }
    }

    /// <summary>
    /// For when we want to grab a default resource. Not used publicly.
    /// </summary>
    class DefaultResourcePath : Path
    {
        public const string DEFAULT_RESOURCE_FOLDER = "default_resources";

        public DefaultResourcePath(string path) : base(path) { }
        public override string ToString()
        {
            return $"{System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/{DEFAULT_RESOURCE_FOLDER}/{RelativePath}";
        }

        public override string GetShortName()
        {
            return ProjectResourceConverter.DEFAULT_RESOURCE_PATH_PREFIX + RelativePath;
        }
        protected override Path CreateNew(string relativePath)
        {
            return new DefaultResourcePath(relativePath);
        }
    }
}
