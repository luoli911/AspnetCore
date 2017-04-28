using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class Step
    {
        [Key]
        [JsonIgnore]
        public int StepId { get; set; }
        [JsonIgnore]
        public int StepFileId { get; set; }
        public Server[] Server { get; set; }
        public Client[] Client { get; set; }
        public string ServerName { get; set; }
        public string ClientName { get; set; }
        public string DotnetVersion { get; set; }
        public string TestappName { get; set; }
        public string WebServer { get; set; }
        public string ServerOS { get; set; }
        [JsonIgnore]
        public virtual StepFile StepFile { get; set; }
    }

    public class Server
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Client
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}