#### 📅 **Date**: 2025-04-07

#### 🔖 **Tags**: #PluginAssembly #PowerApps #Dynamics365 #CSharp 

---
## NuGet 套件

開發 Plugin Assembly 需要引用以下核心 NuGet 套件：
-   `Microsoft.CrmSdk.CoreAssemblies` - 包含核心 SDK 型別與介面 (如 IPlugin, IOrganizationService 等)。
-   `Microsoft.CrmSdk.XrmTooling.PluginRegistrationTool` - 提供 Plugin Registration Tool (`PluginRegistration.exe`)，用於部署 Plugin 到環境中。

---
## IPlugin 介面

-   `: IPlugin` 語法表示一個類別**實作** (implements) 了 `IPlugin` 介面。
-   在 Dynamics 365 / Power Platform Dataverse 中，任何要作為外掛程式 (Plugin) 執行的 C# 類別都**必須**實作 `IPlugin` 介面。
-   此介面要求實作一個 `Execute` 方法，作為 Plugin 的主要進入點。

---

## 設定金鑰 (Assembly Signing)

為了讓 Dynamics 365 / Dataverse 能夠信任並註冊您的 Plugin Assembly，必須對其進行簽署。

> **重要：** 簽署是識別 Assembly 唯一性的方式。

**步驟：**

1.  在 Visual Studio 中打開專案的**屬性 (Properties)**。
2.  切換到 **簽署 (Signing)** 頁籤。
3.  勾選 **簽署組件 (Sign the assembly)** 核取方塊。
4.  在 **選擇強式名稱金鑰檔 (Choose a strong name key file)** 下拉選單中，選擇 `<新增...>` (`<New...>`)。
5.  輸入金鑰檔案名稱（例如 `MyKey.snk`），**不需要**設定密碼。按確定建立金鑰檔。

---

## 將 Plugin DLL 加入環境 (Registering Assembly)

將已編譯好的 Plugin Assembly (.dll 檔) 部署到目標 Dataverse 環境。

**前置作業：**

-   需先透過 NuGet 安裝 `Microsoft.CrmSdk.XrmTooling.PluginRegistrationTool` 套件。

**步驟：**
1.  在 Visual Studio 中建置專案 (快速鍵：`Ctrl + Shift + B`)，確保產生最新的 .dll 檔。
2.  在方案總管中，找到 `packages` 資料夾 (或 NuGet 快取位置)，尋找 `Microsoft.CrmSdk.XrmTooling.PluginRegistrationTool` 資料夾內的 `tools` 資料夾，執行 **`PluginRegistration.exe`**。
3.  在 Plugin Registration Tool 中，點擊 **`CREATE NEW CONNECTION`**。
4.  選擇您的部署類型 (通常是 Microsoft 365)，輸入登入資訊，並選擇要部署的目標 Dataverse 環境。
5.  登入成功後，點擊 **`Register`** -> **`Register New Assembly`**。
6.  在彈出視窗中，點擊步驟 1 右側的 `...` 按鈕，導覽至您專案的輸出路徑 (通常是 `專案資料夾/bin/Debug/` 或 `專案資料夾/bin/Release/`)，選擇您編譯好的 **Plugin .dll 檔案** (例如 `BasicPlugins.dll`)。
7.  步驟 2 會列出該 .dll 檔中所有實作了 `IPlugin` 的類別。勾選您想要註冊的 Plugin 類別 (若有多個 Plugin 類別，它們會一起被註冊或更新，無法單獨處理同個 Assembly 內的 Plugin)。
8.  確認 **Isolation Mode** (通常選 `Sandbox`) 與 **Location** (通常選 `Database`)。
9.  點擊 **`Register Selected Plugins`**。
10. 註冊成功後，即可在左側的 Assembly 清單中看到您剛剛註冊的 Assembly 及其包含的 Plugin 類別。此時 Plugin 只是被部署上去，尚未設定觸發條件。

> **注意：** 同一個 Assembly 裡的 Plugin 無法個別更新，只能以 Assembly 為單位進行更新 (Update)。

---

## 註冊 Plugin 執行步驟 (Registering Step)

設定 Plugin 在何種事件、哪個實體、哪個階段被觸發執行。

**步驟：**

