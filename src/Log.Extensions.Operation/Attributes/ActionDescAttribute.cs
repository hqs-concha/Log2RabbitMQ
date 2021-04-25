using System;

namespace Log.Extensions.Operation.Attributes
{
    /// <summary>
    /// Action 方法名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ActionDescAttribute : Attribute
    {
        public string Name { get; set; }

        public ActionDescAttribute(string name)
        {
            Name = name;
        }
    }
}
