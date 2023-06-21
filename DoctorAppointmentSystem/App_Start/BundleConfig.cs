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
                        "~/Scripts/bootstrap-lib-js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/bootstrap-lib-js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/bootstrap-lib-js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap-lib-js/bootstrap.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-lib/bootstrap.min.css",
                      "~/Content/bootstrap-lib/responsive.bootstrap.min.css",
                      "~/Content/css/Site.css"));

            //Config ScriptBundle and StyleBundle for sweetalert lib
            bundles.Add(new ScriptBundle("~/bundles/sweetalert/js").Include(
                     "~/lib/sweetalert2/sweetalert2.all.min.js"));
            bundles.Add(new StyleBundle("~/Content/sweetalert/css").Include(
               "~/lib/sweetalert2/sweetalert2.min.css"));

            //Config ScriptBundle and StyleBundle for jQuery Datatable
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/bootstrap-lib-js/jquery.dataTables.min.js",
                 "~/Scripts/bootstrap-lib-js/dataTables.responsive.js"));

            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                "~/Content/bootstrap-lib/jquery.dataTables.min.css"));

            //Config ScriptBundle and StyleBundle Chart.js
            bundles.Add(new ScriptBundle("~/bundles/admin/chart").Include(
                     "~/lib/chart.js/Chart.js"));

            //Config ScriptBundle and StyleBundle doctor management
            bundles.Add(new ScriptBundle("~/bundles/admin/doctor").Include(
                     "~/Scripts/js/Areas/Admin/doctormanagement.js"));
            bundles.Add(new StyleBundle("~/Content/admin/doctor").Include(
                    "~/Content/css/Areas/Admin/doctor-management.css"));

            //Config ScriptBundle and StyleBundle patient management
            bundles.Add(new ScriptBundle("~/bundles/admin/patient").Include(
                     "~/Scripts/js/Areas/Admin/patientmanagement.js"));
            bundles.Add(new StyleBundle("~/Content/admin/patient").Include(
                    "~/Content/css/Areas/Admin/patient-management.css"));

            //Config ScriptBundle and StyleBundle user management
            bundles.Add(new ScriptBundle("~/bundles/admin/user").Include(
                     "~/Scripts/js/Areas/Admin/usermanagement.js"));
            bundles.Add(new StyleBundle("~/Content/admin/user").Include(
                    "~/Content/css/Areas/Admin/user-management.css"));
            //Config ScriptBundle and StyleBundle admin management
            bundles.Add(new ScriptBundle("~/bundles/admin/adminuser").Include(
                     "~/Scripts/js/Areas/Admin/adminmanagement.js"));

            //Config ScriptBundle and StyleBundle for select2 lib
            bundles.Add(new ScriptBundle("~/bundles/select2/js").Include(
                     "~/lib/select2/js/select2.js"));
            bundles.Add(new StyleBundle("~/Content/select2/css").Include(
                    "~/lib/select2/css/select2.css"));

            //Config StyleBundle for fontawesome lib
            bundles.Add(new StyleBundle("~/Content/fontawesome/css").Include(
                    "~/lib/font-awesome/css/all.min.css"));

            //Config StyleBundle for layout
            bundles.Add(new StyleBundle("~/Content/layout/css").Include(
                    "~/Content/css/Shared/layout.css"));

            //Config ScriptBundle and StyleBundle doctor schedule management for admin
            bundles.Add(new ScriptBundle("~/bundles/admin/doctorschedule").Include(
                     "~/Scripts/js/Areas/Admin/doctorScheduleManagment.admin.js"));
            bundles.Add(new StyleBundle("~/Content/admin/doctorschedule").Include(
                   "~/Content/css/Areas/Admin/doctor-schedule-mgt.css"));

            //Config ScriptBundle and StyleBundle appointment management for admin
            bundles.Add(new ScriptBundle("~/bundles/admin/appointment").Include(
                     "~/Scripts/js/Areas/Admin/appointmentManagement.admin.js"));
            bundles.Add(new StyleBundle("~/Content/admin/appointment").Include(
                 "~/Content/css/Areas/Admin/admin-appointment.css"));
            //Config ScriptBundle and StyleBundle admin profile
            bundles.Add(new ScriptBundle("~/bundles/admin/profile").Include(
                     "~/Scripts/js/Areas/Admin/adminProfile.js"));
            //Config ScriptBundle and StyleBundle admin dashboard
            bundles.Add(new ScriptBundle("~/bundles/admin/dashboard").Include(
                     "~/Scripts/js/Areas/Admin/managementDashboard.js"));
            //Config ScriptBundle and StyleBundle doctor appointment
            bundles.Add(new ScriptBundle("~/bundles/doctor/appointment").Include(
                     "~/Scripts/js/Areas/Doctor/appointmentsDoctor.js"));

        }
    }
}
