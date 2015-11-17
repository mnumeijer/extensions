﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
    
    #line 1 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Engine;
    
    #line default
    #line hidden
    
    #line 5 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Engine.Operations;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 2 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Entities.Basics;
    
    #line default
    #line hidden
    
    #line 3 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Entities.DiffLog;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 4 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Web.Files;
    
    #line default
    #line hidden
    
    #line 6 "..\..\DiffLog\Views\OperationLog.cshtml"
    using Signum.Web.Translation;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/DiffLog/Views/OperationLog.cshtml")]
    public partial class _DiffLog_Views_OperationLog_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _DiffLog_Views_OperationLog_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 8 "..\..\DiffLog\Views\OperationLog.cshtml"
 using (var e = Html.TypeContext<OperationLogEntity>())
{
    e.LabelColumns = new BsColumn(4);

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 13 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.EntityLine(e, f => f.Operation));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.EntityLine(e, f => f.User));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 19 "..\..\DiffLog\Views\OperationLog.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 22 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.EntityLine(e, f => f.Target));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 25 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.EntityLine(e, f => f.Origin));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 28 "..\..\DiffLog\Views\OperationLog.cshtml"
    

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 31 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.ValueLine(e, f => f.Start));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"col-sm-6\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 34 "..\..\DiffLog\Views\OperationLog.cshtml"
       Write(Html.ValueLine(e, f => f.End));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n");

            
            #line 37 "..\..\DiffLog\Views\OperationLog.cshtml"
    
    e.LabelColumns = new BsColumn(2);
    if (e.Value.Exception != null)
    {
    
            
            #line default
            #line hidden
            
            #line 41 "..\..\DiffLog\Views\OperationLog.cshtml"
Write(Html.EntityLine(e, f => f.Exception));

            
            #line default
            #line hidden
            
            #line 41 "..\..\DiffLog\Views\OperationLog.cshtml"
                                         
    }
    

            
            #line default
            #line hidden
