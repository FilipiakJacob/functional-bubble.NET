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
        /// <param name="context">Context in which the receiver is running</param>
        /// <param name="destination">Usually just Resource.Id.resourcename</param>
        /// <param name="bundleArgName">DEFAULT = null. If not null, the pending intent sends a bundle of arguments to the target destination of the deeplink. </param>
        /// <param name="bundleArgValue">DEFAULT = 0. This argument is only used if bundleVarType is not null. </param>
        /// <returns>A pending intent. The intent will contain a bundle if argument name was specified, or no bundle if argument name was not specified.</returns>
        public PendingIntent CreateDeeplink(Context context, int destination, string bundleArgName = null, int bundleArgValue = 0)
        {
            PendingIntent pendingIntent;
            if (bundleArgName != null)
            {
                Bundle bundle = new Bundle();
                bundle.PutInt(bundleArgName, bundleArgValue);
                pendingIntent = new NavDeepLinkBuilder(context)
                    .SetGraph(Resource.Navigation.nav_graph)
                    .SetDestination(destination)
                    .SetArguments(bundle)
                    .CreatePendingIntent();
            }
            else
            {
                pendingIntent = new NavDeepLinkBuilder(context)
                    .SetGraph(Resource.Navigation.nav_graph)
                    .SetDestination(destination)
                    .CreatePendingIntent();
            }

            return pendingIntent;
        }

    }
}