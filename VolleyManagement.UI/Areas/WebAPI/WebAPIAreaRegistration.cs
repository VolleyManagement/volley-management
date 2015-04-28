namespace VolleyManagement.UI.Areas.WebApi
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;

    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Teams;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    using Microsoft.OData.Edm;
    using Microsoft.OData.Edm.Library;

    /// <summary>
    /// The WebApi area registration.
    /// </summary>
    public class WebApiAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the area name.
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "WebApi";
            }
        }

        /// <summary>
        /// Registers OData routing
        /// </summary>
        /// <param name="config">The config.</param>
        public static void RegisterOData(HttpConfiguration config)
        {
            config.MapODataServiceRoute("odata", "odata", GetEdmModel());
        }

        /// <summary>
        /// The register area.
        /// </summary>
        /// <param name="context"> The context. </param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WebApi_default",
                "WebApi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.WebApi.Controllers" });
        }

        /// <summary>
        /// Gets EdmModel using ModelBuilder.
        /// </summary>
        /// <returns>EdmModel with entity sets and relationships.</returns>
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EnableLowerCamelCase();

            builder.EntitySet<TournamentViewModel>("Tournaments");
            builder.EntitySet<TeamViewModel>("Teams");
            builder.EntitySet<PlayerViewModel>("Players");
            var edmModel = builder.GetEdmModel();
            AddNavigations(edmModel);
            return edmModel;
        }

        /// <summary>
        /// Adds navigation to passed EdmModel.
        /// </summary>
        /// <param name="edmModel">EdmModel to add the navigation to.</param>
        private static void AddNavigations(IEdmModel edmModel)
        {
            var teams = (EdmEntitySet)edmModel.EntityContainer.FindEntitySet("Teams");
            var players = (EdmEntitySet)edmModel.EntityContainer.FindEntitySet("Players");
            var teamType = (EdmEntityType)edmModel.FindDeclaredType("VolleyManagement.UI.Areas.WebApi.ViewModels.Teams.TeamViewModel");
            var playerType = (EdmEntityType)edmModel.FindDeclaredType("VolleyManagement.UI.Areas.WebApi.ViewModels.Players.PlayerViewModel");
            AddOneToManyNavigation("Players", teams, players, teamType, playerType);
            AddManyToOneNavigation("Teams", teams, players, teamType, playerType);
        }

        /// <summary>
        /// Adds navigation from oneEntitySet to manyEntitySet.
        /// </summary>
        /// <param name="navTargetName">Name of navigation.</param>
        /// <param name="oneEntitySet">One EdmEntitySet.</param>
        /// <param name="manyEntitySet">Many EdmEntitySet.</param>
        /// <param name="oneEntityType">Type of one EdmEntitySet.</param>
        /// <param name="manyEntityType">Type of many EdmEntitySet.</param>
        private static void AddOneToManyNavigation(string navTargetName, EdmEntitySet oneEntitySet, EdmEntitySet manyEntitySet,
            EdmEntityType oneEntityType, EdmEntityType manyEntityType)
        {
            var navPropertyInfo = new EdmNavigationPropertyInfo
            {
                TargetMultiplicity = EdmMultiplicity.Many,
                Target = manyEntityType,
                ContainsTarget = false,
                OnDelete = EdmOnDeleteAction.None,
                Name = navTargetName
            };
            oneEntitySet.AddNavigationTarget(oneEntityType.AddUnidirectionalNavigation(navPropertyInfo), manyEntitySet);
        }

        /// <summary>
        ///  Adds navigation from manyEntitySet to oneEntitySet.
        /// </summary>
        /// <param name="navTargetName">Name of navigation.</param>
        /// <param name="oneEntitySet">One EdmEntitySet.</param>
        /// <param name="manyEntitySet">Many EdmEntitySet.</param>
        /// <param name="oneEntityType">Type of one EdmEntitySet.</param>
        /// <param name="manyEntityType">Type of many EdmEntitySet.</param>
        private static void AddManyToOneNavigation(string navTargetName, EdmEntitySet oneEntitySet, EdmEntitySet manyEntitySet,
            EdmEntityType oneEntityType, EdmEntityType manyEntityType)
        {
            var navPropertyInfo = new EdmNavigationPropertyInfo
            {
                TargetMultiplicity = EdmMultiplicity.One,
                Target = oneEntityType,
                ContainsTarget = false,
                OnDelete = EdmOnDeleteAction.None,
                Name = navTargetName
            };
            manyEntitySet.AddNavigationTarget(manyEntityType.AddUnidirectionalNavigation(navPropertyInfo), oneEntitySet);
        }
    }
}