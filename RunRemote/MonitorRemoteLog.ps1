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
        $LogRoot = "C:\cellauto\Log";
        $date=$(((get-date).ToUniversalTime()).ToString("yyyy-MM-dd"));
        $LogPath="$LogRoot\$date";

        $StepFile="C:\cellauto\step.json";
        $content=( Get-Content -Path $StepFile -Raw )| ConvertFrom-Json ;
        $stepServerName=$stepcontent.ServerName;
        $stepClientName=$stepcontent.ClientName;

        function BeforeLogMonitorLoop
        {
            Param(
            [string]$stepName
            )
           #if no logfile in logpath
           while(!(Test-Path "$LogPath"))
           {
             sleep 10
           }
           while(!(Test-Path "$LogPath\$stepName.log"))
           {
             sleep 10
           }
        }


        function GetStepLogFile
        {
            Param(
            [string]$stepName
            )
   
            if((Test-Path "$LogPath\$stepName.log") -eq $null)
            {
                #Write-Host "$LogPath\$stepName.log"
                BeforeLogMonitorLoop $stepName
                #return $null;
            }
            if((Get-Content "$LogPath\$stepName.log"| where {$_ -match 'step execute success'}) -eq 'step execute success')
            {
                return $true;
            }
            else
            {
                return $false;
            }
        #get step log file if exist
        #get output
        }
        function LogMonitor
        {
            [string] $stepName;
            [bool] $IsStepExecuteSuccess;

            #BeforeLogMonitorLoop

            #loop output Step for return
            foreach ($step in ($content.Server.PSObject.Members | ?{ $_.MemberType -eq 'NoteProperty'})) 
            {
                 $stepName = $step.Name;
                 $IsStepExecuteSuccess = GetStepLogFile $stepName
                 if($IsStepExecuteSuccess -eq $null)
                 {
                    Write-Host Step execute error on $stepName
                 }
                 if($IsStepExecuteSuccess -eq $true)
                 {
                    Write-Host Step execute success: $stepName     
                 }
                 if($IsStepExecuteSuccess -eq $false)
                 {
                    #Write-Host Step execute: $stepName           
                    return $stepName          
                 }
            }

            foreach ($step in ($content.Client.PSObject.Members | ?{ $_.MemberType -eq 'NoteProperty'})) 
            {
                 $stepName = $step.Name;
                 $IsStepExecuteSuccess = GetStepLogFile $stepName
                 #if($IsStepExecuteSuccess -eq $null)
                 #{
                 #   Write-Host Step execute error on $stepName
                 #}
                 if($IsStepExecuteSuccess -eq $true)
                 {
                    Write-Host Step execute success: $stepName     
                 }
                 if($IsStepExecuteSuccess -eq $false)
                 {
                    #Write-Host Step execute: $stepName           
                    return $stepName          
                 }
            }
    

        }

        #output RunAll for return

        $StepExecute = LogMonitor
        while((Get-Content "$LogPath\RunAll.log") -eq $null)
        {
            sleep 10
        }
        $RunExcute=Get-Content "$LogPath\RunAll.log"
        Write-Host Step execute error: $RunExcute
       
        }  
    
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
 