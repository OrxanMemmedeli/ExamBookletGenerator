using EBC.Core.Entities.Common;

namespace EBC.Data.Entities;

public class QuestionType : BaseEntity<Guid>, IAuditable
{
    public QuestionType()
    {
        Questions = new HashSet<Question>();
        Responses = new HashSet<Response>();
        QuestionParameters = new HashSet<QuestionParameter>();
    }
    public string ResponseType { get; set; }
    public double? ResponseCount { get; set; } = 0;
    public bool IsShowAnswer { get; set; } = false;
    public string Description { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<Response> Responses { get; set; }
    public ICollection<QuestionParameter> QuestionParameters { get; set; }

}
