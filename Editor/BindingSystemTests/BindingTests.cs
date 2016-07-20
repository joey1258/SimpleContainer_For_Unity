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
    public class BindingTests
    {
        #region To

        /// <summary>
        /// 测试绑定到 TEMP 类型的无 id binding 会否被忽略，不保留在 binder 中
        /// 既然是临时 binding，保存在字典中又没有高效的删除方法，不如即用即弃，让GC自然回收
        /// 如需反复使用，不建议使用 TEMP 类型，或为其加上 id
        /// </summary>
        [Test]
        public void To_SomeValue_BinderDoNotSave()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().ToSelf();
            binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(0, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ToSelf 方法是否使 TEMP 类型的 binding 的值约束转换为 SINGLE 类型
        /// 由于绑定自身的type作为自己的值的缘故，自身的值只有一个，所以值的类型也没有必要保留为 MULTIPLE
        /// </summary>
        [Test]
        public void ToSelf_NullId_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(ConstraintType.SINGLE, binding.constraint);
        }

        /// <summary>
        /// 测试绑定单个实例的值到 TEMP 类型的 binding,binding 是否会自动转换为 SINGLETON 类型
        /// </summary>
        [Test]
        public void BindTempNullIdTo_Instance_BindingChangeTOSingleton()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<int>().To(1).As(1);
            //Assert
            Assert.AreEqual(BindingType.SINGLETON, binder.GetBinding<int>(1).bindingType);
        }

        /// <summary>
        /// 测试绑定单个实例的值到 TEMP 类型的 binding,值约束是否会自动转换为 SINGLE 类型
        /// </summary>
        [Test]
        public void BindTempNullId_ToSelf_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(ConstraintType.SINGLE, binding.constraint);
        }

        #endregion

        #region As

        #endregion

        #region When

        #endregion

        #region Into

        #endregion

        #region ReBind

        #endregion
    }
}