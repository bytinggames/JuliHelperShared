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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuliHelper
{
    public static class ContentFenja
    {
        private static Type[] GetNestedTypes(Type ofType) => ofType.GetNestedTypes();


        public static void LoadRaw(Type fieldContainingClass, string contentPath, string localPath, GraphicsDevice gDevice)
        {
            var fields = fieldContainingClass.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);


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

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    Texture2D newTex = Texture2D.FromStream(gDevice, stream);
                    newTex.Name = Path.Combine(localPath, fileName);
                    return newTex;
                }
            }
            

            var switchType = new Dictionary<Type, Action<FieldInfo>>
            {
                {
                    typeof(Texture2D), f =>
                    {
                        Texture2D newTex = GetTexture(f.Name + ".png");

                        Texture2D tex = f.GetValue(null) as Texture2D;

                        // is a texture already loaded?
                        if (tex == null)
                        {
                            f.SetValue(null, newTex);
                        }
                        else
                        {
                            tex.SetData(newTex.ToColor());
                        }
                    }
                },
                {
                    typeof(Texture2D[]), f =>
                    {
                        int index = 0;

                        List<Texture2D> newTexs = new List<Texture2D>();

                        string GetFileName() => f.Name + "_" + index + ".png";

                        while (File.Exists(GetPath(GetFileName())))
                        {
                            newTexs.Add(GetTexture(GetFileName()));
                            index++;
                        }


                        List<Texture2D> texs = (f.GetValue(null) as Texture2D[])?.ToList();
                        if (texs == null)
                        {
                            f.SetValue(null, newTexs.ToArray());
                        }
                        else
                        {
                            for (int i = 0; i < texs.Count; i++)
                            {
                                texs[i].SetData(newTexs[i].ToColor());
                            }
                        }
                    }
                },
                {
                    typeof(TextureAnim), f =>
                    {
                        Texture2D newTex = GetTexture(f.Name + ".png");

                        TextureAnim newAni;
                        string aniPath = GetPath(f.Name + ".ani");
                        if (!File.Exists(aniPath))
                        {
                            newAni = new TextureAnim(newTex);
                        }
                        else
                        {
                            newAni = new TextureAnim(newTex, File.ReadAllLines(aniPath));
                        }

                        TextureAnim ani = f.GetValue(null) as TextureAnim;

                        // is a texture already loaded?
                        if (ani == null)
                        {
                            f.SetValue(null, newAni);
                        }
                        else
                        {
                            newAni.CopySettingsTo(ani);
                        }
                    }
                },
                {
                    typeof(TextureAnim[]), f =>
                    {
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
                                newAnims.Add(new TextureAnim(newTexs[i]));
                            else
                                newAnims.Add(new TextureAnim(newTexs[i], File.ReadAllLines(aniPath)));
                        }

                        List<TextureAnim> anims = (f.GetValue(null) as TextureAnim[])?.ToList();
                        if (anims == null)
                        {
                            f.SetValue(null, newAnims.ToArray());
                        }
                        else
                        {
                            for (int i = 0; i < anims.Count; i++)
                            {
                                newAnims[i].CopySettingsTo(anims[i]);
                            }
                        }
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
    }
}
