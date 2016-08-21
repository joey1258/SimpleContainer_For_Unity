/*
 * Copyright 2016 Sun Ning（Joey1258）
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using uMVVMCS;
using uMVVMCS.DIContainer;

namespace uMVVMCS_NUitTests
{
    [TestFixture]
    public class InjectionContainerTests
    {
        #region RegisterAOT

        /// <summary>
        /// 测试 RegisterSelf 方法创建 binding 结果是否正确
        /// </summary>
        [Test]
        public void RegisterAOT_T_Correct()
        {
            //Arrange 
            var container = new InjectionContainer();
            ICommandDispatcher dispatcher;
            int num = 0;
            //Act
            container
                .RegisterAOT<UnityContainer>()
                .RegisterAOT<EventContainer>()
                .RegisterAOT<CommanderContainer>()
                .RegisterCommand<TestCommand1>();

            dispatcher = container.GetCommandDispatcher();
            dispatcher.Dispatch<TestCommand1>(num);
            //Assert
            Assert.AreEqual(
                true,
                dispatcher != null &&
                container.GetAllBindings().Count == 1 &&
                binding.type == typeof(IInjectionContainer) &&
                binding.value == container);
        }

        /// <summary>
        /// 测试 RegisterSelf 方法创建 binding 结果是否正确
        /// </summary>
        [Test]
        public void RegisterSelf_CreatBinding_Correct()
        {
            //Arrange 
            var container = new InjectionContainer();
            //Act
            //由于 RegisterSelf 方法于构造函数内部使用，所有不用也无法以 container.RegisterSelf() 形式调用;
            var binding = container.GetAllBindings()[0];
            //Assert
            Assert.AreEqual(
                true,
                container.GetAllBindings().Count == 1 &&
                binding.type == typeof(IInjectionContainer) &&
                binding.value == container);
        }

        #endregion

        #region UnregisterAOT

        #endregion

        #region Dispose

        #endregion

        #region Resolve

        #endregion

        #region Inject

        #endregion
    }
}
