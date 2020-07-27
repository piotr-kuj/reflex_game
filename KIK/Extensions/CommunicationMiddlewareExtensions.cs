using KIK.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIK.Extensions
{
    public static class CommunicationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCommunicationMiddleware(this IApplicationBuilder app) 
        {
            return app.UseMiddleware<CommunicationMiddleware>();
        }
    }
}
