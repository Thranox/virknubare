using System.Collections.Generic;

namespace Domain
{
    public class Globals
    {
        public const string InitialReporteddone = "Initial -> ReportedDone";
        public const string ReporteddoneCertified = "ReportedDone -> Certified";
        public const string CertifiedAssignedForPayment = "Certified -> Assigned For Payment";
        public const string AssignedForPaymentFinal = "Assigned For Payment -> Final";
        public static Dictionary<TravelExpenseStage,string> StageNamesDanish { get; set; }=new Dictionary<TravelExpenseStage, string>()
        {
            {TravelExpenseStage.Initial,"Kladde" },
            {TravelExpenseStage.ReportedDone, "Færdigmeldt" },
            {TravelExpenseStage.Certified,"Attesteret" },
            {TravelExpenseStage.AssignedForPayment,"Anvist til betaling" },
            {TravelExpenseStage.Final,"Færdighåndteret" }
        };
    }
}