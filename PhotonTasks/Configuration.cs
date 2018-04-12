namespace PhotonTasks
{
    internal static class Configuration
    {
        public const string AppPoolName = "Photon.Sample.Pool";

        public static class Apps
        {
            public static class Web
            {
                public const string PackageId = "photon.sample.web";
                public const string AppName = "Photon.Sample.Web";
            }

            public static class Service
            {
                public const string PackageId = "photon.sample.svc";
                public const string AppName = "Photon.Sample.Service";
            }
        }

        public static class Roles
        {
            public const string Build = "build";

            public static class Deploy
            {
                public const string Web = "deploy.web";
                public const string Service = "deploy.svc";
            }
        }
    }
}
