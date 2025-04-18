#### 📅 **Date**: 2025-04-08
#### 🔖 **Tags**: #Action #PowerApps #Dynamics365 #JavaScript #WebApi 

---
## 使用 Xrm.WebApi 呼叫 Action

`Xrm.WebApi.online.execute` 是 Power Apps 模型驅動應用程式用戶端 API 的一部分，它允許從 JavaScript 呼叫 Dataverse Web API 的動作 (Actions)，包括自訂建立的 Action。

### 參考資料

-   [Xrm.WebApi.online.execute (Client API reference) in model-driven apps - Power Apps | Microsoft Learn](https://learn.microsoft.com/en-us/power-apps/developer/model-driven-apps/clientapi/reference/xrm-webapi/online/execute)

### JavaScript 範例 (在 DevTools 中執行)

以下範例展示了如何在瀏覽器的開發人員工具 (DevTools) 中，準備請求物件並呼叫一個名為 `theo_GetAutoNumber` 的自訂 Action。這個 Action 預期接收三個字串類型的輸入參數：`Prefix`, `EntityName`, `AttributeName`。

```javascript
// 1. 定義請求物件 (Request Object)
var request = {
    // --- 輸入參數 ---
    // 傳遞給 Action 的具體參數值
    Prefix: "DEV", 
    EntityName: "theo_employee", 
    AttributeName: "theo_employee_id", 

    // --- 中繼資料函數 ---
    // getMetadata 函數定義了要呼叫的 Action 的相關資訊
    getMetadata: function() {
        return {
            // Action 的唯一名稱 (Unique Name)
            operationName: "theo_GetAutoNumber", 
            // 繫結參數，對於 Unbound Action 設為 null
            boundParameter: null, 
            // 操作類型：0 表示 Action
            operationType: 0, 
            // 定義 Action 參數的類型
            parameterTypes: {
                "Prefix": { 
                    "typeName": "Edm.String", // 參數類型 (Entity Data Model Type)
                    "structuralProperty": 1 // 參數結構屬性 (1: Primitive Type)
                },
                "EntityName": { 
                    "typeName": "Edm.String", 
                    "structuralProperty": 1 
                },
                "AttributeName": { 
                    "typeName": "Edm.String", 
                    "structuralProperty": 1 
                }
            }
        };
    }
};

// 2. 執行 Action 呼叫
// 使用 await 等待非同步操作完成，並將結果存儲在 result 變數中
// (需在 async 函數中執行，或在 DevTools 中直接使用頂層 await)
console.log("準備呼叫 Action: theo_GetAutoNumber");
result = await Xrm.WebApi.online.execute(request);
console.log("Action 呼叫完成，結果:", result);

// 3. 處理結果 (假設 Action 有回傳值)
if (result.ok) {
    console.log("Action 執行成功");
    // 解析 JSON 回應 (如果 Action 有回傳值)
    let response = await result.json(); 
    console.log("Action 回應:", response);
    // 例如，如果 Action 回傳一個名為 AutoNumber 的字串
    // var returnedValue = response.AutoNumber; 
    // console.log("取得的回傳值:", returnedValue);
} else {
    console.error("Action 執行失敗:", result.statusText);
    // 可以進一步解析錯誤訊息
    // let error = await result.json();
    // console.error("錯誤詳情:", error);
}

````

#### 說明：

1. **請求物件 (`request`)**：
    - 直接在此物件上定義 Action 所需的**輸入參數**及其值 (如 `Prefix`, `EntityName`, `AttributeName`)。
    - 包含一個名為 `getMetadata` 的**函數**，此函數返回一個描述 Action 中繼資料的物件。
2. **`getMetadata` 返回的物件**：
    - `operationName`: 指定要呼叫的 Action 的唯一名稱 (例如 `theo_GetAutoNumber`)。
    - `boundParameter`: 對於 Unbound Action (未繫結到特定實體記錄的 Action)，此值為 `null`。
    - `operationType`: `0` 代表這是一個 Action。
    - `parameterTypes`: 定義每個輸入/輸出參數的名稱、EDM 型別 (`typeName`) 和結構 (`structuralProperty`)。這是必需的，以便 `Xrm.WebApi` 知道如何正確序列化請求。
3. **執行呼叫**：
    - 使用 `await Xrm.WebApi.online.execute(request)` 執行非同步呼叫。
    - `await` 關鍵字（通常用在 `async` 函數中，但在現代瀏覽器的 DevTools 中可以直接使用）會等待 Action 執行完成。
4. **處理結果 (`result`)**：
    - `execute` 方法返回一個 [Response](https://developer.mozilla.org/en-US/docs/Web/API/Response) 物件。
    - 可以透過 `result.ok` 檢查呼叫是否成功 (HTTP 狀態碼 2xx)。
    - 如果成功且 Action 有回傳值，可以使用 `await result.json()` 來解析回應主體中的 JSON 資料。
    - 如果失敗，可以透過 `result.statusText` 或 `await result.json()` 查看錯誤訊息。