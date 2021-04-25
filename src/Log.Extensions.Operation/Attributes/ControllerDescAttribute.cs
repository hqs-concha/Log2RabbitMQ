using System;

namespace Log.Extensions.Operation.Attributes
{
    /// <summary>
    /// Controller 类名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ControllerDescAttribute : Attribute
    {
        public string Name { get; set; }

        public ControllerDescAttribute(string name)
        {
            Name = name;
        }
    }
}
