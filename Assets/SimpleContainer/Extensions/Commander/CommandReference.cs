using System;
using SimpleContainer.Container;
using Utils;

namespace SimpleContainer
{
    [Serializable]
    public class CommandReference
    {
        /// <summary>
        /// command namespace
        /// </summary>
        public string commandNamespace;

        /// <summary>
        /// command name
        /// </summary>
        public string commandName;

        /// <summary>
        /// 根据自身的命名空间与名称发送 command.
        /// </summary>
        public void DispatchCommand(params object[] parameters)
        {
            var type = TypeUtils.GetType(this.commandNamespace, this.commandName);
            CommanderUtils.DispatchCommand(type, parameters);
        }
    }
}