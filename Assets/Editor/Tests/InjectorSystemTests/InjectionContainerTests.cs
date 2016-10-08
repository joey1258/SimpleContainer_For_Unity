/**
 * This file is part of SimpleContainer.
 *
 * Licensed under The MIT License
 * For full copyright and license information, please see the MIT-LICENSE.txt
 * Redistributions of files must retain the above copyright notice.
 *
 * @copyright Joey1258
 * @link https://github.com/joey1258/ToluaContainer
 * @license http://www.opensource.org/licenses/mit-license.php MIT License
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ToluaContainer;
using ToluaContainer.Container;

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
            someClass sc = new someClass();
            //Act
            container
                .RegisterAOT<UnityContainerAOT>()
                .RegisterAOT<EventContainerAOT>()
                .RegisterAOT<CommanderContainerAOT>()
                .RegisterCommand<TestCommand1>()
                .Bind<Transform>().ToPrefab("06_Commander/Prism");
            container.PoolCommands();

            dispatcher = container.GetCommandDispatcher();
            dispatcher.Dispatch<TestCommand1>(sc);
            //Assert
            Assert.AreEqual(
                true,
                dispatcher != null &&
                sc.id == 1 &&
                container.GetTypes<Transform>().Count == 1 &&
                ((PrefabInfo)container.GetTypes<Transform>()[0].value).path == "06_Commander/Prism");
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
            var binding = container.GetAll()[0];
            //Assert
            Assert.AreEqual(
                true,
                container.GetAll().Count == 1 &&
                binding.type == typeof(IInjectionContainer) &&
                binding.value == container);
        }

        #endregion

        #region UnregisterAOT

        #endregion

        #region Dispose

        /// <summary>
        /// 测试 RegisterSelf 方法创建 binding 结果是否正确
        /// </summary>
        [Test]
        public void Dispose_Clearn_Correct()
        {
            //Arrange 
            var container = new InjectionContainer();
            //Act
            container.Dispose();
            //Assert
            Assert.AreEqual(
                true,
                container.binder == null &&
                container.cache == null);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// 测试 RegisterSelf 方法创建 binding 结果是否正确
        /// </summary>
        [Test]
        public void Resolve_SomeClass_Correct()
        {
            //Arrange 
            var container = new InjectionContainer();
            //Act
            var instance = container.Resolve<someClass_c>();
            //Assert
            Assert.AreEqual(
                true,
                instance != null &&
                instance.b != null);
        }

        #endregion

        #region Inject

        /// <summary>
        /// 测试 RegisterSelf 方法创建 binding 结果是否正确
        /// </summary>
        [Test]
        public void Inject_SomeClass_Correct()
        {
            //Arrange 
            var container = new InjectionContainer();
            someClass_c c = new someClass_c();
            //Act
            container.Inject(c);
            //Assert
            Assert.AreEqual(
                true,
                c.b != null);
        }

        #endregion
    }
}
