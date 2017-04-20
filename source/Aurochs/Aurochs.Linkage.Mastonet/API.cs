using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Linkage
{
    public static class API
    {
        public static async Task CreateApp(Action<string> openbrowser)
        {
            var reg = await MastodonClient.CreateApp("friends.nico", "Aurochs", Scope.Read | Scope.Write | Scope.Follow);

            var client = new MastodonClient(reg);
            var url = client.OAuthUrl();

            // test use
        }
    }
}
