using System;

namespace MyApp.Framework.Domain.Entities
{
    [Flags]
    public enum SettingScopes
    {
        Application = 1 << 0,
        User = 1 << 1
    }

    public interface ISetting
    {
        SettingScopes Scopes { get; set; }
    }
}