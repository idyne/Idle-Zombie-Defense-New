using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Globalization;
using Google;

namespace Moonee.MoonSDK.Internal.Editor
{
    public class MoonSDKPrebuildiOS : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        private const float MinIosVersion = 13.0f;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.iOS) {
                return;
            }
            SetResolverSettings();
            PreparePlayerSettings();
        }

        private static void SetResolverSettings()
        {
            IOSResolver.PodfileGenerationEnabled = true;
            IOSResolver.PodToolExecutionViaShellEnabled = true;
            IOSResolver.UseProjectSettings = true;
            IOSResolver.CocoapodsIntegrationMethodPref = IOSResolver.CocoapodsIntegrationMethod.Project;
        }

        private static void PreparePlayerSettings()
        {
            PlayerSettings.iOS.allowHTTPDownload = true;
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);

            var changeMinVersion = true;
            if (float.TryParse(PlayerSettings.iOS.targetOSVersionString, out float iosMinVersion)) {
                if (iosMinVersion >= MinIosVersion) {
                    changeMinVersion = false;
                }
            }
            if (changeMinVersion) {
                PlayerSettings.iOS.targetOSVersionString = MinIosVersion.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}