using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTravelApp.Utils
{
    public static class PermissionChecker
    {
        public static async Task<Boolean> checkForAllRequiredPermissions()
        {
            await checkForPermission(Permission.Phone);
            await checkForPermission(Permission.Location);
            await checkForPermission(Permission.Microphone);
            await checkForPermission(Permission.Storage);
            await checkForPermission(Permission.Sms);
            return false;
        }


        public static async Task<Boolean> checkForImmediatePermissions()
        {
            await checkForPermission(Permission.Location);
            await checkForPermission(Permission.Microphone);
            await checkForPermission(Permission.Storage);
            return false;
        }

        public static async Task<Boolean> checkForPermission(Permission pPermission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(pPermission);
                if (status != PermissionStatus.Granted)
                {

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(pPermission);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(pPermission)) status = results[pPermission];

                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
