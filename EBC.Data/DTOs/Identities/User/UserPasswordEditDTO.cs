﻿namespace EBC.Data.DTOs.Identities.User;

public class UserPasswordEditDTO
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}