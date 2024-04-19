namespace CMFramework
{
    public enum BindablePropertyEventModelEnum
    {
        /// <summary>
        /// 忽略规则触发
        /// </summary>
        /// <remarks>只要修改值，立刻触发</remarks>
        IgnoreRuleTrigger,
        /// <summary>
        /// 立刻触发
        /// </summary>
        /// <remarks>修改值的时候不比较值是否相同，只要不为空就触发</remarks>
        ImmediatelyTrigger,
        /// <summary>
        /// 更新触发
        /// </summary>
        /// <remarks>修改值的时候只有值不为空且与之前的值不相同才触发</remarks>
        UpdateTrigger,
    }
}