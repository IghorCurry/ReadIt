﻿namespace ReadIt.DAL.Persistance.Settings
{
    public record AccessTokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
    }
}

