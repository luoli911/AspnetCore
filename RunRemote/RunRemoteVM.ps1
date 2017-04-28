Param(
[string]$CellName,
[string]$ServerOS
)
$LogRoot = "C:\cellauto\Log";
$date=$(((get-date).ToUniversalTime()).ToString("yyyy-MM-dd"));
$LogPath="$LogRoot\$date";
Enable-PSRemoting -Force
$pass = ConvertTo-SecureString -AsPlainText 'Asplab@123' -Force
$Cred=New-Object System.Management.Automation.PSCredential -ArgumentList 'asplab',$pass
#$LinuxHostName="lcell1-win$i.centralus.cloudapp.azure.com"
#$WindowsHostName="wcell$i-win2.centralus.cloudapp.azure.com"
$winrmPort='5986'
$soptions=new-pssessionoption -SkipCACheck
#$ws='ws1','ws2','ws3','ws4','ws6','ws7','ws8','ws9'
#$ls='ls1','ls2','ls3','ls4','ls5','ls6','ls7','ls8','ls9','ls10','ls11','ls12'
#Invoke-Command -ComputerName $hostname -ScriptBlock {     Start-Process powershell.exe -ArgumentList ".\RunAll.ps1" -Wait -WorkingDirectory "C:\cellauto" -RedirectStandardOutput "C:\cellauto\RunAll.log"} -Credential $Cred -Authentication Credssp -Port $winrmPort;

function RunWinodowsVM
{
    Param(
    [string]$CellName
    )
    
    $WindowsHostName="$CellName-win2.centralus.cloudapp.azure.com"  
    
    $ws=New-PSSession -ComputerName $WindowsHostName -Port $winrmPort -Credential $cred -SessionOption $soptions -UseSSL -Authentication Credssp
    Invoke-Command -Session $ws -ScriptBlock{net use \\fastshare.file.core.windows.net\aspnet /user:fastshare 05oBpnhrbJVwZt6LsHUqXaqiyMA84ba3lenO2Ou1ShJq/Bhg4UbDceZ5R634ffa4t+Te5SJ38cmEoWhRzqu/cg==}
    Invoke-Command -session $ws -ScriptBlock{Start-Process powershell.exe -ArgumentList ".\RunAll.ps1" -Wait -WorkingDirectory "C:\cellauto" -RedirectStandardOutput "$LogPath\RunAll.log"} -AsJob
    
}
function RunLinuxVM
{
    Param(
    [string]$CellName
    )
    
    $LinuxHostName="$CellName-win1.centralus.cloudapp.azure.com"
    $ls=New-PSSession -ComputerName $LinuxHostName -Port $winrmPort -Credential $cred -SessionOption $soptions -UseSSL -Authentication Credssp
    Invoke-Command -Session $ls -ScriptBlock{net use \\fastshare.file.core.windows.net\aspnet /user:fastshare 05oBpnhrbJVwZt6LsHUqXaqiyMA84ba3lenO2Ou1ShJq/Bhg4UbDceZ5R634ffa4t+Te5SJ38cmEoWhRzqu/cg==}
    Invoke-Command -session $ls -ScriptBlock{
        param($RemoteLogPath=$LogPath)
        if(!(Test-Path $RemoteLogPath))
        {
            mkdir $RemoteLogPath
        }
    Start-Process powershell.exe -ArgumentList ".\RunAll.ps1" -Wait -WorkingDirectory "C:\cellauto" -RedirectStandardOutput "$RemoteLogPath\RunAll.log" -RedirectStandardError "$RemoteLogPath\RunAllError.log"
    } -ArgumentList $LogPath -AsJob
    
}
function RunDockerVM
{   
    Param(
    [string]$CellName
    )
    if($CellName -eq "lcell9" -or $CellName -eq "lcell10")
    { 
    $pass1 = ConvertTo-SecureString -AsPlainText 'Asplab@123' -Force
    $Cred=New-Object System.Management.Automation.PSCredential -ArgumentList 'asplab',$pass1
    $LinuxHostName="$CellName-win1.centralus.cloudapp.azure.com"
    $ls=New-PSSession -ComputerName $LinuxHostName -Port $winrmPort -Credential $cred -SessionOption $soptions -UseSSL -Authentication Credssp
    Invoke-Command -Session $ls -ScriptBlock{net use \\fastshare.file.core.windows.net\aspnet /user:fastshare 05oBpnhrbJVwZt6LsHUqXaqiyMA84ba3lenO2Ou1ShJq/Bhg4UbDceZ5R634ffa4t+Te5SJ38cmEoWhRzqu/cg==}
    Invoke-Command -session $ls -ScriptBlock{Start-Process powershell.exe -ArgumentList ".\RunAll.ps1" -Wait -WorkingDirectory "C:\cellauto" -RedirectStandardOutput "$LogPath\RunAll.log" -RedirectStandardError "$LogPath\RunAll_Error.log"} -AsJob
    }

    if($CellName -eq "lcell11" -or $CellName -eq "lcell12")
    {
        
    $pass2 = ConvertTo-SecureString -AsPlainText 'Asplab@123456' -Force
    $Cred=New-Object System.Management.Automation.PSCredential -ArgumentList 'asplab',$pass2
        
    $LinuxHostName="$CellName-win1.centralus.cloudapp.azure.com"
    $ls=New-PSSession -ComputerName $LinuxHostName -Credential $cred -SessionOption $soptions -Authentication Credssp
    Invoke-Command -Session $ls -ScriptBlock{net use \\fastshare.file.core.windows.net\aspnet /user:fastshare 05oBpnhrbJVwZt6LsHUqXaqiyMA84ba3lenO2Ou1ShJq/Bhg4UbDceZ5R634ffa4t+Te5SJ38cmEoWhRzqu/cg==}
    Invoke-Command -session $ls -ScriptBlock{Start-Process powershell.exe -ArgumentList ".\RunAll.ps1" -Wait -WorkingDirectory "C:\cellauto" -RedirectStandardOutput "$LogPath\RunAll.log"} -AsJob
    }
}
function RunRemoteVM
{
    if($ServerOS -eq "Linux")
    {
        RunLinuxVM $CellName
    }
    if($ServerOS -eq "Windows")
    {
        RunWinodowsVM $CellName
    }
    if($ServerOS -eq "Docker")
    {
        RunDockerVM $CellName
    }
}

RunRemoteVM
Get-PSSession
 #Get-PSSession | Remove-PSSession
 