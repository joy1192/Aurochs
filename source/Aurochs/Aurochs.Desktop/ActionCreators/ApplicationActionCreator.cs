using Aurochs.Core.Entities;
using Aurochs.Desktop.ActionMessages;
using Infrastructure.Flux.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ActionCreators
{
    public static class ApplicationActionCreator
    {
        public static void StartApplication()
        {
            Task.Run(() =>
            {
                try
                {
                    var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var path = Path.Combine(root, "Aurochs");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Auth auth;
                    ApplicationRegistration reg;
                    {
                        var filepath = Path.Combine(path, "auth.json");
                        var json = File.ReadAllText(filepath);
                        auth = JsonConvert.DeserializeObject<Auth>(json);
                    }
                    {
                        var filepath = Path.Combine(path, "reg.json");
                        var json = File.ReadAllText(filepath);
                        reg = JsonConvert.DeserializeObject<ApplicationRegistration>(json);
                    }

                    Dispatcher.Default.Invoke(new AuthenticatedMessage(reg, auth));
                }
                catch (Exception e)
                {
                    Dispatcher.Default.Invoke(new UnauthenticatedMessage());
                }
            });
        }

        public static void WakeUpApplication()
        {
            Task.Run(() =>
            {
                Dispatcher.Default.Invoke(new WakeUpApplicationMessage());
            });
        }
    }
}
