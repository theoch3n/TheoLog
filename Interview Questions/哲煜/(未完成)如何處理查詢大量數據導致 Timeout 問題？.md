#### 📅 **Date**: 2025-03-12

#### 🔖 **Tags**: #SQL #BackEnd #InterviewQuestions #PendingOrganization

---

當要匯出**大量資料**（如報表、日誌、交易記錄等）時，可能會因為 **查詢時間過長** 或 **系統資源耗盡** 而導致 **Timeout**。這類問題在 **資料庫查詢、API、前端渲染** 都可能發生，解決方案可以從 **查詢優化、分批處理、非同步處理** 等方面入手。

---

## **📍 1. 使用分頁（Pagination）查詢**

### **✅ 問題**

如果一次查詢 **數百萬筆資料**，可能會超過資料庫的執行時間限制，導致超時（Timeout）。

### **✅ 解決方案**

使用 **分頁（Paging）**，一次只讀取部分資料，而不是一次性加載所有數據。

sql

複製編輯

`SELECT * FROM orders ORDER BY id LIMIT 1000 OFFSET 0; -- 第一頁 SELECT * FROM orders ORDER BY id LIMIT 1000 OFFSET 1000; -- 第二頁`

**✅ 優勢**

- **減少單次查詢壓力**
- **提高響應速度**
- **減少伺服器記憶體佔用**

---

## **📍 2. 逐步流式讀取（Streaming Data）**

### **✅ 問題**

如果應用程式一次讀取大量數據，可能會耗盡記憶體（Out of Memory）。

### **✅ 解決方案**

使用 **流式讀取（Streaming）**，只在記憶體中保留一小部分數據，處理完後再繼續讀取。

### **🔹 C# 使用 `IQueryable` + `AsEnumerable()`**

csharp

複製編輯

`var query = dbContext.Orders.AsQueryable();  foreach (var order in query.AsEnumerable()) {     ProcessOrder(order); }`

📌 **優勢**：`AsEnumerable()` 會將數據**逐筆載入**，而非一次載入全部。

---

## **📍 3. 產生批次檔案（Batch File）**

### **✅ 問題**

如果使用者**一次性匯出 1GB CSV**，可能會導致請求超時或記憶體溢出。

### **✅ 解決方案**

**先在後端生成檔案，完成後提供下載連結。** 1️⃣ **後端產生檔案**

csharp

複製編輯

`public async Task<IActionResult> ExportLargeFile() {     var filePath = "export/orders_202403.csv";     using (var writer = new StreamWriter(filePath))     {         foreach (var order in dbContext.Orders.AsEnumerable())         {             writer.WriteLine($"{order.Id},{order.CustomerName},{order.TotalPrice}");         }     }     return Ok(new { url = "/files/orders_202403.csv" }); }`

2️⃣ **前端輪詢查詢狀態**

javascript

複製編輯

`async function checkFileStatus() {     let response = await fetch("/api/export/status");     let data = await response.json();     if (data.ready) {         window.location.href = data.url;     } }`

📌 **優勢**：

- **後端異步處理，不影響前端體驗**
- **減少單次請求壓力**
- **避免超時（Timeout）問題**

---

## **📍 4. 直接使用資料庫索引（Index）**

### **✅ 問題**

如果查詢沒有索引，可能會**全表掃描（Full Table Scan）**，導致查詢速度極慢。

### **✅ 解決方案**

✅ **為查詢條件加上索引**

sql

複製編輯

`CREATE INDEX idx_orders_date ON orders(order_date);`

✅ **避免 `SELECT *`，只查詢需要的欄位**

sql

複製編輯

`SELECT id, customer_name, total_price FROM orders WHERE order_date > '2024-01-01';`

📌 **優勢**

- **查詢速度提升 10-100 倍**
- **降低資料庫負載**
- **防止查詢超時**

---

## **📍 5. 轉為後台非同步處理**

### **✅ 問題**

如果 API 直接執行長時間的報表生成，可能會超時（HTTP Timeout）。

### **✅ 解決方案**

✅ **改用後台 Queue 任務**

csharp

複製編輯

`public async Task<IActionResult> GenerateReport() {     BackgroundJob.Enqueue(() => GenerateReportTask());     return Ok("報表生成中，稍後下載"); }  public void GenerateReportTask() {     // 生成報表的邏輯 }`

📌 **優勢**

- **避免前端等待**
- **防止請求超時**
- **允許背景異步執行**

---

## **📍 6. 提高 API Timeout 設定**

如果 API 本身有時間限制（如 30 秒超時），可以適當增加 `Timeout`。

### **🔹 C# 調整 `HttpClient` Timeout**

csharp

複製編輯

`var client = new HttpClient(); client.Timeout = TimeSpan.FromMinutes(5);`

### **🔹 Nginx / Apache 設定 Timeout**

📌 **Nginx**

nginx

複製編輯

`proxy_read_timeout 300;`

📌 **Apache**

apache

複製編輯

`Timeout 300`

📌 **優勢**

- **允許較長時間的請求**
- **適用於大型報表或 API**

---

## **📌 總結**

|**方法**|**適用場景**|**優勢**|
|---|---|---|
|**分頁查詢（Pagination）**|查詢大量數據|降低查詢壓力，提高速度|
|**流式讀取（Streaming）**|避免記憶體溢出|逐筆處理，不佔用記憶體|
|**產生批次檔案（Batch File）**|匯出大型 CSV、Excel|非同步處理，防止超時|
|**索引（Index）優化**|查詢數據庫大表|查詢加速 10-100 倍|
|**後台 Queue 任務**|長時間報表生成|非同步處理，不影響用戶體驗|
|**提高 API Timeout**|API 處理時間過長|適用於長時間運行的請求|

---

🔥 **面試技巧** 如果面試官問：「如果要匯出大量資料，發生 Timeout，你會怎麼解決？」  
✅ **回答**：「我會考慮幾種方法，包括 **分頁查詢、流式讀取、索引優化、後台非同步處理、產生批次檔案**，具體方法取決於應用場景和性能需求。」