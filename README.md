### 这是一个简单的Unity框架，包含了
> - 单例
> - 缓存池
> - IOC容器
> - 可绑定属性
> - 事件
> - 指令模式
> - 观察者模式

# 架构规则


## 模块化基础规范

#### 对象之间的交互有三种
> - 方法
> - 委托
> - 事件

#### 模块化的三种常规方式
> - 单例
> - IOC
> - 分层

#### 程序开发约定规范
> - #### 自底向上用事件或者委托
> - #### 自顶向下用方法
> - #### 表现和数据分离
> - #### 交互逻辑和表现逻辑分离


## 系统设计架构
#### 架构分为四个层级：
> - __表现层：__
>   IController 接口，负责接收输入和当状态变化时更新表现，一般情况下 MonoBehaviour 均为表现层对象。
> - __系统层：__
>   ISystem 接口，帮助 IController 承担一部分逻辑，在多个表现层共享的逻辑，比如计时系统、商城系统、成就系统等。
> - __模型层：__
>   IModel 接口，负责数据的定义以及数据的增删改查方法的的提供。
> - __工具层：__
>   IUtility 接口，负责提供基础设施，比如存储方法、序列化方法、网络链接方法、蓝牙方法、SDK、框架集成等。
#### 使用规则：
> -	IController 更改 ISystem、IModel 的状态必须用 Command。
> -	ISystem、IModel 状态发生变更后通知 IController 必须用事件 或 BindableProeprty。
> -	IController 可以获取 ISystem、IModel 对象来进行数据查询。
> -	ICommand 不能有状态。
> -	上层可以直接获取下层对象，下层不能获取上层对象。
> -	下层像上层通信用事件。
> -	上层向下层通信用方法调用，IController 的交互逻辑为特使情况，只能用 Command。



## 模块行为
> - __IController__
>   - ICanSendCommand - 发送指令
>   - ICanGetSystem - 获取系统模块
>   - ICanGetModel - 获取数据模块
>   - ICanRegisterEvent - 监听事件
> - __ICommand__
>   - ICanSetArchitecture - 初始化架构
>   - ICanGetModel - 获取数据模块
>   - ICanGetUtility - 获取工具模块
>   - ICanGetSystem - 获取系统模块
>   - ICanSendEvent - 发送事件
>   - ICanSendCommand - 发送指令
> - ISystem
>   - ICanSetArchitecture - 初始化架构
>   - ICanGetUtility - 获取工具模块
>   - ICanGetModel - 获取数据模块
>   - ICanSendEvent - 发送事件
>   - ICanRegisterEvent - 监听事件
> - IModel
>   - ICanSetArchitecture - 初始化架构
>   - ICanGetUtility - 获取工具模块
>   - ICanSendEvent - 发送事件

## 事件规则
> - __IController__ 可以监听事件
> - __ICommand__ 可以发送事件
> - __ISystem__ 可以发送和监听事件
> - __IModel__ 可以发送事件