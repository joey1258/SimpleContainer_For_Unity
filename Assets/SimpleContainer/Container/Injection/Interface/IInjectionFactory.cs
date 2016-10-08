using System;

namespace ToluaContainer.Container
{
    public interface IInjectionFactory
    {
        /// <summary>
        /// Creates an instance of the object of the type created by the factory.
        /// </summary>
        object Create(InjectionInfo context);

    }
}