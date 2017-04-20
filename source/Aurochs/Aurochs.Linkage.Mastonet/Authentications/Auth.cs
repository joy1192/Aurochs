using Aurochs.Core.Entities;
using Aurochs.Linkage.Converter;
using Mastonet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Linkage.Authentications
{
    public static class Auth
    {
        public static async Task<ApplicationRegistration> GetRegistration()
        {
            var rawReg = await MastodonClient.CreateApp("friends.nico", "Aurochs", Scope.Read | Scope.Write | Scope.Follow);
            return rawReg.ToAppRegistration();
        }
    }
}
