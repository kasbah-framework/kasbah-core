using System;

namespace Kasbah.Core.Annotations
{
    public class StaticAttribute : Attribute
    {
        #region Public Constructors

        public StaticAttribute(object staticValue)
        {
            StaticValue = staticValue;
        }

        #endregion

        #region Public Properties

        public object StaticValue { get; set; }

        #endregion
    }
}