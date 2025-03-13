#### 📅 **Date**: 2025-03-12

#### 🔖 **Tags**: #MVC #DesignPattern #InterviewQuestions

---

HTTP（**HyperText Transfer Protocol，超文本傳輸協議**）是**網頁和 API 通訊的核心協議**，瀏覽器與伺服器之間的溝通都是透過 **HTTP Request（請求）** 和 **HTTP Response（回應）** 來完成。

---

## **📍 1. HTTP Request 的基本結構**

HTTP Request 主要包含 4 個部分： 
1️⃣ **請求行（Request Line）** → **請求方法（Method）、URL、協議版本**  
2️⃣ **請求標頭（Headers）** → 相關資訊，如 `Content-Type`、`Authorization`  
3️⃣ **請求主體（Body）** → 送出的數據（僅適用於 `POST`、`PUT`）  
4️⃣ **查詢參數（Query Parameters）** → `?key=value` 形式的額外資訊

---

## **📍 2. HTTP Request 結構範例**

📌 **使用 `GET` 方法請求**

http

複製編輯

`GET /api/users?name=Alice HTTP/1.1 Host: example.com User-Agent: Mozilla/5.0 Accept: application/json`

📌 **使用 `POST` 方法請求**

http

複製編輯

`POST /api/users HTTP/1.1 Host: example.com Content-Type: application/json Authorization: Bearer token123  {     "name": "Alice",     "age": 25 }`

🔹 **`GET` 無 Body，`POST` 透過 Body 傳遞數據**  
🔹 **`Authorization: Bearer token123`** 用於 API 授權

---

## **📍 3. 常見的 HTTP Request 方法**

|**方法**|**用途**|**是否有 Request Body**|
|---|---|---|
|`GET`|取得資源（Read）|❌ 無|
|`POST`|**新增** 資源（Create）|✅ 有|
|`PUT`|**更新** 整個資源（Update）|✅ 有|
|`PATCH`|**部分更新** 資源（Partial Update）|✅ 有|
|`DELETE`|刪除資源（Delete）|❌ 無|
|`HEAD`|只獲取 Header，不返回 Body|❌ 無|
|`OPTIONS`|查詢可用的 HTTP 方法|❌ 無|

---

## **📍 4. HTTP Request Headers（請求標頭）**

HTTP Headers **攜帶請求的額外資訊**，例如身份驗證、內容類型等。

### **✅ 常見 Headers**

|**Header**|**用途**|**範例值**|
|---|---|---|
|`Authorization`|API 授權|`Bearer token123`|
|`Content-Type`|請求的數據格式|`application/json`|
|`Accept`|客戶端期望的回應格式|`application/json`|
|`User-Agent`|瀏覽器或應用標識|`Mozilla/5.0`|
|`Cache-Control`|設定快取策略|`no-cache`|
|`Origin`|CORS 跨域請求來源|`https://example.com`|

---

## **📍 5. HTTP Request vs Response**

|**比較項目**|**HTTP Request**|**HTTP Response**|
|---|---|---|
|**發送方向**|**客戶端 → 伺服器**|**伺服器 → 客戶端**|
|**組成**|方法、URL、Headers、Body|狀態碼、Headers、Body|
|**範例**|`GET /api/users`|`200 OK`|
|**常見 Headers**|`Content-Type`、`Authorization`|`Set-Cookie`、`Cache-Control`|

---

## **📍 6. HTTP Request 相關問題（面試應對）**

### **✅ Q1: `GET` 和 `POST` 的差異？**

✅ **回答**： 
1️⃣ `GET` **不應該有 Body，數據透過 Query Parameters 傳遞**  
2️⃣ `POST` **有 Request Body，適合傳送大量數據（如 JSON、表單）**  
3️⃣ `GET` **可快取（Cache），`POST` 不可快取**  
4️⃣ `GET` **通常用於查詢，`POST` 用於新增資源**

---

### **✅ Q2: HTTP Headers 的作用？**

✅ **回答**：

- **`Authorization`**：用於 API 權限認證（如 `Bearer Token`、`Basic Auth`）
- **`Content-Type`**：指定請求的資料格式（如 `application/json`）
- **`Accept`**：指定客戶端希望伺服器返回的格式
- **`User-Agent`**：告訴伺服器客戶端的類型（如 `Mozilla/5.0`）

---

### **✅ Q3: 什麼是 CORS，如何解決跨域請求問題？**

✅ **回答**： 1️⃣ **CORS（跨來源資源共享，Cross-Origin Resource Sharing）** 是瀏覽器的**安全機制**，用來**限制不同來源的請求**（例如從 `http://localhost:3000` 發送請求到 `https://api.example.com`）。  
2️⃣ 如果 API 伺服器**不允許跨域請求**，瀏覽器會封鎖請求，並拋出 `CORS policy` 錯誤。  
3️⃣ 伺服器可透過 **`Access-Control-Allow-Origin`** 標頭允許跨域。

---

## **📍 7. 如何解決 CORS 問題？**

### **✅ 方式 1：在伺服器端設定 `Access-Control-Allow-Origin`**

📌 **在後端（C# .NET Core）允許 CORS**

csharp

複製編輯

