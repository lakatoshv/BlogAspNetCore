﻿namespace BLog.Web.ViewModels.AspNetUser
{
    public class TwoFactorAuthenticationViewModel
    {
        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        public bool Is2FaEnabled { get; set; }
    }
}
