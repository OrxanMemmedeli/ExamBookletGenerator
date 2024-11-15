namespace EBC.Data.Entities.CombineEntities;

public class QuestionAttahment
{
    public Guid QuestionId { get; set; }
    public Guid AttachmentId { get; set; }

    public Question Question { get; set; }
    public Attachment Attachment { get; set; }
}
