using System;
using System.Collections.Generic;
using System.Text;

namespace Aurochs.Core.Entities
{
    public class ApplicationRegistration
    {
        public long Id { get; set; }

        public string RedirectUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Instance { get; set; }

        public bool AllowRead { get; set; }

        public bool AllowWrite { get; set; }

        public bool AllowFollow { get; set; }
    }
}
