using System;

namespace MyApp.Framework.Data
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}