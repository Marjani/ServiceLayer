namespace MyApp.Framework.Data.Hooks
{
    public interface IPreActionHook : IHook
    {
        bool RequiresValidation { get; }
    }
}