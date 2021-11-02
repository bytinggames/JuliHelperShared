using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JuliHelper
{
    public class AnimationData
    {
        public Dictionary<string, Frame> frames { get; set; }

        public int TotalDuration { get; private set; }

        public void Initialize()
        {
            int time = 0;
            foreach (var f in frames.Values)
            {
                f.timestamp = time;
                time += f.duration;
            }

            TotalDuration = time;
        }

        public Rectangle GetSourceRectangle(long time)
        {
            time %= TotalDuration;
            foreach (var f in frames.Values)
            {
                time -= f.duration;
                if (time < 0)
                    return f.rectangle;
            }

            throw new Exception();
        }

        public class Frame
        {
            public string name { get; set; }
            public Rectangle rectangle { get; set; }
            public int duration { get; set; }
            public int timestamp { get; internal set; }

            public Rect frame
            {
                set
                {
                    rectangle = new Rectangle(value.x, value.y, value.w, value.h);
                }
            }

        }

        public class Rect
        {
            public int x { get; set; }
            public int y { get; set; }
            public int w { get; set; }
            public int h { get; set; }
        }



        public static AnimationData FromJson(string json)
        {
            var data = JsonSerializer.Deserialize<AnimationData>(json);
            data.Initialize();
            return data;
        }

        static Dictionary<string, AnimationData> animationDatas = new Dictionary<string, AnimationData>();

        public static AnimationData GetAnimationData(ContentManager content, string assetName)
        {
            if (animationDatas.ContainsKey(assetName))
                return animationDatas[assetName];
            
            string file = Path.Combine(G.exeDir, content.RootDirectory, assetName.Replace('/', '\\') + ".json");
            string json = File.ReadAllText(file);
            AnimationData data = FromJson(json);
            animationDatas.Add(assetName, data);
            return data;
        }
    }

}
