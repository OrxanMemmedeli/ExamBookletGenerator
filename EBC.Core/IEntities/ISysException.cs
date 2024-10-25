﻿using EBC.Core.IEntities.Common;

namespace EBC.Core.IEntities;

public interface ISysException : IBaseEntity
{
    public string UserIP { get; set; }
    public string UserName { get; set; }
    public string RequestPath { get; set; }
    public int StatusCode { get; set; }
    public string StackTrace { get; set; }
    public string Message { get; set; }
    public string Exception { get; set; }
}
