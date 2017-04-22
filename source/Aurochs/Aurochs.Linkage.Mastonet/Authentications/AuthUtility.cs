using Aurochs.Core.Entities;
using Mastonet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aurochs.Linkage.Converter;

namespace Aurochs.Linkage.Authentications
{
    public static class AuthUtility
    {
        public static async Task<ApplicationRegistration> GetRegistration(string instance)
        {
            var client = new AuthenticationClient(instance);
            var rawReg = await client.CreateApp("Aurochs", Scope.Read | Scope.Write | Scope.Follow);
            return rawReg.ToAppRegistration();
        }

        public static async Task<Auth> CreateAuthObjectWithPassword(string instanceUrl, string email, string password)
        {
            var authClient = new AuthenticationClient(instanceUrl);

            var appRegistration = await authClient.CreateApp("Aurochs", Scope.Read | Scope.Write | Scope.Follow);
            var auth = await authClient.ConnectWithPassword(email, password);

            return auth.ToAuth();
        }
    }
}
