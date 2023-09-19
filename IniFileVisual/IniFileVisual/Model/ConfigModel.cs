using System;
using System.Reflection;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using IniParser;
using IniParser.Model;

namespace IniFileVisual.Model
{
    //配置类
    public class ConfigModel : ObservableObject
    {
        //从文件中读取配置
        protected void Configure(string sectionName, bool isRead)
        {
            try
            {
                string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Config.ini";

                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(ConfigPath);
                KeyDataCollection section = data[sectionName];
                PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
              
                if (isRead)
                {
                    foreach (PropertyInfo property in properties)
                    {
                        string key = property.Name;
                     
                        if (section.ContainsKey(key))
                        {
                            string value = section[key];
                            object typeValue = TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value);
                            property.SetValue(this, typeValue);
                        }
                    }
                }
                else
                {
                    foreach (PropertyInfo property in properties)
                    {
                        string key = property.Name;
                        object value = property.GetValue(this);
                        section[key] = Convert.ToString(value);
                    }
                    parser.WriteFile(ConfigPath, data);
                }
            }
            catch
            {
                throw;
            }
        }

        //将配置映射到实例
        public void MapProperties(object target)
        {
            try
            {

                Type sourceType = GetType();
                Type targetType = target.GetType();

                PropertyInfo[] sourceProperties = sourceType.GetProperties();
                PropertyInfo[] targetProperties = targetType.GetProperties();

                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    PropertyInfo targetProperty = Array.Find(targetProperties, p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

                    if (targetProperty != null && targetProperty.CanWrite)
                    {
                        object value = sourceProperty.GetValue(this);
                        targetProperty.SetValue(target, value);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }

}