WriteLiteral(@"    <style>
        .colorIcon
        {
            color: black;
            padding: 2px;
        }

            .colorIcon.red
            {
                background: #FF8B8B;
            }

            .colorIcon.mini.red
            {
                background: #FFD1D1;
            }

            .colorIcon.green
            {
                background: #72F272;
            }

            .colorIcon.mini.green
            {
                background: #CEF3CE;
            }

        .nav-tabs > li.linkTab > a:hover
        {
            border-color: transparent;
            background-color: transparent;
        }
    </style>
");

            
            #line 77 "..\..\DiffLog\Views\OperationLog.cshtml"

    using (var diff = e.SubContext(a => a.Mixin<DiffLogMixin>()))
    {
        var minMax = Signum.Engine.DiffLog.DiffLogLogic.OperationLogNextPrev(e.Value);
        
        
        using (var tabs = Html.Tabs(e))
        {
            if (diff.Value.InitialState != null)
            {
                var prev = minMax.Min;

                if (prev != null && prev.Mixin<DiffLogMixin>().FinalState != null)
                {
                    tabs.Tab(new Signum.Web.DiffLog.LinkTab( 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, " \r\n<span>");

            
            #line 92 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, DiffLogMessage.PreviousLog.NiceToString());

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteLiteralTo(__razor_template_writer, " class=\"glyphicon glyphicon-new-window\"");

WriteLiteralTo(__razor_template_writer, "/></span> \r\n");

WriteLiteralTo(__razor_template_writer, " ");

})
            
            #line 94 "..\..\DiffLog\Views\OperationLog.cshtml"
        , Navigator.NavigateRoute(prev)) {  ToolTip = DiffLogMessage.NavigatesToThePreviousOperationLog.NiceToString() });
                    
                    var eq = prev.Mixin<DiffLogMixin>().FinalState == diff.Value.InitialState;

                    tabs.Tab(new Tab("diffPrev", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 2720), Tuple.Create("\"", 2795)
, Tuple.Create(Tuple.Create("", 2728), Tuple.Create("glyphicon", 2728), true)
, Tuple.Create(Tuple.Create(" ", 2737), Tuple.Create("glyphicon-fast-backward", 2738), true)
, Tuple.Create(Tuple.Create(" ", 2761), Tuple.Create("colorIcon", 2762), true)
, Tuple.Create(Tuple.Create(" ", 2771), Tuple.Create("red", 2772), true)
            
            #line 99 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 2775), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 2776), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 2815), Tuple.Create("\"", 2892)
, Tuple.Create(Tuple.Create("", 2823), Tuple.Create("glyphicon", 2823), true)
, Tuple.Create(Tuple.Create(" ", 2832), Tuple.Create("glyphicon-step-backward", 2833), true)
, Tuple.Create(Tuple.Create(" ", 2856), Tuple.Create("colorIcon", 2857), true)
, Tuple.Create(Tuple.Create(" ", 2866), Tuple.Create("green", 2867), true)
            
            #line 100 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 2872), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 2873), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    ");

})
            
            #line 101 "..\..\DiffLog\Views\OperationLog.cshtml"
           , 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre>");

            
            #line 102 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, TranslationClient.Diff(prev.Mixin<DiffLogMixin>().FinalState, diff.Value.InitialState));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</pre>");

})
            
            #line 102 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                                                       ) { ToolTip = DiffLogMessage.DifferenceBetweenFinalStateOfPreviousLogAndTheInitialState.NiceToString() });
                }

                tabs.Tab(new Tab("initialGraph", Html.PropertyNiceName(() => diff.Value.InitialState), 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 106 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, diff.Value.InitialState);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 106 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                  ) { ToolTip = DiffLogMessage.StateWhenTheOperationStarted.NiceToString() });
            }

            if (diff.Value.InitialState != null && diff.Value.FinalState != null)
            {
                var eq = diff.Value.InitialState == diff.Value.FinalState;

                tabs.Tab(new Tab("diff", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 3651), Tuple.Create("\"", 3726)
, Tuple.Create(Tuple.Create("", 3659), Tuple.Create("glyphicon", 3659), true)
, Tuple.Create(Tuple.Create(" ", 3668), Tuple.Create("glyphicon-step-backward", 3669), true)
, Tuple.Create(Tuple.Create(" ", 3692), Tuple.Create("colorIcon", 3693), true)
, Tuple.Create(Tuple.Create(" ", 3702), Tuple.Create("red", 3703), true)
            
            #line 114 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 3706), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 3707), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 3746), Tuple.Create("\"", 3822)
, Tuple.Create(Tuple.Create("", 3754), Tuple.Create("glyphicon", 3754), true)
, Tuple.Create(Tuple.Create(" ", 3763), Tuple.Create("glyphicon-step-forward", 3764), true)
, Tuple.Create(Tuple.Create(" ", 3786), Tuple.Create("colorIcon", 3787), true)
, Tuple.Create(Tuple.Create(" ", 3796), Tuple.Create("green", 3797), true)
            
            #line 115 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 3802), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 3803), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    ");

})
            
            #line 116 "..\..\DiffLog\Views\OperationLog.cshtml"
           , 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre>");

            
            #line 117 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, TranslationClient.Diff(diff.Value.InitialState, diff.Value.FinalState));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</pre>");

})
            
            #line 117 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                                       ) { Active = true, ToolTip = DiffLogMessage.DifferenceBetweenInitialStateAndFinalState.NiceToString() });
            }

            if (diff.Value.FinalState != null)
            {
                tabs.Tab(new Tab("FinalState", Html.PropertyNiceName(() => diff.Value.FinalState), 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre><code>");

            
            #line 123 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, diff.Value.FinalState);

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</code></pre>");

})
            
            #line 123 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                ) { ToolTip = DiffLogMessage.StateWhenTheOperationFinished.NiceToString() });

                var next = minMax.Max;

                if (next != null && next.Mixin<DiffLogMixin>().InitialState != null)
                {
                    var eq = diff.Value.FinalState == next.Mixin<DiffLogMixin>().InitialState;

                    tabs.Tab(new Tab("diffNext", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 4680), Tuple.Create("\"", 4754)
, Tuple.Create(Tuple.Create("", 4688), Tuple.Create("glyphicon", 4688), true)
, Tuple.Create(Tuple.Create(" ", 4697), Tuple.Create("glyphicon-step-forward", 4698), true)
, Tuple.Create(Tuple.Create(" ", 4720), Tuple.Create("colorIcon", 4721), true)
, Tuple.Create(Tuple.Create(" ", 4730), Tuple.Create("red", 4731), true)
            
            #line 132 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 4734), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 4735), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 4774), Tuple.Create("\"", 4850)
, Tuple.Create(Tuple.Create("", 4782), Tuple.Create("glyphicon", 4782), true)
, Tuple.Create(Tuple.Create(" ", 4791), Tuple.Create("glyphicon-fast-forward", 4792), true)
, Tuple.Create(Tuple.Create(" ", 4814), Tuple.Create("colorIcon", 4815), true)
, Tuple.Create(Tuple.Create(" ", 4824), Tuple.Create("green", 4825), true)
            
            #line 133 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 4830), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 4831), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    ");

})
            
            #line 134 "..\..\DiffLog\Views\OperationLog.cshtml"
           , 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre>");

            
            #line 135 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, TranslationClient.Diff(diff.Value.FinalState, next.Mixin<DiffLogMixin>().InitialState));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</pre>");

})
            
            #line 135 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                                                       ) { ToolTip = DiffLogMessage.DifferenceBetweenFinalStateAndTheInitialStateOfNextLog.NiceToString() });

                    tabs.Tab(new Signum.Web.DiffLog.LinkTab( 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, " \r\n<span>");

            
            #line 138 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, DiffLogMessage.NextLog.NiceToString());

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteLiteralTo(__razor_template_writer, " class=\"glyphicon glyphicon-new-window\"");

WriteLiteralTo(__razor_template_writer, "/></span> \r\n");

WriteLiteralTo(__razor_template_writer, " ");

})
            
            #line 140 "..\..\DiffLog\Views\OperationLog.cshtml"
        , Navigator.NavigateRoute(next)) { ToolTip = DiffLogMessage.NavigatesToTheNextOperationLog.NiceToString() });
                }
                else
                {
                    var entity = (Lite<Entity>)e.Value.Target;

                    var dump = !entity.Exists() ? null : entity.Retrieve().Dump();

                    var eq = diff.Value.FinalState == dump;

                    tabs.Tab(new Tab("diffCurrent", 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 5726), Tuple.Create("\"", 5800)
, Tuple.Create(Tuple.Create("", 5734), Tuple.Create("glyphicon", 5734), true)
, Tuple.Create(Tuple.Create(" ", 5743), Tuple.Create("glyphicon-step-forward", 5744), true)
, Tuple.Create(Tuple.Create(" ", 5766), Tuple.Create("colorIcon", 5767), true)
, Tuple.Create(Tuple.Create(" ", 5776), Tuple.Create("red", 5777), true)
            
            #line 151 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 5780), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 5781), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    <span");

WriteAttributeTo(__razor_template_writer, "class", Tuple.Create(" class=\"", 5820), Tuple.Create("\"", 5896)
, Tuple.Create(Tuple.Create("", 5828), Tuple.Create("glyphicon", 5828), true)
, Tuple.Create(Tuple.Create(" ", 5837), Tuple.Create("glyphicon-fast-forward", 5838), true)
, Tuple.Create(Tuple.Create(" ", 5860), Tuple.Create("colorIcon", 5861), true)
, Tuple.Create(Tuple.Create(" ", 5870), Tuple.Create("green", 5871), true)
            
            #line 152 "..\..\DiffLog\Views\OperationLog.cshtml"
, Tuple.Create(Tuple.Create(" ", 5876), Tuple.Create<System.Object, System.Int32>(eq ? "mini" : ""
            
            #line default
            #line hidden
, 5877), false)
);

WriteLiteralTo(__razor_template_writer, "></span>\r\n    ");

})
            
            #line 153 "..\..\DiffLog\Views\OperationLog.cshtml"
           , 
    
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, "<pre>");

            
            #line 154 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, TranslationClient.Diff(diff.Value.FinalState, dump));

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "</pre>");

})
            
            #line 154 "..\..\DiffLog\Views\OperationLog.cshtml"
                                                                    ) { ToolTip = DiffLogMessage.DifferenceBetweenFinalStateAndTheCurrentStateOfTheEntity.NiceToString() });

                    tabs.Tab(new Signum.Web.DiffLog.LinkTab( 
            
            #line default
            #line hidden
item => new System.Web.WebPages.HelperResult(__razor_template_writer => {

WriteLiteralTo(__razor_template_writer, " \r\n<span>");

            
            #line 157 "..\..\DiffLog\Views\OperationLog.cshtml"
WriteTo(__razor_template_writer, DiffLogMessage.CurrentEntity.NiceToString());

            
            #line default
            #line hidden
WriteLiteralTo(__razor_template_writer, "\r\n    <span");

WriteLiteralTo(__razor_template_writer, " class=\"glyphicon glyphicon-new-window\"");

WriteLiteralTo(__razor_template_writer, "/></span> \r\n");

WriteLiteralTo(__razor_template_writer, " ");

})
            
            #line 159 "..\..\DiffLog\Views\OperationLog.cshtml"
        , Navigator.NavigateRoute(e.Value.Target)) { ToolTip = DiffLogMessage.NavigatesToTheCurrentEntity.NiceToString() });
                }
            }
        }
    }
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
