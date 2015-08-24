using System.Web;
using System.Web.Optimization;

namespace TournamentMaker
{
    public class BundleConfig
    {
        // Pour plus d’informations sur le regroupement, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-animate.js")
                .Include("~/Scripts/i18n/angular-locale_fr-fr.js")
                .Include("~/Scripts/angular-route.js")
                .Include("~/Scripts/angular-resource.js")
                .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
            );


            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/app/app.js")
                // TODO : décommenter une fois les dossier alimentés en fichiers
                .IncludeDirectory("~/app/controllers", "*.js")
                .IncludeDirectory("~/app/directives", "*.js")
                .IncludeDirectory("~/app/services", "*.js")
                .IncludeDirectory("~/app/models", "*.js")
                .IncludeDirectory("~/app/services/interceptors", "*.js")
            );

            bundles.Add(new StyleBundle("~/content/css/styles").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/style.css"));
        }
    }
}
