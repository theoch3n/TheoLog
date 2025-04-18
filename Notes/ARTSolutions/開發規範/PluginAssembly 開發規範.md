#### 📅 **Date**: 2025-04-18

#### 🔖 **Tags**: #PluginAssembly #PowerApps #Dynamics365 #CSharp 

---

**1. 專案資料夾結構 (Folder Structure)**

- **強制要求:** 每個 Plugin Assembly 的專案根目錄下**必須**包含一個名為 `Plugins` 的子資料夾。
- **建議結構:** 除了 `Plugins` 資料夾外，建議根據功能將程式碼分散到其他資料夾，例如：
    - `Services`: 存放提供特定服務或封裝複雜邏輯的類別 (例如，呼叫外部 API、執行複雜的資料處理)。
    - `Models`: 存放資料傳輸物件 (DTO) 或自訂的資料結構類別。
    - `Enums`: 存放列舉型別。
- **結構範例:**

```
    ApprovalPlugins  (<- 專案根目錄, Plugin Assembly 名稱)
    ├── Plugins       (<- 強制要求，存放 Plugin 類別)
    │   ├── RetrieveApproverPlugin.cs
    │   ├── RequestApprovalFlowPlugin.cs
    │   └── UpdateApprovalStatusPlugin.cs
    ├── Services      (<- 建議，存放服務類別)
    │   ├── ApprovalDataService.cs
    │   └── ApprovalNotificationService.cs
    ├── Models        (<- 建議，存放資料模型類別)
    │   ├── ApprovalFlowModel.cs
    │   └── ApprovalNodeModel.cs
    └── Enums         (<- 建議，存放列舉型別)
        ├── ApprovalStatus.cs
        └── ApprovalResult.cs
```

- **目的:** 清晰地分離不同職責的程式碼。將核心的 Plugin 邏輯 (實作 `IPlugin` 的類別) 集中在 `Plugins` 資料夾，使得尋找和管理 Plugin 步驟對應的程式碼更加容易。其他輔助類別（Services, Models, Enums）的分離則提高了程式碼的模組化和可重用性。

**2. 檔案與類別組織 (File and Class Organization)**

- **Plugin 檔案:** 每一個 Plugin 類別都應該放在 `Plugins` 資料夾下的一個獨立 `.cs` 檔案中。
- **單一職責原則 (檔案層級):** 每一個 `.cs` 檔案**只應包含一個** `class`, `interface`, 或 `enum` 的宣告。
- **目的:** 提高程式碼的可讀性，方便透過檔名快速定位特定的類別、介面或列舉。這也是 C# 開發的通用最佳實踐。

**3. 命名規範 (Naming Conventions)**

- **Plugin 類別名稱:** 實際執行 Plugin 邏輯的類別（即實作 `IPlugin` 的類別），其名稱**必須以 "Plugin" 結尾**。
    - 範例: `RetrieveApproverPlugin`, `UpdateApprovalStatusPlugin`。
- **Plugin 命名空間 (Namespace):** 建議遵循 `<公司名稱>.<模組名稱>.<功能名稱>Plugins` 的格式。
    - 範例: `Twtoto.OrderManagement.SAPIntegrationPlugins`。
    - 在提供的 Plugin 程式碼範例中，使用了較簡潔的 `ApprovalPlugins.Plugins`，這也適用於較小或內部專案，但大型專案建議使用更詳細的格式以避免衝突。
- **目的:**
    - 類別名稱後綴 "Plugin" 能清晰識別出哪些類別是可註冊的 Plugin 步驟。
    - 結構化的命名空間有助於組織大型專案的程式碼，防止不同模組或功能間的命名衝突。

**4. Plugin 程式碼格式 (Plugin Code Format)**

- **基本結構:** 提供了一個標準的 Plugin 類別範本。

```csharp
    // Namespace 應符合資料夾結構與命名規範
    namespace ApprovalPlugins.Plugins // 或例如 Twtoto.OrderManagement.ApprovalPlugins.Plugins
    {
        // 類別名稱以 Plugin 結尾並實作 IPlugin 介面
        public class RetrieveApproverPlugin : IPlugin
        {
            // 宣告私有成員變數以供 Execute 方法使用
            IPluginExecutionContext context;
            IOrganizationServiceFactory serviceFactory;
            IOrganizationService service; // 代表 Dataverse/CRM 的主要互動服務
            ITracingService tracingService; // 用於記錄追蹤資訊

            // 實作 IPlugin 介面的 Execute 方法，這是 Plugin 的進入點
            public void Execute(IServiceProvider serviceProvider)
            {
                // --- 標準初始化步驟 ---
                // 1. 取得 Plugin 執行上下文 (包含觸發事件的資訊)
                context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // 2. 取得 Organization Service Factory (用於建立 Service)
                serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                // 3. 建立 Organization Service 實例 (使用觸發 Plugin 的使用者權限)
                service = serviceFactory.CreateOrganizationService(context.UserId);

                // 4. 取得 Tracing Service (用於偵錯與記錄)
                tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                // --- 實際的 Plugin 業務邏輯寫在這裡 ---
                tracingService.Trace("RetrieveApproverPlugin started.");
                try
                {
                    // 例如：檢查 InputParameters 是否包含 Target
                    if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                    {
                        Entity entity = (Entity)context.InputParameters["Target"];
                        tracingService.Trace($"Processing entity: {entity.LogicalName}, ID: {entity.Id}");

                        // ... 呼叫 Service 或直接撰寫資料處理邏輯 ...
                        // var dataService = new ApprovalDataService(service, tracingService);
                        // var approver = dataService.FindApprover(entity);
                        // context.OutputParameters["Approver"] = approver;
                    }
                    tracingService.Trace("RetrieveApproverPlugin finished successfully.");
                }
                catch (Exception ex)
                {
                    tracingService.Trace($"Error in RetrieveApproverPlugin: {ex.ToString()}");
                    // 異常處理，可以選擇性地拋出 InvalidPluginExecutionException
                    throw new InvalidPluginExecutionException($"An error occurred in RetrieveApproverPlugin: {ex.Message}", ex);
                }
            }
        }
    }
```

- **核心概念:**
    - **`IPlugin` 介面:** 任何要註冊為 Plugin 步驟的類別都必須實作此介面及其 `Execute` 方法。
    - **`IServiceProvider`:** `Execute` 方法的參數，用於獲取 Plugin 執行時所需的各種服務 (Context, Service Factory, Tracing Service)。
    - **`IPluginExecutionContext`:** 提供有關觸發 Plugin 的事件信息（例如，觸發訊息 MessageName、階段 Stage、觸發實體 Target、深度 Depth 等）。
    - **`IOrganizationServiceFactory` & `IOrganizationService`:** 用於與 Dataverse/CRM 資料庫進行互動 (CRUD 操作)。透過 Factory 以觸發使用者的身份 (`context.UserId`) 建立 Service 是標準做法。
    - **`ITracingService`:** 用於記錄 Plugin 執行過程中的資訊，對於偵錯非常重要。記錄的內容可以透過 Plugin Trace Log 查看。
    - **異常處理:** 建議使用 `try-catch` 區塊包覆主要邏輯，並使用 `tracingService` 記錄錯誤，可以選擇性拋出 `InvalidPluginExecutionException` 將錯誤訊息顯示給使用者。
