using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;
using EBC.Data.Entities.Identity;
using EBC.Data.Enums;

namespace EBC.Data.Entities;

public class Attachment : AuditableEntity<Guid, User>, IAuditable
{
    public Attachment()
    {
        QuestionAttahments = new HashSet<QuestionAttahment>();
    }
    public AttachmentType ProductType { get; set; }
    public string FileName { get; set; }
    public string NewFileName { get; set; }
    public string FileExtention { get; set; }
    public string FilePath { get; set; }
    public string? ContentType { get; set; }
    public byte[] Bytes { get; set; }
    public string Base64 { get; set; }

    public ICollection<QuestionAttahment> QuestionAttahments { get; set; }

}
