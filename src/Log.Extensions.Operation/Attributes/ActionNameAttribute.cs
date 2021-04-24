using System;

namespace Log.Extensions.Operation.Attributes
{
    /// <summary>
    /// Action 方法名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ActionNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ActionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
