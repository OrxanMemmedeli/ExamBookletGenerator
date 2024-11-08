namespace EBC.Core.Jobs.Common;

public abstract class BaseJob
{
    /// <summary>
    /// Job-un icra ediləcək method.
    /// </summary>
    public abstract Task Execute();

    /// <summary>
    /// Job üçün vaxt periodunu müəyyən edən Cron ifadəsi.
    /// </summary>
    public abstract string CronExpression { get; }
}
