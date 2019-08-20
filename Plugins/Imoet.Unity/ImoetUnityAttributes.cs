using UnityEngine;
namespace Imoet.Unity
{
    public class BoxInfoAttribute : PropertyAttribute
    {
        public string message;
        public BoxInfoAttribute(string message)
        {
            this.message = message;
        }
    }
    public class BoxWarningAttribute : PropertyAttribute
    {
        public string message;
        public BoxWarningAttribute(string message)
        {
            this.message = message;
        }
    }
    public class BoxErrorAttribute : PropertyAttribute
    {
        public string message;
        public BoxErrorAttribute(string message)
        {
            this.message = message;
        }
    }

    public class NotEditableAttribute : PropertyAttribute { }
    public class ReadOnlyAttribute : PropertyAttribute { }
    public class DisabledOnPlayModeAttribute : PropertyAttribute { }
    public class FullBarToggleAttribute : PropertyAttribute { }
    public class WideToggleAttribute : PropertyAttribute
    {
        public string enableText, disableText;
        public WideToggleAttribute() : this("Enable", "Disable") { }
        public WideToggleAttribute(string enableText, string disableText)
        {
            this.enableText = enableText;
            this.disableText = disableText;
        }
    }

    public class ExeButtonAttribute : PropertyAttribute
    {
        public string methodName;
        public ExeButtonAttribute(string methodName)
        {
            this.methodName = methodName;
        }
    }
    public class ShowIfNotNullAttribute : PropertyAttribute
    {
        public string inspectedPropertyName;
        public bool reverse;
        public ShowIfNotNullAttribute(string inspectedPropertyName) : this(inspectedPropertyName, false) { }
        public ShowIfNotNullAttribute(string inspectedPropertyName, bool reverse)
        {
            this.inspectedPropertyName = inspectedPropertyName;
            this.reverse = reverse;
        }
    }
    public class ShowIfMoreThanAttribute : PropertyAttribute
    {
        public string inspectedPropertyName;
        public float num;
        public ShowIfMoreThanAttribute(string inspectedPropertyName, float num)
        {
            this.inspectedPropertyName = inspectedPropertyName;
            this.num = num;
        }
    }
    public class ShowIfLessThanAttribute : PropertyAttribute
    {
        public string inspectedPropertyName;
        public float num;
        public ShowIfLessThanAttribute(string inspectedPropertyName, float num)
        {
            this.inspectedPropertyName = inspectedPropertyName;
            this.num = num;
        }
    }
    public class ShowIfInBeetwenAttribute : PropertyAttribute
    {
        public string inspectedPropertyName;
        public float startNum;
        public float endNum;
        public bool reverse;
        public ShowIfInBeetwenAttribute(string inspectedPropertyName, float startNum, float endNum) : this(inspectedPropertyName, startNum, endNum, false) { }
        public ShowIfInBeetwenAttribute(string inspectedPropertyName, float startNum, float endNum, bool reverse)
        {
            this.inspectedPropertyName = inspectedPropertyName;
            this.startNum = startNum;
            this.endNum = endNum;
            this.reverse = reverse;
        }
    }
    public class ShowIfEqualAttribute : PropertyAttribute
    {
        public string inspectedPropertyName;
        public object obj;
        public bool reverse;
        public ShowIfEqualAttribute(string inspectedPropertyName, object obj) : this(inspectedPropertyName, obj, false) { }
        public ShowIfEqualAttribute(string inspectedPropertyName, object obj, bool reverse)
        {
            this.inspectedPropertyName = inspectedPropertyName;
            this.obj = obj;
            this.reverse = reverse;
        }
    }
}