1.  在 Plugin Registration Tool 中，找到您要設定的 Plugin 類別 (例如 `BasicPlugins.SimpleTracelog`)，點擊**右鍵**。
2.  選擇 **`Register New Step`**。
3.  在 `Register New Step` 視窗中進行設定：
    * **`Message`**: 選擇觸發事件的名稱。這是核心的動作，例如 `Create` (建立), `Update` (更新), `Delete` (刪除), `Retrieve` (讀取單筆), `RetrieveMultiple` (讀取多筆) 等。
    * **`Primary Entity`**: 選擇此事件所作用的實體 (Table) 的邏輯名稱 (例如 `account`, `contact`, `xxx_demotable`)。
    * **`Eventing Pipeline Stage of Execution`**: 選擇 Plugin 在事件處理管線中的執行階段。常用的選項是 `PostOperation`。
        > -   **`PreValidation`**: 在主要系統驗證之前執行 (交易外部)。適合做早期驗證，若拋出錯誤可中止後續操作。
        > -   **`PreOperation`**: 在主要系統操作 (如資料庫寫入) 執行**之前**，但在資料庫交易內部執行。適合修改即將寫入的資料。
        > -   **`PostOperation`**: 在主要系統操作執行**之後**，且在資料庫交易內部執行。這是最常用的階段，可以存取已完成操作的結果 (如新建記錄的 ID)，並可執行後續相關操作。
    * **`Execution Mode`**: 通常選擇 `Synchronous` (同步)。
    * 其他設定如 `Filtering Attributes`, `Run in User's Context` 等依需求設定。
4.  點擊 **`Register New Step`**。
5.  完成後，即可在 Plugin 類別下方看到註冊的 Step。此時，當符合條件的事件發生時，Plugin 就會被觸發執行。

---
## 範例: SimpleTracelog

