[CmdletBinding()]
param (
    [Parameter()]
    [bool]
    $play = $true
)

$GenshinTime = {}
if ($play)
{
  $GenshinTime = Measure-Command { Start-Process -FilePath 'F:\Games\Genshin Impact\Genshin Impact Game\GenshinImpact.exe' -Wait }
}

Add-Type -Path .\PlaytimeTrackerLib\bin\Release\PlaytimeTrackerLib.dll

$playtimeSave = (Get-Item .).FullName + "\GenshinTimes.dat"

$SavedPlaytimes = [PlaytimeTrackerLib.Playtime]::FromFile($playtimeSave);

if ($SavedPlaytimes -Eq $null)
{
  $SavedPlaytimes = [PlaytimeTrackerLib.Playtime]::new();
}

if ($play)
{
  $SavedPlaytimes.AddPlaytime($GenshinTime);
}

$SavedPlaytimes.WriteToFile($playtimeSave);

$SavedPlaytimes.GetPlaytimeEntries() | Format-Table @{L = "Playtime"; E = {$_.TimeSpan}}, Date

Write-Host "Total playtime: $($SavedPlaytimes.TotalTime())"

Read-Host "Press enter to continue..."