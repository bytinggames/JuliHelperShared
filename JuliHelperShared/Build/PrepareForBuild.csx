using System.IO;
using System.Collections.Generic;

string contentDir = Path.Combine(Environment.CurrentDirectory, "LD48", "Content");

//string contentDirRelease = Path.Combine(contentDir, "Release");
//if (!Directory.Exists(contentDirRelease))
//    Directory.CreateDirectory(contentDirRelease);

string mgcbPath = Path.Combine(contentDir, "ContentGenerated_do-not-edit.mgcb");
string contentListPath = Path.Combine(contentDir, "ContentListGenerated_do-not-edit.txt");


string[] pngs = Directory.GetFiles(Path.Combine(contentDir, "Textures"), "*.png", SearchOption.AllDirectories);
string[] wavs = Directory.GetFiles(Path.Combine(contentDir, "Sounds"), "*.wav", SearchOption.AllDirectories);
string[] songs = Directory.GetFiles(Path.Combine(contentDir, "Music"), "*.ogg", SearchOption.AllDirectories);
string[] copies = Directory.GetFiles(Path.Combine(contentDir, "Fonts"), "*.xnb", SearchOption.AllDirectories);


string[][] filesArray = new string[][]{
    pngs, wavs, songs, copies
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

    foreach (var wav in wavs)
    {
        sw.WriteLine($@"
#begin {wav}
/importer:WavImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:{wav}
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
    foreach (var copy in copies)
    {
        sw.WriteLine($@"
#begin {copy}
/copy:{copy}
");
    }
}


// TODO: delete generated content file after release?