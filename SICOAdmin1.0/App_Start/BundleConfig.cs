using System.Web;
using System.Web.Optimization;

namespace SICOAdmin1._0
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));
            //---------------------------------------------------------------
            /*
                <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
                <script src="~/Scripts/bootstrap.min.js"></script>
             @section styles{
    
    <script src=></script>
<link href= rel="stylesheet" />
}
             
             */
            bundles.Add(new StyleBundle("~/Vendors/css").Include(
                        "~/Vendors/bootstrap/dist/css/bootstrap.min.css",
                        "~/Vendors/fontawesome/css/font-awesome.min.css",
                        "~/Vendors/nprogress/nprogress.css",
                        "~/Content/css/style.css", 
                        "~/Content/css/sweetalert.min.css",
                        "~/Content/bootstrap4.min.css",//
                        "~/Content/gijgo.min.css",//
                        "~/Content/css/custom.min.css"
                        ));

            bundles.Add(new ScriptBundle("~/Vendors/js").Include(
                        "~/Vendors/jquery/jquery.min.js",
                        "~/Vendors/bootstrap/dist/js/bootstrap.bundle.min.js",
                        "~/Vendors/nprogress/nprogress.js",
                        "~/Scripts/sweetalert.min.js",
                        "~/Scripts/gijgo.min.js",//
                        "~/Scripts/jquery-3.3.1.min.js",//
                        "~/Content/js/custom.min.js"
                        ));

        }
    }
}
