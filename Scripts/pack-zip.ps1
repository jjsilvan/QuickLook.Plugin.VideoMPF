Remove-Item ..\QuickLook.Plugin.VideoMPF.qlplugin -ErrorAction SilentlyContinue

Get-ChildItem -Path ..\bin\Release\ -Exclude *.pdb,*.xml | Compress-Archive -DestinationPath ..\QuickLook.Plugin.VideoMPF.zip
Move-Item ..\QuickLook.Plugin.VideoMPF.zip ..\QuickLook.Plugin.VideoMPF.qlplugin