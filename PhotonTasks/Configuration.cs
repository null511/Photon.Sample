namespace PhotonTasks
{
    internal static class Configuration
    {
        public const string AppPoolName = "DefaultAppPool";

        public static class Apps
        {
            public const string Web = "Photon.Sample.Web";
            public const string Service = "Photon.Sample.Service";
        }

        public static class Roles
        {
            public static class Deploy
            {
                public const string Web = "deploy.web";
                public const string Service = "deploy.svc";
            }
        }
    }
}
