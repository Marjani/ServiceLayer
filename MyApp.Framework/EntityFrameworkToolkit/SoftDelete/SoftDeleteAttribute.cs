using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using MyApp.Framework.EntityFrameworkToolkit.Extensions;

namespace MyApp.Framework.EntityFrameworkToolkit.SoftDelete
{
    /// <summary>
    ///     configuration: you should register <see cref="SoftDeleteInterceptor" /> by Calling
    ///     <see cref="DbConfigurationExtensions.AddSoftDeleteInterceptor" /> Method in Ctor of CustomDbConfiguration
    ///     and Call <see cref="DbModelBuilderExtensions.AddSoftDeleteConvention" /> Method For Register SoftDeleteConvention
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SoftDeleteAttribute : Attribute
    {
        public SoftDeleteAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public string ColumnName { get; set; }

        public static string GetSoftDeleteColumnName(EdmType type)
        {
            var annotation = type.MetadataProperties
                .SingleOrDefault(p => p.Name.EndsWith("customannotation:SoftDeleteColumnName"));

            return (string) annotation?.Value;
        }
    }
}