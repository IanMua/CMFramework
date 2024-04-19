namespace CMFramework
{
    public interface ICanSetArchitecture
    {
        /// <summary>
        /// 给架构赋值
        /// </summary>
        /// <param name="architecture"></param>
        void SetArchitecture(IArchitecture architecture);
    }
}