這個範例展示了一個基本的 Plugin，它會在觸發時：
1.  查詢 `theo_demotable` 的所有紀錄。
2.  記錄查詢到的 `theo_name`。
3.  將查詢到的總筆數更新回觸發此 Plugin 的那筆 `theo_demotable` 記錄的 `theo_index` 欄位。

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace BasicPlugins
{
    // 實作 IPlugin 介面，用於 Dynamics 365 Plugin 開發
    public class SimpleTracelog : IPlugin
    {
        // 實作 IPlugin 介面要求的 Execute 方法，作為 Plugin 的進入點
        public void Execute(IServiceProvider serviceProvider)
        {
            #region 1. 取得執行環境與服務
            // 從 serviceProvider 取得插件執行上下文 (IPluginExecutionContext)
            // context 包含觸發事件的詳細資訊，例如涉及的實體、操作類型和觸發的使用者
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // 從 serviceProvider 取得組織服務工廠 (IOrganizationServiceFactory)
            // 用於建立與 Dynamics 365/Dataverse 互動的組織服務物件
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // 使用工廠建立組織服務 (IOrganizationService) 實例
            // 以觸發事件的使用者權限 (context.UserId) 執行操作
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // 從 serviceProvider 取得追蹤服務 (ITracingService)
            // 用於記錄插件執行過程中的訊息，方便除錯
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            #endregion

            #region 2. 業務邏輯
            // 記錄一條追蹤訊息
            tracer.Trace("SimpleTracelog Activated!");

            // 從上下文的輸入參數中取得觸發事件的目標實體 (Target)
            // 目標實體是觸發插件操作的記錄，例如正在建立或更新的記錄
            // 需要確保 InputParameters 包含 "Target" 且其型別為 Entity
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity target = (Entity)context.InputParameters["Target"];

                // 建立查詢表達式 (QueryExpression)，用於查詢 theo_demotable 實體
                QueryExpression query = new QueryExpression()
                {
                    EntityName = "theo_demotable",              // 指定查詢的實體名稱
                    ColumnSet = new ColumnSet("theo_name"),     // 指定查詢 theo_name 欄位
                    Criteria = new FilterExpression()           // 初始化過濾條件，類似 SQL 的 WHERE 子句
                };
                // 過濾 theo_name 等於 "test" 的記錄 (此行被註解掉，表示查詢所有記錄)
                // query.Criteria.AddCondition("theo_name", ConditionOperator.Equal, "test");

                // 執行查詢，取得符合條件的記錄集合 (EntityCollection)
                EntityCollection result = service.RetrieveMultiple(query);

                // 遍歷查詢結果中的每筆記錄
                tracer.Trace(<span class="math-inline">"Found \{result\.Entities\.Count\} records in theo\_demotable\:"\);
foreach \(Entity record in result\.Entities\)
\{
// 記錄每筆記錄的 theo\_name 欄位值
if \(record\.Contains\("theo\_name"\)\)
\{
tracer\.Trace\(</span>" - Record Name: {record["theo_name"].ToString()}");
                    }
                    else
                    {
                        tracer.Trace(" - Record found but theo_name is null.");
                    }
                }

                // 取得查詢到的記錄總數
                int index = result.Entities.Count;

                // 建立一個新的 Entity 物件，用於更新目標記錄
                // 使用目標記錄的 Id 指定要更新的記錄
                Entity targetToUpdate = new Entity(target.LogicalName, target.Id); // 使用 target.LogicalName 更通用

                // 將查詢到的記錄數量設定到 theo_index 欄位
                targetToUpdate["theo_index"] = index;

                // 呼叫服務更新目標記錄，將變更儲存到 Dynamics 365
                tracer.Trace($"Updating target record {target.Id} with index {index}");
                service.Update(targetToUpdate);
                tracer.Trace("Target record updated successfully.");
            }
            else
            {
                tracer.Trace("Target entity not found in InputParameters.");
            }
            #endregion
        }
    }
}
```

---

## 程式碼詳解

### 1. 類別與介面

```csharp
public class SimpleTracelog : IPlugin
```

- `SimpleTracelog` 類別實作了 `IPlugin` 介面，這是 Dynamics 365 / Dataverse Plugin 的標準要求。

### 2. Execute 方法

```csharp
public void Execute(IServiceProvider serviceProvider)
```

- `Execute` 方法是 Plugin 被觸發時，平台會呼叫的**入口點**。
- `serviceProvider` 參數是一個服務容器，提供了 Plugin 執行時所需的各種服務物件。

### 3. 取得服務

程式碼的第一部分是從 `serviceProvider` 中取得必要的服務：

```csharp
IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
```

- **目的**：取得 **`IPluginExecutionContext` (執行上下文)**，它包含觸發 Plugin 事件的詳細資訊（例如：觸發訊息 `MessageName`、實體邏輯名稱 `PrimaryEntityName`、執行階段 `Stage`、觸發使用者 `UserId`、輸入/輸出參數 `InputParameters`/`OutputParameters` 等）。
- **細節**：上下文是理解 Plugin 為何觸發以及存取相關資料的關鍵。

```csharp
IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
```

- **1目的**：建立 **`IOrganizationService` (組織服務)** 實例，這是與 Dataverse 進行資料互動（CRUD - 建立、讀取、更新、刪除）的主要介面。
- **細節**：
    
    - `IOrganizationServiceFactory` 用於建立服務實例。
    
    - 使用 `context.UserId` 建立服務，確保後續的資料操作是以**觸發此 Plugin 的使用者**的權限來執行，遵循其安全角色設定。

```csharp
ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
```

- **目的**：取得 **`ITracingService` (追蹤服務)**，用於在 Plugin 執行過程中記錄自訂的偵錯或流程訊息。
- **細節**：這些追蹤記錄對於開發和除錯非常重要，可以在 Dataverse 環境中的「外掛程式追蹤記錄 (Plug-in Trace Log)」中查看（需先啟用追蹤功能）。

### 4. 業務邏輯

```csharp
tracer.Trace("SimpleTracelog Activated!");
```

- 使用追蹤服務記錄一條簡單訊息，表示 Plugin 已被成功觸發並開始執行。

```csharp
if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
{
    Entity target = (Entity)context.InputParameters["Target"];
    // ... 後續邏輯 ...
}
```

- 檢查執行上下文的 `InputParameters` 集合中是否存在名為 **`Target`** 的參數，並且該參數的型別是 `Entity`。
- **細節**：
    - 對於 `Create`, `Update` 等操作，`Target` 參數通常包含了觸發 Plugin 的那筆實體記錄的資料。
    - 這個檢查是必要的，確保 Plugin 在預期的上下文中執行，且能安全地取得目標實體。
    - `target` 物件即為觸發事件的實體記錄，可以存取其欄位（屬性）。

```csharp
QueryExpression query = new QueryExpression() { ... };
```

- 建立一個 `QueryExpression` 物件，這是用程式碼定義查詢條件的方式之一。
- **細節**：
    - `EntityName = "theo_demotable"`：指定要查詢的實體（Table）。
    - `ColumnSet = new ColumnSet("theo_name")`：指定只取回 `theo_name` 這個欄位的資料，避免查詢不必要的欄位以提升效能。
    - `Criteria = new FilterExpression()`：初始化查詢條件，此處未加任何條件 (`AddCondition`)，表示查詢所有記錄。

```csharp
// query.Criteria.AddCondition("theo_name", ConditionOperator.Equal, "test");
```

- 這行被註解掉的程式碼展示了如何加入過濾條件，例如只查詢 `theo_name` 欄位值等於 "test" 的記錄。

```csharp
EntityCollection result = service.RetrieveMultiple(query);
```

- 使用先前建立的 `IOrganizationService` 物件 (`service`) 執行 `RetrieveMultiple` 方法，傳入定義好的 `QueryExpression` (`query`)。
- **細節**：
    - `RetrieveMultiple` 會根據查詢條件從 Dataverse 中擷取多筆記錄。
    - 返回的 `result` 是一個 `EntityCollection` 物件，它包含了所有符合條件的 `Entity` 物件（記錄）的集合。

```csharp
foreach (Entity record in result.Entities) {
    // 加入 Contains 檢查更安全
    if (record.Contains("theo_name")) 
    {
        tracer.Trace($" - Record Name: {record["theo_name"].ToString()}");
    }
}
```

- 使用 `foreach` 迴圈遍歷 `result.Entities` 集合中的每一筆 `Entity` 記錄 (`record`)。
- 在迴圈內，使用 `tracer.Trace()` 記錄下每筆記錄的 `theo_name` 欄位值。
- **細節**：
    - `record["theo_name"]` 透過欄位名稱存取該記錄的屬性值。
    - `.ToString()` 將屬性值轉為字串以便記錄。加入 `record.Contains("theo_name")` 檢查更為穩健。

```csharp
int index = result.Entities.Count;
```

- 取得 `result.Entities` 集合中的記錄總數 (即查詢到的總筆數)，並存入 `index` 變數。

```csharp
Entity targetToUpdate = new Entity(target.LogicalName, target.Id);
targetToUpdate["theo_index"] = index;
service.Update(targetToUpdate);
```

- 建立一個新的 `Entity` 物件 (`targetToUpdate`)，用於執行**更新**操作。
- **細節**：
    - 建立 `Entity` 時，需指定目標實體的**邏輯名稱** (`target.LogicalName` 比硬編碼 "theo_demotable" 更通用) 和**記錄的唯一識別碼 (GUID)** (`target.Id`)。這告訴 Dataverse 要更新的是哪一筆記錄。
    - `targetToUpdate["theo_index"] = index;` 設定要更新的欄位 (`theo_index`) 及其新值 (`index` - 查詢到的記錄總數)。**只設定要變更的欄位**是最佳實踐。
    - 最後呼叫 `service.Update(targetToUpdate)`，將這個只包含變更資訊的 `Entity` 物件傳給 Dataverse，執行實際的更新操作。

---

### Plugin 功能總結
1. **觸發**：當 `theo_demotable` 實體上的某個已註冊步驟的事件 (例如 `Create` 或 `Update`) 發生時，此 Plugin 被觸發。
2. **記錄啟動**：使用追蹤服務記錄一條訊息，確認 Plugin 已啟動。
3. **取得目標**：從執行上下文中安全地取得觸發事件的 `theo_demotable` 記錄 (`Target`)。
4. **查詢**：查詢 Dataverse 中所有的 `theo_demotable` 記錄，僅擷取 `theo_name` 欄位。
5. **記錄查詢結果**：遍歷查詢結果，並使用追蹤服務記錄下每筆記錄的 `theo_name` 值。
6. **計算總數**：計算查詢到的總記錄數。
7. **更新記錄**：將計算出的總記錄數，更新回觸發此 Plugin 的那筆 `theo_demotable` 記錄的 `theo_index` 欄位中。