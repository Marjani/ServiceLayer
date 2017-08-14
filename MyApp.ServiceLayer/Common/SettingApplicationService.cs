using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyApp.DomainClasses.Common;
using MyApp.Framework.Application.Services;
using MyApp.Framework.Domain.Entities;

namespace MyApp.ServiceLayer.Common
{
    public class SettingApplicationService : ApplicationService, ISettingApplicationService
    {
        public Setting GetSettingById(long settingId)
        {
            throw new NotImplementedException();
        }

        public T GetSettingByKey<T>(string key, T defaultValue = default(T), int storeId = 0, bool loadSharedValueIfNotFound = false)
        {
            throw new NotImplementedException();
        }

        public IList<Setting> GetAllSettings()
        {
            throw new NotImplementedException();
        }

        public bool SettingExists<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public T LoadSetting<T>(int storeId = 0) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void SetSetting<T>(string key, T value, int storeId = 0, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T>(T settings, int storeId = 0) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, int storeId = 0, bool clearCache = true) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void UpdateSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool overrideForStore, int storeId = 0) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void InsertSetting(Setting setting, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void UpdateSetting(Setting setting, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(Setting setting)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T>() where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISetting, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(string key, int storeId = 0)
        {
            throw new NotImplementedException();
        }

        public int DeleteSettings(string rootKey)
        {
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
