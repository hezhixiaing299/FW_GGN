using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FW.Tool
{
    public class ConvertBase
    {
        /// <summary>
        /// 反射实体属性时屏蔽一些字段
        /// </summary>
        public string[] OutFiled = new string[1] { "Id" };

        public static string ConvertPlace(string Place)
        {
            if (Place.Length > 2)
            {
                if (Place.IndexOf("-") == -1)
                {
                    return Place.Substring(0, 2);
                }
                else
                {
                    var ary = Place.Split('-');
                    return ary[0].Substring(0, 2) + ary[1].Substring(0, 2);
                }
            }
            return Place;
        }



        public static string ConvertNowTime(DateTime createTime, DateTime nowTime)
        {
            TimeSpan ts = nowTime - createTime;

            if (ts.TotalSeconds < 60)//秒
            {
                return Math.Round(ts.TotalSeconds) + "秒前";
            }
            else if (ts.TotalMinutes < 60)
            {
                return Math.Round(ts.TotalMinutes) + "分钟前";
            }
            else if (ts.TotalHours < 24)
            {
                return Math.Round(ts.TotalHours) + "小时前";
            }
            else if (ts.TotalDays < 30)
            {
                return Math.Round(ts.TotalDays) + "天前";
            }
            else if (ts.TotalDays < 365)
            {
                var month = (nowTime.Year - createTime.Year) * 12 + (nowTime.Month - createTime.Month);
                return month.ToString() + "个月前";
            }
            else
            {
                var year = (nowTime.Year - createTime.Year);
                return year.ToString() + "个年前";
            }
            //return "";
        }
        /// <summary>
        /// 获取实体的备注说明
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  List<PropertyDescription> GetPropertyDescription(object entity)
        {
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            List<PropertyDescription> Data = new List<PropertyDescription>();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                object[] objs = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                if (objs.Length > 0)
                {
                    PropertyDescription pd = new PropertyDescription();
                    pd.value = property.Name;
                    if (OutFiled.Contains(pd.value))
                    {
                        continue;
                    }
                    pd.text = ((System.ComponentModel.DescriptionAttribute)objs[0]).Description;
                    if (string.IsNullOrEmpty(pd.text))
                    {
                        pd.text = pd.value;
                    }
                    Data.Add(pd);
                }
            }
            return Data;
        }
    }
    public class PropertyDescription
    {
        public string value { get; set; }
        public string text { get; set; } 
    }
}
