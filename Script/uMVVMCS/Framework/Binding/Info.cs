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

/*
 * 由于隐藏IList和ICollection暴露的Add、Remove等方法当前没有较好的方法，用数组将造成大量GC，干脆将其设置为可读可写。
 */

using System;

namespace uMVVMCS.DIContainer
{
    public class Info : IInfo
    {
        #region constructor

        public Info(Type t, BindingType ct)
        {
            _type = t;
            _bindingType = ct;
            _valueConstraint = ConstraintType.SINGLE;

            hasBeenInjected = false;
        }

        public Info(Type t, BindingType ct, ConstraintType c)
        {
            _type = t;
            _bindingType = ct;
            _valueConstraint = c;

            hasBeenInjected = false;
        }

        #endregion

        #region IInfo implementation 

        /// <summary>
        /// type 属性
        /// </summary>
        public Type type
        {
            get { return _type; }
        }
        protected Type _type;

        /// <summary>
        ///  value 属性
        /// </summary>
        public object value
        {
            get { return _value; }
            set { _value = value; }
        }
        protected object _value;

        /// <summary>
        /// id 属性
        /// </summary>
        public object id
        {
            get { return _id; }
            set { _id = value; }
        }
        protected object _id;

        /// <summary>
        /// 注入类型
        /// </summary>
        public BindingType bindingType
        {
            get { return _bindingType; }
        }
        protected BindingType _bindingType;

        /// <summary>
        /// value 约束 (SINGLE \ PLURAL \ POOL)
        /// </summary>
        public ConstraintType valueConstraint
        {
            get { return _valueConstraint; }
        }
        protected ConstraintType _valueConstraint;

        /// <summary>
        /// 是否已经被注入
        /// </summary>
        public bool hasBeenInjected { get; set; }

        /// <summary>
        /// 条件判断委托
        /// </summary>
        public Condition condition { get; set; }

        #endregion
    }
}
