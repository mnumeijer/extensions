﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    
    #line 1 "..\..\Translation\Views\TranslatorUser.cshtml"
    using Signum.Entities.Translation;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Translation/Views/TranslatorUser.cshtml")]
    public partial class _Translation_Views_TranslatorUser_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Translation_Views_TranslatorUser_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Translation\Views\TranslatorUser.cshtml"
 using (var tc = Html.TypeContext<TranslatorUserEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 4 "..\..\Translation\Views\TranslatorUser.cshtml"
Write(Html.EntityLine(tc, t => t.User));

            
            #line default
            #line hidden
            
            #line 4 "..\..\Translation\Views\TranslatorUser.cshtml"
                                     
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Translation\Views\TranslatorUser.cshtml"
Write(Html.EntityRepeater(tc, t => t.Cultures, er => er.Move = true));

            
            #line default
            #line hidden
            
            #line 5 "..\..\Translation\Views\TranslatorUser.cshtml"
                                                                   
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
