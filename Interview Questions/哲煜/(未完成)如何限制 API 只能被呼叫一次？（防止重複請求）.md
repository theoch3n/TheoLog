#### 📅 **Date**: 2025-03-14

#### 🔖 **Tags**: #Basic #BackEnd #InterviewQuestions

---

當 API 需要確保 **「每個請求只能執行一次，不能重複呼叫」**，我們可以採取 **去重（Idempotency）、Token 機制、Redis 緩存、資料庫記錄** 等方法來解決。

---

## **📍 1. 使用唯一請求 ID（Idempotency Key）**

**適用場景**：防止 **重複提交表單**、**交易請求**（如付款、訂單創建）

### **✅ 解決方案**

- **前端產生一個 `idempotency-key`，每次請求必須帶上這個 Key**
- **後端儲存 `idempotency-key`，如果相同 Key 已存在，則拒絕重複請求**

### **🔹 API 設計**

http

複製編輯

`POST /api/orders Authorization: Bearer token123 Idempotency-Key: abc123xyz`

### **🔹 後端實作（C# .NET / Redis）**

csharp

複製編輯

`public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request, [FromHeader] string idempotencyKey) {     var cacheKey = $"idempotency:{idempotencyKey}";      // 檢查 Redis 是否已經處理過這個請求     if (await _redis.ExistsAsync(cacheKey))         return Conflict("此請求已經處理，請勿重複提交");      // 標記請求已處理（設定有效期限，例如 10 分鐘）     await _redis.SetAsync(cacheKey, "processed", TimeSpan.FromMinutes(10));      // 執行實際業務邏輯     var order = _orderService.CreateOrder(request);     return Ok(order); }`

📌 **優勢**

- **適合付款、訂單等業務場景**
- **防止使用者誤觸按鈕造成重複提交**

---

## **📍 2. 使用 Redis 記錄請求（防止短時間重複 Call）**

**適用場景**：防止 **用戶短時間內重複請求**（如秒殺活動、API 防刷機制）

### **✅ 解決方案**

- **使用 Redis 記錄每個 API 請求，設置 TTL（Time-To-Live）**
- **如果短時間內請求相同 API，則直接拒絕**

### **🔹 後端實作（C# .NET / Redis）**

csharp

複製編輯

`public async Task<IActionResult> ProcessRequest(string userId) {     string requestKey = $"api_request:{userId}";      // 檢查 Redis 是否已存在該請求     if (await _redis.ExistsAsync(requestKey))         return Conflict("請求已處理，請勿重複請求");      // 記錄請求，設置 TTL 60 秒     await _redis.SetAsync(requestKey, "1", TimeSpan.FromSeconds(60));      return Ok("請求成功！"); }`

📌 **優勢**

- **適合 API Rate Limiting，防止多次請求**
- **使用 Redis，效能高、不影響資料庫負擔**

---

## **📍 3. 使用資料庫唯一約束（適用於交易類 API）**

**適用場景**：確保 **資料只會寫入一次**（如註冊、支付、訂單）

### **✅ 解決方案**

- **在資料庫內加上 `Unique Constraint`**
- **如果請求 ID 重複，則回傳錯誤**

### **🔹 SQL 設計**

sql

複製編輯

`CREATE TABLE orders (     id SERIAL PRIMARY KEY,     user_id INT NOT NULL,     order_number VARCHAR(255) UNIQUE, -- 確保不重複     amount DECIMAL(10,2) );`

### **🔹 C# 後端處理**

csharp

複製編輯

`public IActionResult CreateOrder(OrderRequest request) {     try     {         var order = new Order         {             OrderNumber = request.OrderNumber, // 由前端提供             UserId = request.UserId,             Amount = request.Amount         };          _db.Orders.Add(order);         _db.SaveChanges(); // 如果 OrderNumber 重複，會拋出錯誤         return Ok(order);     }     catch (DbUpdateException)     {         return Conflict("此訂單已存在，請勿重複提交！");     } }`

📌 **優勢**

- **即使 API 並行請求，資料庫層面仍然保證唯一性**
- **適合交易、付款等高安全性操作**

---

## **📍 4. 使用 Token 機制（前端請求一次後失效）**

**適用場景**：一次性請求（如驗證碼、臨時權限）

### **✅ 解決方案**

1️⃣ **後端產生 Token，前端請求 API 時必須帶上 Token**  
2️⃣ **請求後立即作廢，不可重複使用**

### **🔹 C# 後端實作**

csharp

複製編輯

`public IActionResult VerifyOnce(string token) {     if (_redis.Exists($"one_time_token:{token}"))         return Conflict("Token 已使用，請勿重複請求");      _redis.Set($"one_time_token:{token}", "used", TimeSpan.FromMinutes(5));      return Ok("驗證成功！"); }`

📌 **優勢**

- **適合一次性授權請求**
- **防止惡意重複提交**

---

## **📌 5. API 限制只能呼叫一次的方法對比**

|**方法**|**適用場景**|**優勢**|**缺點**|
|---|---|---|---|
|**Idempotency Key**|**付款、訂單請求**|高效防止重複提交|需要前端傳送唯一 Key|
|**Redis 防重複請求**|**防止 API 短時間重複 Call**|記憶體快，不影響 DB|需使用 Redis|
|**資料庫唯一約束**|**交易類操作**|保證資料完整性|影響資料庫性能|
|**一次性 Token**|**一次性驗證（OTP、驗證碼）**|可確保請求只能使用一次|需要額外管理 Token|

---

🔥 **面試技巧** 如果面試官問：「如何確保 API 只能被呼叫一次？」  
✅ **回答**：「可以使用多種方法，如 **Idempotency Key**（確保請求唯一）、**Redis 紀錄**（防止短時間內重複 Call）、**資料庫唯一約束**（確保交易不重複）、或 **一次性 Token**（適合臨時授權）。具體方案取決於 API 的業務需求。」