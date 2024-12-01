using EBC.Core.Entities.Common;
using EBC.Data.Enums;

namespace EBC.Data.Entities;

public class SendingEmail : BaseEntity<Guid>
{
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool IsSent { get; set; }
    public EmailSubjectType SubjectType { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Company Company { get; set; }
}