`var builder = WebApplication.CreateBuilder(args);  builder.Services.AddCors(options => {     options.AddPolicy("AllowAllOrigins", policy =>     {         policy.AllowAnyOrigin()               .AllowAnyMethod()               .AllowAnyHeader();     }); });  var app = builder.Build();  app.UseCors("AllowAllOrigins"); // 啟用 CORS  app.MapControllers();  app.Run();`

📌 **效果**

- `AllowAnyOrigin()` → 允許所有來源（可改為 `WithOrigins("https://example.com")`）
- `AllowAnyMethod()` → 允許所有 HTTP 方法（`GET`、`POST` 等）
- `AllowAnyHeader()` → 允許任何 `Headers`

---

### **✅ 方式 2：使用 Nginx 設定 CORS**

📌 **如果 API 部署在 Nginx，可在 `nginx.conf` 添加**

nginx

複製編輯

`server {     location /api/ {         add_header 'Access-Control-Allow-Origin' '*';         add_header 'Access-Control-Allow-Methods' 'GET, POST, OPTIONS';         add_header 'Access-Control-Allow-Headers' 'Authorization, Content-Type';     } }`

📌 **效果**

- 允許所有 `Origin` 請求
- 允許 `GET, POST, OPTIONS` 方法
- 允許 `Authorization` 和 `Content-Type` 標頭

---

### **✅ 方式 3：前端設定 `mode: 'cors'`**

📌 **如果前端使用 `fetch()`，需確保 `mode: 'cors'`**

javascript

複製編輯

`fetch("https://api.example.com/data", {     method: "GET",     mode: "cors", // 確保請求以 CORS 模式發送     headers: {         "Content-Type": "application/json",         "Authorization": "Bearer token123"     } }) .then(response => response.json()) .then(data => console.log(data)) .catch(error => console.error("CORS 錯誤:", error));`

📌 **效果**

- `mode: 'cors'` 讓瀏覽器發送 CORS 請求
- 如果 API 沒有允許跨域，仍然會被瀏覽器阻擋

---

### **✅ 方式 4：使用 `JSONP`（僅適用 `GET` 請求）**

📌 **某些舊系統不支援 CORS，可用 `JSONP` 來繞過限制**

html

複製編輯

`<script> function handleData(data) {     console.log("接收到資料:", data); }  var script = document.createElement("script"); script.src = "https://api.example.com/data?callback=handleData"; document.body.appendChild(script); </script>`

📌 **效果**

- `JSONP` 透過 `<script>` 標籤載入外部 JSON
- 伺服器需要返回 `callback(handleData)` 格式的 JSON
- **缺點**：僅支援 `GET`，不安全，**現代應用不推薦使用**

---

## **📍 8. 常見的 HTTP Request 面試問題**

### **✅ Q4: HTTP `GET` vs `POST`，何時該用哪一種？**

✅ **回答**：

- **`GET`** 用於**讀取數據**（不可修改資料）
- **`POST`** 用於**提交數據**（如表單、JSON）
- **`GET` 可快取（Cache），`POST` 不可快取**
- **`GET` 參數暴露在 URL 中，`POST` 參數在 Body，較安全**

---

### **✅ Q5: HTTP `PUT` vs `PATCH`，有什麼不同？**

✅ **回答**：

- **`PUT`** → **更新整個資源**，如果欄位缺失，可能會被覆蓋
- **`PATCH`** → **只更新部分欄位**，不影響其他數據 📌 **範例**

http

複製編輯

`PUT /api/users/1 {     "name": "Alice",     "age": 26 }` 

📌 **更新整個 `User`，缺少的欄位可能會被刪除**

http

複製編輯

`PATCH /api/users/1 {     "age": 26 }`

📌 **僅更新 `age` 欄位，不影響其他數據**

---

### **✅ Q6: HTTP `OPTIONS` 方法的用途是什麼？**

✅ **回答**：

- **`OPTIONS`** 用於查詢 API **允許的請求方法與 Headers**
- 常用於 **CORS 預檢請求（Preflight Request）**
- 伺服器應返回：

http

複製編輯

`HTTP/1.1 204 No Content Access-Control-Allow-Methods: GET, POST, OPTIONS Access-Control-Allow-Headers: Authorization, Content-Type`

---

## **📌 總結**

1️⃣ **HTTP Request 包含 `Method`、`URL`、`Headers`、`Body`**  
2️⃣ **常見 HTTP 方法：`GET`（讀取）、`POST`（新增）、`PUT`（更新）、`DELETE`（刪除）**  
3️⃣ **CORS 是跨域安全機制，可透過 `Access-Control-Allow-Origin` 允許跨域請求**  
4️⃣ **可透過後端 `CORS` 設定、前端 `mode: 'cors'`、Nginx 來解決跨域問題**

---

🔥 **面試技巧** 如果面試官問：「如何解決 CORS 問題？」  
✅ **回答**： 
1️⃣ **伺服器設定 `Access-Control-Allow-Origin`，允許特定域名請求**  
2️⃣ **設定 `Access-Control-Allow-Methods`，允許 `GET, POST, OPTIONS` 方法**  
3️⃣ **在 ASP.NET Core 內建 CORS 設定 `AllowAnyOrigin()`**  
4️⃣ **如果 API 不支援 CORS，可考慮 JSONP（僅適用 GET）**