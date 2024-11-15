using EBC.Core.Entities.Common;
using EBC.Data.Entities.CombineEntities;
using EBC.Data.Enums;

namespace EBC.Data.Entities;

public class Attachment : BaseEntity<Guid>
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

    public ICollection<QuestionAttahment> QuestionAttahments { get; set; }

}
