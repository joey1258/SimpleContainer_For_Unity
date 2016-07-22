# uMVVMCS
DI+MVVC+Controller+Servers Framework

参考 StrangeIoC、adic 等现有框架。

目前尚处于雏形阶段，完善注入容器部分。

binding 的 type 不再如 StrangeIoC 对应多个 type，每个 binding 只对应一个唯一的 key：type。

binding 不再内含更低粒度的 semibinding，binding 自身为最低粒度单位。

binder 会将所有 binding 储存在以 type 为 key 的字典中，为了 get 方法更加快速，当 id 不为空时将会将引用分别存在另外两个字典中以便获取。

无 id 的 TEMP 类型 binding 将不再被保存在 binder 中，既然是临时类型，保存引用在字典中又没有高效的删除方法，不如即用即弃；如需反复使用，不建议使用 TEMP 类型，或为其加上 id




