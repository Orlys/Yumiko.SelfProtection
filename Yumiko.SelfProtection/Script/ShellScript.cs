
namespace Yumiko.SelfProtection.Script
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ShellScript : Dictionary<string,string>
    {
        private ProcessStartInfo info;
        public ShellScript():this(new ProcessStartInfo
        {
            ErrorDialog = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
        }){}
        public Process Process { get; private set; }
        internal ShellScript(ProcessStartInfo info)
        {
            this.info = info ?? new ProcessStartInfo { FileName = "cmd.exe" };
        }

        public Process Run()
        {
            if (this.Process != null)
                return this.Process;
            var command = string.Join(" & ", this);
            this.info.Arguments = command;
            return this.Process = Process.Start(this.info);
        }


        public readonly static ShellScript SelfDelete
            = new ShellScript
            {
                ["Delay"] = "/C ping 1.1.1.1 -n 10 -w 1 > Nul",
                ["Taskkill"] = $"Taskkill /IM \"{AppDomain.CurrentDomain.FriendlyName}\"",
                ["Delete"] = $"Del \"{AppDomain.CurrentDomain.FriendlyName}\""
            };
    }
}
