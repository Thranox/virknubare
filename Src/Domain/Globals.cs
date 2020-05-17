using System.Collections.Generic;

namespace Domain
{
    public class Globals
    {
        public const string InitialReporteddone = "Initial -> ReportedDone";
        public const string ReporteddoneCertified = "ReportedDone -> Certified";
        public const string CertifiedAssignedForPayment = "Certified -> Assigned For Payment";
        public const string AssignedForPaymentFinal = "Assigned For Payment -> Final";

        public static Dictionary<TravelExpenseStage, string> StageNamesDanish { get; } =
            new Dictionary<TravelExpenseStage, string>
            {
                {TravelExpenseStage.Initial, "Kladde"},
                {TravelExpenseStage.ReportedDone, "F�rdigmeldt"},
                {TravelExpenseStage.Certified, "Attesteret"},
                {TravelExpenseStage.AssignedForPayment, "Anvist til betaling"},
                {TravelExpenseStage.Final, "F�rdigh�ndteret"}
            };

        public static Dictionary<UserStatus, string> UserStatusNamesDanish { get; } =
            new Dictionary<UserStatus, string>
            {
                {UserStatus.Initial, "Tilf�jet, ikke godkendt"},
                {UserStatus.Registered, "Registreret og godkendt"},
                {UserStatus.UserAdministrator, "Administrator"}
            };
    }
}