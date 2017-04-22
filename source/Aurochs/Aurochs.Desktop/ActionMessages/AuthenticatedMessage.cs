using Infrastructure.Flux.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurochs.Core.Entities;

namespace Aurochs.Desktop.ActionMessages
{
    public class AuthenticatedMessage : ActionMessage
    {
        public Auth Auth { get; set; }

        public ApplicationRegistration Registration { get; set; }

        public AuthenticatedMessage(ApplicationRegistration reg, Auth auth)
        {
            this.Registration = reg;
            this.Auth = auth;
        }
    }
}
