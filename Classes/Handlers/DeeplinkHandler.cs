using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace functional_bubble.NET.Classes.Handlers
{
    public class DeeplinkHandler
    {
        /// <summary>
        /// Use NavDeepLinkBuilder to create a PendingIntent that deeplinks to a a destination.
        /// </summary>
        /// <param name="destination">Usually just Resource.Id.rourcename</param>
        /// <param name="context">Context in which the receiver is running</param>
        /// <param name="bundleVarType">DEFAULT = null. The pending intent sends a bundle of arguments to the target destination of the deeplink. </param>
        /// <param name="bundleVarValue">DEFAULT = 0. This argument is only used if bundleVarType is not null. </param>
        /// <returns>A pending intent</returns>
        public PendingIntent CreateDeeplink(Context context, int destination, string bundleVarType = null, int bundleVarValue = 0)
        {
            Bundle bundle = new Bundle();
            if (bundleVarType != null)
            {
                bundle.PutInt(bundleVarType, bundleVarValue);
            }
            PendingIntent pendingIntent = new NavDeepLinkBuilder(context)
                .SetGraph(Resource.Navigation.nav_graph)
                .SetDestination(destination)
                .SetArguments(bundle)
                .CreatePendingIntent();
            return pendingIntent;
        }

    }
}