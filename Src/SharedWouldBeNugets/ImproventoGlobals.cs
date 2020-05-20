namespace SharedWouldBeNugets
{
    public static class ImproventoGlobals
    {
        public static int LocalKataPort = 45656;
        public static string LocalKataRedirect = "https://127.0.0.1:"+LocalKataPort;

        public static string IssUri = "https://improvento.dk/";

        public static string[] AllowedCorsOrigins =
            {"https://localhost:44324", "http://localhost:50627", "https://localhost:4200", "http://localhost:4200"};

        public static string ImproventoSubClaimName = "ImproventoSub";

        public static string AngularClientId = "polangularclient";
    }
}

