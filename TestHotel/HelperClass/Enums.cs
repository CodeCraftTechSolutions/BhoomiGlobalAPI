using Nancy.Json;
using System.Reflection;

namespace BhoomiGlobalAPI.HelperClass
{
    public class Enums
    {
        public enum TargetModule /*TargetModule is for Carousel of Mobile*/
        {
            Store = 1,
            Product = 2,
            Page = 3,
            ProductCategory = 4,
            Brand = 5,
            OrderDetail = 6,
            Faq = 7,
        }


        public enum MenuItemStatus
        {
            Active = 1,
            Inactive = 0
        }

        public enum MenuCategoryStatus
        {
            Active = 1,
            Inactive = 0
        }


        public enum Role
        {
            [System.ComponentModel.Description("SUPER ADMINISTRATOR")]
            SUPERADMINISTRATOR,
            [System.ComponentModel.Description("ADMINISTRATOR")]
            ADMINISTRATOR,
            [System.ComponentModel.Description("GENERAL")]
            GENERAL
        }




        public object ExportEnum<T>()
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();
            var dict = values.ToDictionary(e => e.ToString(), e => Convert.ToInt32(e));
            var json = new JavaScriptSerializer().Serialize(dict);
            return json;
        }
        public object ExportEnumDescriptive<T>()
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();
            var dict = values.ToDictionary(e => SpacePascalCase(e.ToString()), e => Convert.ToInt32(e));
            var json = new JavaScriptSerializer().Serialize(dict);
            return json;
        }

        public object ExportEnumDescription<T>()
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();
            var dict = values.ToDictionary(e => EnumHelper.GetDescription(e), e => Convert.ToInt32(e));
            var json = new JavaScriptSerializer().Serialize(dict);
            return json;
        }
        private static string SpacePascalCase(string input)
        {
            return input.Aggregate(string.Empty, (old, x) => $"{old}{(char.IsUpper(x) ? " " : "")}{x}").TrimStart(' ');
        }

    }


    public class EnumHelper
    {
        public static string GetDescription<T>(T @enum)
        {
            if (@enum == null)
                return null;
            string description = @enum.ToString();

            try
            {
                FieldInfo fi = @enum.GetType().GetField(@enum.ToString());
                Type typeOfObj = typeof(System.ComponentModel.DescriptionAttribute);
                System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeOfObj, false);
                if (attributes.Length > 0)
                    description = attributes[0].Description;
            }
            catch (Exception ex)
            {
            }
            return description;
        }
    }
}
