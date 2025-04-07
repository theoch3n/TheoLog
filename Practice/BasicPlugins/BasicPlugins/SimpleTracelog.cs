using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace BasicPlugins
{
    public class SimpleTracelog : IPlugin   // 實作 IPlugin 介面
    {
        // 實作 IPlugin 介面要求的 Execute 方法，是 Plugin 的進入點
        public void Execute(IServiceProvider serviceProvider)
        {
            // 從 serviceProvider 取得 Plugin 執行內容 (Execution Context)
            // context 包含觸發事件的詳細資訊 (是哪個實體、哪個訊息、哪個使用者等)
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // 從 ServiceProvider 取得組織服務工廠 (Organization Service Factory)
            // 這個工廠用於建立能與 Dataverse/Dynamics 365 互動的服務物件
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // 使用工廠建立組織服務 (Organization Service) 的實例
            // context.UserId 表示這個服務會以觸發事件的使用者權限來執行操作
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // 從 ServiceProvider 取得追蹤服務 (Tracing Service)
            // tracer 用於紀錄 Plugin 執行過程中的訊息
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracer.Trace("SimpleTracelog Activated!");
        }
    }
}
