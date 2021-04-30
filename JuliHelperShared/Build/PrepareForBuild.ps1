# get to solution level
cd ..

$ide = $args[0];
$ide = $ide.Remove($ide.Length - 1);
echo $ide;
$csi = (get-item $ide).Parent.Parent.FullName;
$csi += "\MSBuild\Current\Bin\Roslyn\csi.exe";

$projects = Get-Content 'LD48.sln' |
  Select-String 'Project\(' |
    ForEach-Object {
      $projectParts = $_ -Split '[,=]' | ForEach-Object { $_.Trim('[ "{}]') };
      New-Object PSObject -Property @{
        Name = $projectParts[1];
        File = $projectParts[2];
        Guid = $projectParts[3]
      }
    }


$index = $projects.Name.IndexOf("JuliHelperShared");
$path = $projects.File[$index];
$path = (get-item $path).Directory.FullName;
$path += "\Build\PrepareForBuild.csx";
echo $path;

. "$csi" $path;