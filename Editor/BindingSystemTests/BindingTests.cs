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
        #region ToSelf

        /// <summary>
        /// 测试 ToSelf 方法是否使 TEMP 类型的无 id binding 的值约束转换为 SINGLE 类型
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
        /// 测试 ToSelf 方法是否使 TEMP 类型的无 id binding 是否被 binder 保存
        /// </summary>
        [Test]
        public void ToSelf_NullId_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(0, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ToSelf 方法是否使 TEMP 类型的有 id binding 的值约束转换为 SINGLE 类型
        /// 由于绑定自身的type作为自己的值的缘故，自身的值只有一个，所以值的类型也没有必要保留为 MULTIPLE
        /// </summary>
        [Test]
        public void ToSelf_HaslId_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf();
            //Assert
            Assert.AreEqual(ConstraintType.SINGLE, binding.constraint);
        }

        /// <summary>
        /// 测试 ToSelf 方法是否使 TEMP 类型的有 id binding 是否被 binder 保存
        /// </summary>
        [Test]
        public void ToSelf_HasId_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().ToSelf().As(1);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].value == typeof(object));
        }

        #endregion

        #region To（type）

        /// <summary>
        /// 测试 To（type） 有 id 的 TEMP 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToType_TempBindingNullId_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().To<object>();
            //Assert
            Assert.AreEqual(0, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 To（type） 有 id 的 TEMP 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToType_TempBindingHasId_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().To<object>().As(1);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList[0] == typeof(object));
        }

        /// <summary>
        /// 测试 To（type） 有 id 的 SINGLETON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToType_SingletonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindSingleton<object>().To<object>();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].value == typeof(object));
        }

        /// <summary>
        /// 测试 To（type） 有 id 的 FACTORY 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToType_FactoryBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindFactory<object>().To<someClass>();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].value == typeof(someClass));
        }

        /// <summary>
        /// 测试 To（type） 有 id 的 MULTITON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToType_MultitonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.BindMultiton<object>().To<object>();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList[0] == typeof(object));
        }

        #endregion

        #region To（instance）

        /// <summary>
        /// 测试 To（instance） TEMP 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToInstance_TempBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList[0] == o);
        }

        /// <summary>
        /// 测试 To（instance） TEMP 类型 binding,bindingType 是否正确转换为 Singleton
        /// </summary>
        [Test]
        public void ToInstance_TempBinding_BindingTypeChangeTOSingleton()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o);
            IBinding binding = binder.GetAllBindings()[0];
            //Assert
            Assert.AreEqual(
                true,
                binding != null &&
                binding.value == o &&
                binding.bindingType == BindingType.SINGLETON);
        }

        /// <summary>
        /// 测试 To（instance） TEMP 类型 binding,值约束是否正确转换为 Single
        /// </summary>
        [Test]
        public void ToInstance_TempBinding_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o);
            IBinding binding = binder.GetAllBindings()[0];
            //Assert
            Assert.AreEqual(
                true,
                binding != null &&
                binding.value == o &&
                binding.constraint == ConstraintType.SINGLE);
        }

        /// <summary>
        /// 测试 To（instance） SINGLTON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToInstance_SingletonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.BindSingleton<object>().To(o);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].value == o);
        }

        /// <summary>
        /// 测试 To（instance） FACTORY 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToInstance_FactoryBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            someClass o = new someClass();
            //Act
            binder.BindFactory<someClass>().To(o);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].value == o);
        }

        /// <summary>
        /// 测试 To（instance） MULTITON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToInstance_MultipleBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.BindSingleton<object>().To(o);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList[0] == o);
        }

        #endregion

        #region To(Instance[])

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToMultipleInstance_TempBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.Bind<object>().To(new object[] { o1, o2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList.Count == 2 &&
                binder.GetAllBindings()[0].valueList[0] == o1 &&
                binder.GetAllBindings()[0].valueList[1] == o2);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,bindingType 是否转换为 MULTITON
        /// </summary>
        [Test]
        public void ToMultipleInstance_TempBinding_BindingTypeChangeTOSingleton()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.Bind<object>().To(new object[] { o1, o2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList.Count == 2 &&
                binder.GetAllBindings()[0].valueList[0] == o1 &&
                binder.GetAllBindings()[0].valueList[1] == o2 &&
                binder.GetAllBindings()[0].bindingType == BindingType.MULTITON);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,值约束是否转换为 MULTIPLE
        /// </summary>
        [Test]
        public void ToMultipleInstance_TempBinding_ConstraintChangeTOSingle()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.Bind<object>().To(new object[] { o1, o2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList.Count == 2 &&
                binder.GetAllBindings()[0].valueList[0] == o1 &&
                binder.GetAllBindings()[0].valueList[1] == o2 &&
                binder.GetAllBindings()[0].constraint == ConstraintType.MULTIPLE);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,相同的值是否被过滤
        /// </summary>
        [Test]
        public void ToMultipleInstance_TempBinding_SameValueFiltered()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.Bind<object>().To(new object[] { o1, o2 }).To(new object[] { o1, o2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binder.GetAllBindings()[0].valueList.Count == 2 &&
                binder.GetAllBindings()[0].valueList[0] == o1 &&
                binder.GetAllBindings()[0].valueList[1] == o2);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,相同的值是否被过滤
        /// </summary>
        [Test]
        public void ToMultipleInstance_TempBinding_ValueNumberAdd()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.Bind<object>().To(new object[] { o1, o2 }).To(new object());
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings()[0].valueList.Count == 3);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,相同的值是否被过滤
        /// </summary>
        [Test]
        public void ToMultipleInstance_MultitonBinding_SameValueFiltered()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.BindMultiton<object>().To(new object[] { o1, o2 }).To(o1);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings()[0].valueList.Count == 2);
        }

        /// <summary>
        /// 测试 To(Instance[]) Temp 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ToMultipleInstance_MultitonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o1 = new object();
            object o2 = new object();
            //Act
            binder.BindMultiton<object>().To(new object[] { o1, o2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1);
        }

        #endregion

        #region As

        /// <summary>
        /// 测试 As 添加 id 是否正确保存
        /// </summary>
        [Test]
        public void As_id_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            binder.Bind<object>().As(1);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetBinding<object>(1) != null);
        }

        #endregion

        #region When

        /// <summary>
        /// 测试 When 返回是否正确保存
        /// </summary>
        [Test]
        public void When_Condition_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            InjectionContext Context = new InjectionContext();
            object o = new object();
            Context.id = o;
            //Act
            binder.Bind<object>().As(1).When((context) => { return context.id == o; });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetBinding<object>(1).condition(Context));
        }

        #endregion

        #region Into

        /// <summary>
        /// 测试 Into 返回是否正确保存
        /// </summary>
        [Test]
        public void Into_Condition_Correct()
        {
            //Arrange 
            IBinder binder = new Binder();
            InjectionContext Context = new InjectionContext();
            Context.parentType = typeof(object);
            //Act
            binder.Bind<object>().As(1).Into(typeof(object));
            //Assert
            Assert.AreEqual(
                true,
                binder.GetBinding<object>(1).condition(Context));
        }

        #endregion

        #region ReBind

        /// <summary>
        /// 测试 ReBind TEMP 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ReBind_TempBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o).Bind<object>().To(new object());
            //Assert
            Assert.AreEqual(2, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ReBind SINGLETON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ReBind_SingletonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o).BindSingleton<object>().To(new object());
            //Assert
            Assert.AreEqual(2, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ReBind FACTORY 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ReBind_FactoryBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            someClass o = new someClass();
            //Act
            binder.BindFactory<someClass>().To(o).BindSingleton<someClass>().To(new someClass ());
            //Assert
            Assert.AreEqual(2, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ReBind MULTITON 类型 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ReBind_MultitonBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o).BindMultiton<object>().To(new object());
            //Assert
            Assert.AreEqual(2, binder.GetAllBindings().Count);
        }

        /// <summary>
        /// 测试 ReBind 多个 binding,binder 是否正确保存
        /// </summary>
        [Test]
        public void ReMultipleBind_MultipleBinding_BinderSaveCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            binder.Bind<object>().To(o).MultipleBind(new Type[] { typeof(int), typeof(float) }, new BindingType[] { BindingType.SINGLETON, BindingType.SINGLETON }).As(new object[] { 1, 2 });
            //Assert
            Assert.AreEqual(3, binder.GetAllBindings().Count);
        }

        #endregion

        #region RemoveValue

        /// <summary>
        /// 测试 RemoveValue SINGLETON 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_SingletonBindingOneValue_ValueNumeberCorrectAndUnbindTrue()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.Bind<int>().To(1);
            //Act
            binding.RemoveValue(1);
            //Assert
            Assert.AreEqual(
                true, 
                binder.GetAllBindings().Count == 0 &&
                binding.valueList.Count == 0);
        }

        /// <summary>
        /// 测试 RemoveValue FACTORY 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_FactoryBindingOneValue_ValueNumeberCorrectAndUnbindTrue()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.BindFactory<someClass>().To(typeof(someClass));
            //Act
            binding.RemoveValue(typeof(someClass));
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 0 &&
                binding.valueList.Count == 0);
        }

        /// <summary>
        /// 测试 RemoveValue 只有一个值的 MULTITON 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_OneValueMultitonBindingOneValue_ValueNumeberCorrectAndUnbindTrue()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.BindMultiton<someClass>().To(typeof(someClass));
            //Act
            binding.RemoveValue(typeof(someClass));
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 0 &&
                binding.valueList.Count == 0);
        }

        /// <summary>
        /// 测试 RemoveValue 只有一个值的 MULTITON 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_MultipleValueMultitonBindingOneValue_ValueNumeberCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.BindMultiton<int>().To(new object[] { 1, 2, 3, 4, 5, 6 });
            //Act
            binding.RemoveValue(2);
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binding.valueList.Count == 5);
        }

        /// <summary>
        /// 测试 RemoveValue 只有一个值的 MULTITON 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_MultipleValueMultitonBindingMultipleValue_ValueNumeberCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.BindMultiton<int>().To(new object[] { 1, 2, 3, 4, 5, 6 });
            //Act
            binding.RemoveValues(new object[] { 1, 2 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binding.valueList.Count == 4);
        }

        /// <summary>
        /// 测试 RemoveValue 只有一个值的 MULTITON 类型 binding,值的数量是否正确，自身是否从 binder 中移除
        /// </summary>
        [Test]
        public void RemoveValue_MultipleValueMultitonBindingSameNumberValue_ValueNumeberCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            IBinding binding = binder.BindMultiton<int>().To(new object[] { 1, 2, 3, 4, 5, 6 });
            //Act
            binding.RemoveValues(new object[] { 1, 2, 3, 4, 5, 6 });
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 0 &&
                binding.valueList.Count == 0);
        }

        #endregion

        #region Chain

        /// <summary>
        /// 测试链式添加值给 TEMP 类型 binding,binder 是否正确保存以及后保存的值是否覆盖之前的值
        /// </summary>
        [Test]
        public void ChainToSelf_TempBindingToType_BinderSaveAndValueOverWhiteCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf().To(typeof(object));
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 0 &&
                binding.value == typeof(object));
        }

        /// <summary>
        /// 测试链式添加值给 TEMP 类型 binding,binder 是否正确保存以及后保存的值是否覆盖之前的值
        /// 由于后保存的是实例，所以当添加实例时 binding 的类型被改变了，所以 binder 进行了保存
        /// </summary>
        [Test]
        public void ChainToSelf_TempBindingToInstance_BinderSaveAndValueOverWhiteCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            IBinding binding = binder.Bind<object>().ToSelf().To(o);
            //Assert
            Assert.AreEqual(
                true, 
                binder.GetAllBindings().Count == 1 &&
                binding.value == o);
        }

        /// <summary>
        /// 测试链式添加值给 TEMP 类型 binding,binder 是否正确保存以及后保存的值是否覆盖之前的值
        /// 由于保存实例时已经改变了 binding 的类型，而后保存的 ToSelf 只会改变约束类型，所以 binder 还是会进行保存（同时还由于第一保存实例时 binder 已经进行过保存了，而 ToSelf 并不会调用 UnBind 方法，所以即使转换为 TEMP 类型也一样不会从 binder 中移除
        /// </summary>
        [Test]
        public void ChainTo_TempBindingToSelf_BinderSaveAndValueOverWhiteCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            IBinding binding = binder.Bind<object>().To(o).ToSelf();
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binding.value == typeof(object));
        }

        /// <summary>
        /// 测试链式添加值给 TEMP 类型 binding,值的数量是否正确
        /// </summary>
        [Test]
        public void ChainMultipleTo_TempBindingTo_ValueNumberCorrect()
        {
            //Arrange 
            IBinder binder = new Binder();
            object o = new object();
            //Act
            IBinding binding = binder.Bind<object>().To(new object[] { o, new object() }).To(new object());
            //Assert
            Assert.AreEqual(
                true,
                binder.GetAllBindings().Count == 1 &&
                binding.valueList.Count == 3);
        }

        #endregion
    }
}