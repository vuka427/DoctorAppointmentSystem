using DoctorAppointmentSystem.Models.DB;
using System.Web;
using System.Web.Optimization;

namespace DoctorAppointmentSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/responsive.bootstrap.min.css",
                      "~/Content/site.css"));

            //Config ScriptBundle and StyleBundle for sweetalert lib
            bundles.Add(new ScriptBundle("~/bundles/sweetalert/js").Include(
                     "~/lib/sweetalert2/sweetalert2.all.min.js"));
            bundles.Add(new StyleBundle("~/Content/sweetalert/css").Include(
               "~/lib/sweetalert2/sweetalert2.min.css"));


            //Config ScriptBundle and StyleBundle for jQuery Datatable
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/jquery.dataTables.min.js",
                 "~/Scripts/dataTables.responsive.js"));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                "~/Content/jquery.dataTables.min.css"));

            //Config ScriptBundle doctor management
            bundles.Add(new ScriptBundle("~/bundles/admin/doctor").Include(
                     "~/Scripts/js/doctormanagement.js"));
            //Config ScriptBundle patient management
            bundles.Add(new ScriptBundle("~/bundles/admin/patient").Include(
                     "~/Scripts/js/patientmanagement.js"));

        

        }
    }
}
