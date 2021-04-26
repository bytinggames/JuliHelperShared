using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using JuliHelperShared;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper
{
    public static class ContentFenja
    {
        private static Type[] GetNestedTypes(Type ofType) => ofType.GetNestedTypes();
        private static DateTime exeBuildTime;

        // variable, lastWrittenTo
        static Dictionary<string, DateTime> changedLoaded = new Dictionary<string, DateTime>();

        public static void Initialize(DateTime _exeBuildDate)
        {
            exeBuildTime = _exeBuildDate;
        }

        static bool IsNew(string path)
        {
            DateTime fileTime = new FileInfo(path).LastWriteTimeUtc;
            if (changedLoaded.ContainsKey(path))
            {
                if (fileTime > changedLoaded[path])
                {
                    changedLoaded[path] = fileTime;
                    return true;
                }
            }
            else
            {
                if (fileTime > exeBuildTime)
                {
                    changedLoaded.Add(path, fileTime);
                    return true;
                }
            }
            return false;
        }

        public static void LoadRaw(Type fieldContainingClass, string contentPath, string localPath, GraphicsDevice gDevice)
        {
            var fields = fieldContainingClass.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            bool checkIfNew = false;


            string GetPath(string fileName)
            {
                return Path.Combine(contentPath, localPath, fileName);
            }

            Texture2D GetTexture(string fileName)
            {
                string path = GetPath(fileName);
                if (!File.Exists(path))
                {
                    string[] files = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(path), fileName + "#*.png");
                    if (files.Length != 1)
                        throw new Exception(files.Length + " matching files found for " + fileName);

                    path = files[0];
                }

                if (checkIfNew && !IsNew(path))
                    return null;

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    Texture2D newTex = Texture2D.FromStream(gDevice, stream);
                    newTex.Name = Path.Combine(localPath, fileName);
                    return newTex;
                }
            }

            SoundEffect GetSound(string fileName)
            {
                string path = GetPath(fileName);
                if (!File.Exists(path))
                    throw new Exception("Sound file not found: " + fileName);

                if (checkIfNew && !IsNew(path))
                    return null;

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    SoundEffect newSound = SoundEffect.FromStream(stream);
                    newSound.Name = Path.Combine(localPath, fileName);
                    return newSound;
                }
            }

            SoundEffect[] GetSounds(string directoryName)
            {
                string path = GetPath(directoryName);
                if (!Directory.Exists(path))
                    throw new Exception("Directory could not be found: " + directoryName);

                if (checkIfNew && !IsNew(path))
                    return null;

                string[]files = Directory.GetFiles(path, "*.wav", SearchOption.TopDirectoryOnly);

                List<SoundEffect> sounds = new List<SoundEffect>();
                foreach (var file in files)
                {
                    using (FileStream stream = new FileStream(file, FileMode.Open))
                    {
                        SoundEffect newSound = SoundEffect.FromStream(stream);
                        newSound.Name = Path.Combine(localPath, directoryName);
                        sounds.Add(newSound);
                    }
                }
                return sounds.ToArray();
            }


            var switchType = new Dictionary<Type, Action<FieldInfo>>
            {
                {
                    typeof(Texture2D), f =>
                    {
                        Texture2D newTex = GetTexture(f.Name + ".png");

                        if (newTex == null)
                            return;

                        Texture2D tex = f.GetValue(null) as Texture2D;
                        if (tex == null)
                            f.SetValue(null, newTex);
                        else
                            tex.SetData(newTex.ToColor());
                    }
                },
                {
                    typeof(Texture2D[]), f =>
                    {
                        List<Texture2D> texs = (f.GetValue(null) as Texture2D[])?.ToList();
                        if (texs == null)
                            return;

                        int index = 0;

                        List<Texture2D> newTexs = new List<Texture2D>();

                        string GetFileName() => f.Name + "_" + index + ".png";

                        while (File.Exists(GetPath(GetFileName())))
                        {
                            newTexs.Add(GetTexture(GetFileName()));
                            index++;
                        }

                        if (newTexs.All(g => g == null))
                            return;

                        for (int i = 0; i < texs.Count; i++)
                        {
                            if (newTexs[i] != null)
                                texs[i].SetData(newTexs[i].ToColor());
                        }
                    }
                },
                {
                    typeof(TextureAnim), f =>
                    {
                        TextureAnim ani = f.GetValue(null) as TextureAnim;
                        if (ani == null)
                            return;

                        Texture2D newTex = GetTexture(f.Name + ".png");

                        TextureAnim newAni;
                        string aniPath = GetPath(f.Name + ".ani");
                        if (!File.Exists(aniPath))
                        {
                            if (newTex == null)
                                return;
                            newAni = new TextureAnim(ani.Texture);
                        }
                        else
                        {
                            if (newTex == null && !IsNew(aniPath))
                                return;

                            newAni = new TextureAnim(newTex ?? ani.Texture, File.ReadAllLines(aniPath));
                        }

                        newAni.CopySettingsTo(ani);
                    }
                },
                {
                    typeof(TextureAnim[]), f =>
                    {
                        List<TextureAnim> anims = (f.GetValue(null) as TextureAnim[])?.ToList();
                        if (anims == null)
                            return;

                        int index = 0;

                        List<Texture2D> newTexs = new List<Texture2D>();

                        string GetFileName() => f.Name + "_" + index + ".png";

                        while (File.Exists(GetPath(GetFileName())))
                        {
                            newTexs.Add(GetTexture(GetFileName()));
                            index++;
                        }

                        List<TextureAnim> newAnims = new List<TextureAnim>();

                        for (int i = 0; i < newTexs.Count; i++)
                        {
                            string aniPath = GetPath(f.Name + "_" + i + ".ani");
                            if (!File.Exists(aniPath))
                                newAnims.Add(new TextureAnim(newTexs[i] ?? anims[i].Texture));
                            else
                            {
                                if (newTexs[i] == null && !IsNew(aniPath))
                                    newAnims.Add(null);
                                else
                                    newAnims.Add(new TextureAnim(newTexs[i] ?? anims[i].Texture, File.ReadAllLines(aniPath)));
                            }
                        }

                        if (newAnims.All(g => g == null))
                            return;

                        for (int i = 0; i < anims.Count; i++)
                        {
                            if (newAnims[i] != null)
                                newAnims[i].CopySettingsTo(anims[i]);
                        }
                    }
                },
                {
                    typeof(SoundItem), f =>
                    {
                        SoundEffect newSound = GetSound(f.Name + ".wav");

                        if (newSound == null)
                            return;

                        SoundItem sound = f.GetValue(null) as SoundItem;
                        if (sound == null)
                            f.SetValue(null, new SoundItem(newSound));
                        else
                            sound.SoundEffect = newSound;
                    }
                },
                {
                    typeof(SoundItemCollection), f =>
                    {
                        SoundEffect[] newSounds = GetSounds(f.Name);

                        if (newSounds == null)
                            return;

                        SoundItemCollection sound = f.GetValue(null) as SoundItemCollection;
                        if (sound == null)
                            f.SetValue(null, new SoundItemCollection(newSounds));
                        else
                            sound.SoundEffects = newSounds;
                    }
                },
            };

            for (int i = 0; i < fields.Length; i++)
            {
                if (switchType.ContainsKey(fields[i].FieldType))
                        switchType[fields[i].FieldType](fields[i]);
            }

            Type[] nested = GetNestedTypes(fieldContainingClass);
            for (int i = 0; i < nested.Length; i++)
            {
                LoadRaw(nested[i], contentPath, Path.Combine(localPath, nested[i].Name), gDevice);
            }
        }

        public static void LoadProcessed(Type fieldContainingClass, string localPath, ContentManager content)
        {
            var fields = fieldContainingClass.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            var switchType = new Dictionary<Type, Action<FieldInfo>>
            {
                {
                    typeof(Texture2D), f =>
                    {
                        f.SetValue(null, content.Load<Texture2D>(Path.Combine(localPath, f.Name)));
                    }
                },
                {
                    typeof(Texture2D[]), f =>
                    {
                        List<Texture2D> texs = new List<Texture2D>();
                        int index = 0;
                        string name = f.Name + "_" + index;

                        while (File.Exists(Path.Combine("Content", localPath,(name = f.Name + "_"+index)+".xnb")))
                        {
                            texs.Add(content.Load<Texture2D>(Path.Combine(localPath, name)));
                            index++;
                        }
                        f.SetValue(null, texs.ToArray());
                    }
                },
                {
                    typeof(SpriteFont), f =>
                    {
                        f.SetValue(null, content.Load<SpriteFont>(Path.Combine(localPath, f.Name)));
                    }
                },
                {
                    typeof(TextureAnim), f =>
                    {
                        Texture2D tex = content.Load<Texture2D>(Path.Combine(localPath, f.Name));
                        TextureAnim ani;
                        string path = Path.Combine("Content", localPath,f.Name + ".ani");
                        if (File.Exists(path))
                            ani = new TextureAnim(tex, File.ReadAllLines(path));
                        else
                            ani = new TextureAnim(tex);
                    }
                },
                {
                    typeof(TextureAnim[]), f =>
                    {
                        List<TextureAnim> anis = new List<TextureAnim>();
                        int index = 0;
                        string name = f.Name + "_" + index;

                        while (File.Exists(Path.Combine("Content", localPath,(name = f.Name + "_"+index)+".xnb")))
                        {
                            Texture2D tex = content.Load<Texture2D>(Path.Combine(localPath, name));

                        TextureAnim ani;
                        string path = Path.Combine("Content", localPath,f.Name + "_" + index + ".ani");
                        if (File.Exists(path))
                            ani = new TextureAnim(tex, File.ReadAllLines(path));
                        else
                            ani = new TextureAnim(tex);
                        anis.Add(ani);
                            index++;
                        }
                        f.SetValue(null, anis.ToArray());
                    }
                },
            };

            if (fields.Length > 0)
            {
                if (fields[0].GetValue(null) == null)
                {
                    // RELEASE: replace this with hard code
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (switchType.ContainsKey(fields[i].FieldType))
                            switchType[fields[i].FieldType](fields[i]);
                    }
                }
            }

            Type[] nested = GetNestedTypes(fieldContainingClass);
            for (int i = 0; i < nested.Length; i++)
            {
                LoadProcessed(nested[i], Path.Combine(localPath, nested[i].Name), content);
            }
        }

        public static void DisposeContent(Type fieldContainingClass)
        {
            var fields = fieldContainingClass.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            var switchType = new Dictionary<Type, Action<FieldInfo>>
            {
                {
                    typeof(Texture2D), f =>
                    {
                        Texture2D tex = f.GetValue(null) as Texture2D;
                        if (tex == null)
                            return;

                        tex.Dispose();
                    }
                },
                {
                    typeof(Texture2D[]), f =>
                    {
                        List<Texture2D> texs = (f.GetValue(null) as Texture2D[])?.ToList();
                        if (texs == null)
                            return;

                        for (int i = 0; i < texs.Count; i++)
                        {
                            texs[i].Dispose();
			            }
                    }
                },
                {
                    typeof(TextureAnim), f =>
                    {
                        TextureAnim ani = f.GetValue(null) as TextureAnim;
                        if (ani == null)
                            return;
                        ani.Dispose();
                    }
                },
                {
                    typeof(TextureAnim[]), f =>
                    {
                        List<TextureAnim> anims = (f.GetValue(null) as TextureAnim[])?.ToList();
                        if (anims == null)
                            return;
                        for (int i = 0; i < anims.Count; i++)
                        {
                            anims[i].Dispose();
			            }
                    }
                },
                {
                    typeof(SoundItem), f =>
                    {
                        var item = f.GetValue(null) as SoundItem;
                        if (item == null)
                            return;
                        item.Dispose();
                    }
                },
            };

            for (int i = 0; i < fields.Length; i++)
            {
                if (switchType.ContainsKey(fields[i].FieldType))
                    switchType[fields[i].FieldType](fields[i]);
            }

            Type[] nested = GetNestedTypes(fieldContainingClass);
            for (int i = 0; i < nested.Length; i++)
            {
                DisposeContent(nested[i]);
            }
        }
    }
}
