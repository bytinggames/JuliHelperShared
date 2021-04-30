make automatic content building work:
1. copy PrepareForBuild.ps1 to your project directory
2. add the following lines to that projects .csproj file:
"
<!-- added next two tags for automatic content generation -->
<Target Name="PrepareForBuildCustom" BeforeTargets="PrepareForBuild" Condition=" '$(Configuration)' == 'Release' ">
	<Exec Command="Powershell.exe -executionpolicy remotesigned -File PrepareForBuild.ps1 &quot;$(DevEnvDir)&quot;" />
</Target>
<ItemGroup>
	<EmbeddedResource Include="Content\ContentListGenerated_do-not-edit.txt" Condition=" '$(Configuration)' == 'Release' " />
</ItemGroup>
"
3. old Content.mgcb can be deleted. But it can also persist if you want to build custom data from directories other than Fonts, Music, Sounds and Textures