using System.IO;
using System.Collections.Generic;

string projectName = Args[0];

string contentDir = Path.Combine(Environment.CurrentDirectory, projectName, "Content");

string mgcbPath = Path.Combine(contentDir, "ContentGenerated_do-not-edit.mgcb");
string contentListPath = Path.Combine(contentDir, "ContentListGenerated_do-not-edit.txt");


string[] pngs = GetFiles("Textures", "png");
string[] soundsWav = GetFiles("Sounds", "wav");
string[] soundsOgg = GetFiles("Sounds", "ogg");
string[] songs = GetFiles("Music", "ogg");
string[] fonts = GetFiles("Fonts", "spritefont");

List<string> copies = new List<string>();
copies.AddRange(GetFiles("Fonts", "xnb"));
copies.AddRange(GetFiles("Textures", "json"));

string[] GetFiles(string name, string extension)
{
    string dir = Path.Combine(contentDir, name);
    if (!Directory.Exists(dir))
        return new string[0];
    return Directory.GetFiles(dir, "*." + extension, SearchOption.AllDirectories);
}


string[][] filesArray = new string[][]{
    pngs, soundsWav, soundsOgg, songs, fonts, copies.ToArray()
};


List<string> files = new List<string>();

for (int i = 0; i < filesArray.Length; i++)
{
    TrimPaths(filesArray[i]);
    files.AddRange(filesArray[i]);
}

void TrimPaths(string[] paths)
{
    for (int i = 0; i < paths.Length; i++)
    {
        paths[i] = paths[i].Substring(contentDir.Length + 1);
        paths[i] = paths[i].Replace("\\", "/");
    }
}


using (FileStream fs = File.Create(contentListPath))
using (StreamWriter sw = new StreamWriter(fs))
{
    for (int i = 0; i < files.Count; i++)
    {
        sw.WriteLine(files[i]);
    }
}

using (FileStream fs = File.Create(mgcbPath))
using (StreamWriter sw = new StreamWriter(fs))
{
    sw.WriteLine(@"
#----------------------------- Global Properties ----------------------------#

/outputDir:bin
/intermediateDir:obj
/platform:Windows
/config:
/profile:Reach
/compress:False

#-------------------------------- References --------------------------------#


#---------------------------------- Content ---------------------------------#
");

    foreach (var png in pngs)
    {
        sw.WriteLine($@"
#begin {png}
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyEnabled=False
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=False
/processorParam:MakeSquare=False
/processorParam:TextureFormat=Color
/build:{png}
");
    }

    foreach (var wav in soundsWav)
    {
        sw.WriteLine($@"
#begin {wav}
/importer:WavImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:{wav}
");
    }

    foreach (var ogg in soundsOgg)
    {
        sw.WriteLine($@"
#begin {ogg}
/importer:OggImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:{ogg}
");
    }

    foreach (var song in songs)
    {
        sw.WriteLine($@"
#begin {song}
/importer:OggImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:{song}

");
    }
    foreach (var font in fonts)
    {
        sw.WriteLine($@"
#begin {font}
/importer:FontDescriptionImporter
/processor:FontDescriptionProcessor
/processorParam:PremultiplyAlpha=True
/processorParam:TextureFormat=Compressed
/build:{font}
");
    }

    foreach (var copy in copies)
    {
        sw.WriteLine($@"
#begin {copy}
/copy:{copy}
");
    }
}


// TODO: delete generated content file after release?