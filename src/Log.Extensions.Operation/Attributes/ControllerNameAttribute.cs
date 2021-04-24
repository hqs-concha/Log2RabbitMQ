using System;

namespace Log.Extensions.Operation.Attributes
{
    /// <summary>
    /// Controller 类名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ControllerNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ControllerNameAttribute(string name)
        {
            Name = name;
        }
    }
}
