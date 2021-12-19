using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DevFramework.Core.Aspect.Postsharp.AuthorizationAspects
{
    [Serializable]
    public class SecuredOperation : OnMethodBoundaryAspect
    {
        public string Roles { get; set; }
        public override void OnEntry(MethodExecutionArgs args)
        {
            string[] roles = Roles.Split(',');
            bool isAutorized = false;
            for (int i = 0; i < roles.Length; i++)
            {
                if (!System.Threading.Thread.CurrentPrincipal.IsInRole(roles[i])){
                    isAutorized = true;
                }
            }
            if (isAutorized == false)
            {
                throw new SecurityException("You are not authorized");
            }
        }
    }
}
