
namespace Yumiko.SelfProtection.Script
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class ShellScript : Dictionary<string,string>
    {
        public List<string> Script { get; set; }
        private ProcessStartInfo info;
        public bool Exit { get; set; }
        public ShellScript(IEnumerable<string> script , bool exit = false):this(default(ProcessStartInfo),exit)
        {
            this.Script = script?.ToList() ?? new List<string>();
        }


        public Process Process { get; private set; }
        internal ShellScript(ProcessStartInfo info , bool exit = false)
        {
            this.Exit = exit;
            if (!info?.FileName.Contains("cmd") ?? true)
                this.info = new ProcessStartInfo
                {
                    ErrorDialog = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                };
        }

        public dynamic Run()
        {
            
            if (this.Process != null)
                return this.Process;
            var command = string.Join(" & ", this.Script.Select(n => this[n]));
            this.info.Arguments = command;
            if (!this.Exit)
                return this.Process = Process.Start(this.info);
            else
            {
                Process.Start(this.info);
                Environment.Exit(0);
                return null;
            }
        }
        
        // Retain
        public static ShellScript Erase
            => new ShellScript(new[] { "Delay" ,"Terminate" , "Delay" , "Delete" } , true)
            {
                ["Delay"] = "/C ping 1.1.1.1 -n 10 -w 1 > Nul",
                ["Terminate"] = $"taskkill /IM \"{AppDomain.CurrentDomain.FriendlyName}\"",
                ["Delete"] = $"del \"{AppDomain.CurrentDomain.FriendlyName}\""
            };
        
    }
}
