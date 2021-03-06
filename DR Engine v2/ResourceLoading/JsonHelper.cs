﻿using DREngine.Game;
using GameEngine.Game;
using GameEngine.Util;
using Newtonsoft.Json;

namespace DREngine.ResourceLoading
{
    public class JsonHelper
    {
        public static void SaveToJson<T>(T obj, Path path)
        {
            var text = JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                    {TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented});
            //Debug.LogSilent($"JSON: {text}");
            IOHelper.WriteTextFile(path, text);
        }

        public static T LoadFromJson<T>(DRGame game, Path path)
        {
            var text = IOHelper.ReadTextFile(path);
            IDependentOnDRGame.CurrentGame = game;
            return JsonConvert.DeserializeObject<T>(text,
                new JsonSerializerSettings
                    {TypeNameHandling = TypeNameHandling.Auto, Formatting = Formatting.Indented});
        }

        /*
        public static void SaveToProjectJson<T>(DRGame game, T obj, Path filepath)
        {
            SaveToJson(obj, game.GameData.GetFullProjectPath(filepath));
        }

        public static T LoadFromProjectJson<T>(DRGame game, Path filepath)
        {
            return LoadFromJson<T>(game, game.GameData.GetFullProjectPath(filepath));
        }
        */
    }
}