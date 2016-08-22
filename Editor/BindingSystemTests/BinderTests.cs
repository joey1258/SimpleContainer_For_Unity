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
    public class BinderTests
    {
        #region Bind

        /// <summary>
        /// 测试 Bind 方法是否生成了 ADDRESS 类型的 binding,值约束为 Multiple
        /// </summary>
        [Test]
        public void Bind_CreateTempBinding_True()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>();
            //Assert
            Assert.AreEqual(
                true, 
                binding != null && 
                binding.bindingType == BindingType.ADDRESS &&
                binding.constraint == ConstraintType.MULTIPLE);
        }

        /// <summary>
        /// 测试 BindSingleton 方法是否生成了 SINGLETON 类型的 binding,值约束为 Single
        /// </summary>
        [Test]
        public void BindSingleton_CreateSingletonBinding_True()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.BindSingleton<object>();
            //Assert
            Assert.AreEqual(
                true,
                binding != null &&
                binding.bindingType == BindingType.SINGLETON &&
                binding.constraint == ConstraintType.SINGLE);
        }

        /// <summary>
        /// 测试 BindFactory 方法是否生成了 FACTORY 类型的 binding,值约束为 Single
        /// </summary>
        [Test]
        public void BindFactory_CreateFactoryBinding_True()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.BindFactory<object>();
            //Assert
            Assert.AreEqual(
                true,
                binding != null &&
                binding.bindingType == BindingType.FACTORY &&
                binding.constraint == ConstraintType.SINGLE);
        }

        /// <summary>
        /// 测试 BindMultiton 方法是否生成了 MULTITON 类型的 binding,值约束为 Multiple
        /// </summary>
        [Test]
        public void BindMultiton_CreateMultitonBinding_True()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.BindMultiton<object>();
            //Assert
            Assert.AreEqual(
                true,
                binding != null &&
                binding.bindingType == BindingType.MULTITON &&
                binding.constraint == ConstraintType.MULTIPLE);
        }

        /// <summary>
        /// 测试 MultipleBind 方法是否生成了指定个指定类型的 binding，其值约束是否各种正确
        /// </summary>
        [Test]
        public void MultipleBind_CreateMultipleBinding_True()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.MultipleBind(
                new Type[] { typeof(int) , typeof(float), typeof(someClass) },
                new BindingType[] { BindingType.MULTITON, BindingType.SINGLETON, BindingType.FACTORY })
                .As(new object[] { 1, 2, 3 });
            IList<IBinding> bindings = binder.GetAll();
            //Assert
            Assert.AreEqual(
                true,
                bindings.Count == 3 &&
                bindings[0].bindingType == BindingType.MULTITON &&
                bindings[0].constraint == ConstraintType.MULTIPLE &&
                bindings[1].bindingType == BindingType.SINGLETON &&
                bindings[1].constraint == ConstraintType.SINGLE &&
                bindings[2].bindingType == BindingType.FACTORY &&
                bindings[2].constraint == ConstraintType.SINGLE);
        }

        #endregion

        #region GetBinding

        /// <summary>
        /// 测试 GetTypes 所获取的各种类型的 binding 数量是否正确
        /// </summary>
        [Test]
        public void GetBindingsByType_TypeBindingsNumber_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .MultipleBind(
                new Type[] { typeof(int), typeof(int), typeof(int) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 1, 2, 3 })
                .MultipleBind(
                new Type[] { typeof(float), typeof(float) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 4, 5 })
                .MultipleBind(
                new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 6, 7, 8, 9 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetTypes<int>().Count == 3 &&
                binder.GetTypes<float>().Count == 2 &&
                binder.GetTypes<string>().Count == 4);
        }

        /// <summary>
        /// 测试 GetIds 所获取的各种 id 的 binding 数量是否正确
        /// </summary>
        [Test]
        public void GetBindingsById_IdBindingsNumber_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .MultipleBind(
                new Type[] { typeof(int), typeof(int), typeof(int) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 1, 2, 3 })
                .MultipleBind(
                new Type[] { typeof(float), typeof(float) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 1, 5 })
                .MultipleBind(
                new Type[] { typeof(string), typeof(string), typeof(string) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 1, 5, 3 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetIds(1).Count == 3 &&
                binder.GetIds(2).Count == 1 &&
                binder.GetIds(3).Count == 2 &&
                binder.GetIds(5).Count == 2);
        }

        /// <summary>
        /// 测试 GetAll 所获取的 binding 数量是否正确
        /// </summary>
        [Test]
        public void GetAllBindings_BindingsNumber_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .MultipleBind(
                new Type[] { typeof(int), typeof(int), typeof(int) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 1, 2, 3 })
                .MultipleBind(
                new Type[] { typeof(float), typeof(float) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 4, 5 })
                .MultipleBind(
                new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) },
                new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON, BindingType.SINGLETON })
                .As(new object[] { 6, 7, 8, 9 });
            //Assert
            Assert.AreEqual( 9, binder.GetAll().Count);
        }

        /// <summary>
        /// 测试 GetSameNullId 获取的 binding 是否正确
        /// </summary>
        [Test]
        public void GetSameNullIdBinding_ReturnBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding1 = binder.Bind<object>().To(1);
            IBinding binding2 = binder.BindMultiton<object>().To(new object[] { 7, 8, 9 });
            //Act
            binder
                .Bind<object>()
                .To(1)
                .Bind<object>()
                .To(2)
                .BindMultiton<object>()
                .To(new object[] { 7, 8, 9 })
                .Bind<int>()
                .To(1)
                .BindMultiton<int>()
                .To(new object[] { 7, 8, 9 });
            //Assert
            Assert.AreEqual(
                true,
                /*确认没有取到多余的值，且取到的不是自身*/
                binder.GetSameNullId(binding1).Count == 1 &&
                binder.GetSameNullId(binding1)[0].GetHashCode() !=
                binding1.GetHashCode() &&
                binder.GetSameNullId(binding2).Count == 1 &&
                binder.GetSameNullId(binding2)[0].GetHashCode() !=
                binding2.GetHashCode());
        }

        /// <summary>
        /// 测试 GetBinding 是否获取到了正确的 binding
        /// </summary>
        [Test]
        public void GetBinding_ReturnBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .Bind<object>()
                .To(1)
                .As(1)
                .Bind<int>()
                .To(1)
                .As(1)
                .Bind<string>()
                .To("1")
                .As(1)
                .Bind<float>()
                .To(1.111111f)
                .As(1);
            //Assert
            Assert.AreEqual(1.111111f, binder.GetBinding<float>(1).value);
        }

        #endregion

        #region Unbind

        /// <summary>
        /// 测试 UnbindType 方法是否正确的移除了相应的 binding
        /// </summary>
        [Test]
        public void UnbindByType_UnbindBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder
                .Bind<object>()
                .To(1)
                .Bind<object>()
                .To(2)
                .Bind<int>()
                .To(1)
                .Bind<int>()
                .To(2)
                .Bind<float>()
                .To(1f)
                .Bind<float>()
                .To(2f);
            //Act
            binder.UnbindType<int>();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAll().Count == 4 &&
                binder.GetTypes<int>().Count == 0);
        }

        /// <summary>
        /// 测试 UnbindId 方法是否正确的移除了相应的 binding
        /// </summary>
        [Test]
        public void UnbindById_UnbindBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder
                .Bind<object>()
                .To(1)
                .As(1)
                .Bind<object>()
                .To(2)
                .As(2)
                .Bind<int>()
                .To(1)
                .As(1)
                .Bind<int>()
                .To(2)
                .As(2)
                .Bind<float>()
                .To(1f)
                .As(1)
                .Bind<float>()
                .To(2f)
                .As(2);
            //Act
            binder.UnbindId(1);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAll().Count == 3);
        }

        /// <summary>
        /// 测试 UnbindNullIdType 方法是否正确的移除了相应的 binding
        /// </summary>
        [Test]
        public void UnbindNullIdBindingByType_UnbindBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder
                .Bind<object>()
                .To(1)
                .Bind<object>()
                .To(2)
                .As(2)
                .Bind<int>()
                .To(1)
                .Bind<int>()
                .To(2)
                .Bind<float>()
                .To(1f)
                .As(1)
                .Bind<float>()
                .To(2f);
            //Act
            binder.UnbindNullIdType<object>();
            binder.UnbindNullIdType<int>();
            binder.UnbindNullIdType<float>();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAll().Count == 2 &&
                binder.GetTypes<object>().Count == 1 &&
                binder.GetTypes<int>().Count == 0 &&
                binder.GetTypes<float>().Count == 1);
        }

        /// <summary>
        /// 测试 Unbind 方法是否正确的移除了相应的 binding
        /// </summary>
        [Test]
        public void Unbind_UnbindBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder
                .Bind<object>()
                .To(1)
                .Bind<object>()
                .To(2)
                .As(2)
                .Bind<int>()
                .To(1)
                .Bind<int>()
                .To(2)
                .Bind<float>()
                .To(1f)
                .As(1)
                .Bind<float>()
                .To(2f);
            //Act
            binder.Unbind<object>(1);
            //Assert
            Assert.AreEqual(true , binder.GetBinding<object>(1) == null);
        }

        /// <summary>
        /// 测试 Unbind(binding) 方法是否正确的移除了相应的 binding
        /// </summary>
        [Test]
        public void UnbindBinding_UnbindBinding_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            binder
                .Bind<object>()
                .To(1)
                .Bind<object>()
                .To(2)
                .As(2)
                .Bind<int>()
                .To(1)
                .Bind<int>()
                .To(2)
                .Bind<float>()
                .To(1f)
                .As(1)
                .Bind<float>()
                .To(2f);
            IBinding binding = binder.GetBinding<object>(1);
            //Act
            binder.Unbind(binding);
            //Assert
            Assert.AreEqual(true, binder.GetBinding<object>(1) == null);
        }

        #endregion

        #region Chain

        /// <summary>
        /// 测试链式绑定（单数开始无 id）是否正确的绑定了相应的 binding
        /// 初版不保存 TEMP 类型 binding；2016.8.1 改 TEMP 类型为 ADDRESS，并将其保存在 binder 中
        /// </summary>
        [Test]
        public void ChainBind_SingleBeginNullId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .BindMultiton<string>()
                .To(new object[] { "a", "b", "c" })
                .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() });
            //Assert
            Assert.AreEqual(7, binder.GetAll().Count);
        }

        /// <summary>
        /// 测试链式绑定（单数开始无 id）是否正确的绑定了相应的 binding
        /// 初版不保存 TEMP 类型 binding；2016.8.1 改 TEMP 类型为 ADDRESS，并将其保存在 binder 中
        /// </summary>
        [Test]
        public void ChainBind_SingleBeginHasId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder
                .Bind<string>()
                .To(new object[] { "a", "b", "c" })
                .As(0)
                .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .As(new object[] { null, 1, 2 })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .As(new object[] { null, 3, 4 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAll().Count == 10 &&
                binder.GetBinding<int>(1) != null &&
                binder.GetBinding<string>(0) != null &&
                binder.GetBinding<someClass>(2) != null &&
                binder.GetBinding<int>(3) != null &&
                binder.GetBinding<someClass>(4) != null);
        }

        /// <summary>
        /// 测试链式绑定（复数开始无 id）是否正确的绑定了相应的 binding
        /// 初版不保存 TEMP 类型 binding；2016.8.1 改 TEMP 类型为 ADDRESS，并将其保存在 binder 中
        /// </summary>
        [Test]
        public void ChainBind_MultipleBeginNullId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() });
            //Assert
            Assert.AreEqual(6, binder.GetAll().Count);
        }

        /// <summary>
        /// 测试链式绑定（复数开始有 id）是否正确的绑定了相应的 binding
        /// 初版不保存 TEMP 类型 binding；2016.8.1 改 TEMP 类型为 ADDRESS，并将其保存在 binder 中
        /// </summary>
        [Test]
        public void ChainBind_MultipleBeginHasId_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .As(new object[] { null, 1, 2 })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .MultipleBind(
                new Type[] { typeof(someClass_b), typeof(int), typeof(someClass) },
                new BindingType[] {
                    BindingType.ADDRESS,
                    BindingType.SINGLETON,
                    BindingType.FACTORY })
                    .To(new object[] { typeof(someClass_b), 1, new someClass() })
                    .As(new object[] { null, 3, 4 });
            //Assert
            Assert.AreEqual(
                true, 
                binder.GetAll().Count == 9 &&
                binder.GetBinding<int>(1) != null &&
                binder.GetBinding<someClass>(2) != null &&
                binder.GetBinding<int>(3) != null &&
                binder.GetBinding<someClass>(4) != null);
        }

        #endregion
    }
}
