using System;

namespace Kasbah.Core.Annotations
{
    public class DefaultAttribute : Attribute
    {
        #region Public Constructors

        public DefaultAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        #endregion

        #region Public Properties

        public object DefaultValue { get; set; }

        #endregion
    }
}