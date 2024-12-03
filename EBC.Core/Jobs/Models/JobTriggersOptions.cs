namespace EBC.Core.Jobs.Models;

public class JobTriggersOptions
{
    public JobTime CompanyFieldsControlJob { get; set; }
    public JobTime EmailsSenderJob { get; set; }
    public JobTime DebtsAndSummaryJob { get; set; }
    public JobTime DebtEmailSenderJob { get; set; }
    public JobTime ConfirmAndPaymentEmailsSenderJob { get; set; }
}
