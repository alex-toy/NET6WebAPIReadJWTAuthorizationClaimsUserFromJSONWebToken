﻿namespace CustomerAPI.Models.Authentication
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken(string username);
    }
}
