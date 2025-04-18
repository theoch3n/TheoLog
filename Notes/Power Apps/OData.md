#### 📅 **Date**: 2025-04-18 
#### 🔖 **Tags**: #OData #WebApi #Standard #Protocol #Dynamics365 #Dataverse #BusinessCentral

---
# OData (Open Data Protocol) 說明

**OData (Open Data Protocol)** 是一個由 OASIS 標準組織制定的開放標準協定，它定義了一組用於**建立和使用 RESTful API 的最佳實踐與慣例**。

簡單來說，OData 就像一套標準化的「網路溝通規則」，讓不同的應用程式能用一致、可預期的方式，透過 HTTP/HTTPS 來存取和操作資料。

---

## 核心目的與理念

1.  **標準化 REST API：** 統一資源定位 (URL)、CRUD 操作 (建立/讀取/更新/刪除) 和資料查詢的方式。
2.  **簡化資料存取：** 讓客戶端 (前端應用、整合服務等) 能更容易地與後端資料互動。
3.  **提升互操作性：** 基於通用的 Web 標準 (HTTP, URI, JSON/XML)，促進不同技術間的整合。

---

## 主要特色與概念

### 1. 基於 REST 原則
-   完全建立在 **HTTP/HTTPS** 協定之上。
-   使用標準 HTTP 動詞：`GET` (讀取), `POST` (建立), `PUT` / `PATCH` (更新), `DELETE` (刪除)。

### 2. 資源導向 (Resource-Based)
-   資料被視為「資源」，每種資源或資源集合都有唯一的 **URI**。
-   範例 URI：
    -   `/Customers` (所有客戶)
    -   `/Customers('ALFKI')` (特定客戶)
    * `/Customers('ALFKI')/Orders` (特定客戶的所有訂單)

### 3. 標準 URL 查詢選項 (Query Options)
-   OData 的核心優勢之一。允許客戶端在 URI 後加上 `$` 開頭的參數來精確控制回傳結果。
-   常見選項：
    * `$select`: 指定回傳欄位 (如 `$select=Name,Email`)。
    * `$filter`: 篩選資料 (如 `$filter=Country eq 'Taiwan'`)。
    * `$orderby`: 排序結果 (如 `$orderby=OrderDate desc`)。
    * `$top`: 限制筆數 (如 `$top=10`)。
    * `$skip`: 跳過筆數 (如 `$skip=20`)。
    * `$expand`: 載入關聯資料 (如 `$expand=Orders`)。
    * `$count`: 要求包含總筆數 (`$count=true`)。
    * `$search`: 全文檢索 (如 `$search=keyword`)。

### 4. 中繼資料 (Metadata)
-   OData 服務通常提供一個 **`$metadata`** 文件 (CSDL XML 格式)。
-   描述服務的資料模型：實體、屬性、關聯、可用操作等。
-   使 API 具有**自我描述性**，便於客戶端探索。

### 5. 資料格式 (Data Formats)
-   最常用的是 **JSON** (帶有 OData 特定標註)。
-   也支援 Atom/XML。

### 6. 動作 (Actions) 與函式 (Functions)
-   除了 CRUD，還可定義自訂操作：
    * **Actions:** 有副作用的操作 (如：核准訂單)。
    * **Functions:** 無副作用的唯讀操作或計算 (如：取得庫存)。

---

## OData 的優點

-   **互操作性強：** 標準化，易於整合。
-   **開發效率高：** 客戶端透過查詢選項自行取得所需資料，減少後端負擔。
-   **降低網路負載：** 只傳輸必要資料。
-   **易於探索：** 中繼資料使 API 自我描述。
-   **基於 Web 標準：** 學習曲線平緩。

---

## 與 Dynamics 365 / BC 的關聯

-   **Dataverse Web API：** 就是一個 OData v 4 服務端點，是與 Dataverse 互動的主要方式。先前筆記中用 `Xrm.WebApi.online.execute` 呼叫 Action，底層就是透過 OData。
-   **Business Central APIs：** 也提供基於 OData 的 API 供外部整合。
-   **Plugin 觸發：** 如先前文件所述，由 OData 端點觸發的事件也能執行 Plugin，顯示其在平台中的整合性。

總之，OData 是建立現代化、彈性且易於整合的 Web API 的重要標準。