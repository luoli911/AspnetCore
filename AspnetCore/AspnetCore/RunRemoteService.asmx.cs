using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Web.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AspnetCore
{
    /// <summary>
    /// RunRemoteService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class RunRemoteService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public void RunScript(string CellName, string ServerOS)
        {
            String Path = @"C:\RunRemote\RunRemoteVM.ps1";
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            using (PowerShell PowershellInstance = PowerShell.Create())
            {
                PowershellInstance.AddScript(Path+" " + CellName +" "+ServerOS);
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                Collection<PSObject> results = PowershellInstance.Invoke();
                Collection<ErrorRecord> errors = PowershellInstance.Streams.Error.ReadAll();
                
                foreach (var result in results)
                {
                    Debug.WriteLine("*********result*************");
                    Debug.WriteLine(result);
                  
                }
                foreach (var error in errors)
                {
                    Debug.WriteLine("********error**************");
                    Debug.WriteLine(error);
                   
                }
            }
            runspace.Close();
            
        }

        [WebMethod]
        public void MonitorRemoteLog(string CellName, string ServerOS)
        {
            String Path = @"C:\RunRemote\MonitorRemoteLog.ps1";
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            using (PowerShell PowershellInstance = PowerShell.Create())
            {
                PowershellInstance.AddScript(Path + " " + CellName + " " + ServerOS);
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                Collection<PSObject> results = PowershellInstance.Invoke();
                Collection<ErrorRecord> errors = PowershellInstance.Streams.Error.ReadAll();

                foreach (var result in results)
                {
                    Debug.WriteLine("*********result*************");
                    Debug.WriteLine(result);

                }
                foreach (var error in errors)
                {
                    Debug.WriteLine("********error**************");
                    Debug.WriteLine(error);

                }
            }
            runspace.Close();


        }

        [WebMethod]
        public string GetDateTime()
        {
            return DateTime.Now.ToString();
        }
    }

}
